using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GateOfEmotionsUIManager : MonoBehaviour
{
    [Header("Traducciones")]
    [SerializeField] private GateOfEmotionsLocalization localizacionES;
    [SerializeField] private GateOfEmotionsLocalization localizacionIT;
    [SerializeField] private GateOfEmotionsLocalization localizacionDE;
    [SerializeField] private GateOfEmotionsLocalization localizacionEN;
    [SerializeField] private GateOfEmotionsLocalization localizacionFI;
    public GateOfEmotionsLocalization localization;
    private string codeLanguage;

    [Header("Paneles")]
    public GameObject panelIntro;
    public GameObject panelEmotionSelector;
    public GameObject panelHeaven;
    public GameObject panelFeedback;

    [Header("Elementos Intro")]
    public TextMeshProUGUI textIntro;
    public Button buttonNextIntro;

    [Header("Selector de emociones")]
    public TextMeshProUGUI textInstruction;
    public Button buttonAnger;
    public Button buttonFear;
    public Button buttonJoy;
    public Button buttonSadness;

    [Header("Heaven - Preguntas")]
    public TextMeshProUGUI textQuestion;
    public GameObject panelButtons;
    public Button buttonOption1;
    public Button buttonOption2;
    public TextMeshProUGUI textOption1;
    public TextMeshProUGUI textOption2;

    [Header("Feedback final")]
    public TextMeshProUGUI textFeedback;
    public Button buttonNextFromFeedback;

    [Header("Gestor de NPCs")]
    public GateOfEmotionsNPCManager npcManager;

    [Header("Transición con Fade")]
    public Animator changeSceneAnimator;
    public float fadeDuration = 1.2f;

    [Header("Canvas del Minijuego")]
    public GameObject canvasGateOfEmotions;

    [Header("Gestor de Neones")]
    public NeonEmotionManager neonManager;

    [Header("Parte 2")]
    public GateOfEmotionsUIManager_Part2 part2Manager;

    private List<EmotionId> selectedEmotions = new List<EmotionId>();
    private EmotionId selectedEmotion1;
    private EmotionId selectedEmotion2;

    private int currentHeavenStep = 0;
    private string answerPleasantness = "";
    private string answerEnergy = "";
    private bool isAfterFirstNPC = true;

    private void Start()
    {
        defineLanguage();

        panelIntro.SetActive(true);
        panelEmotionSelector.SetActive(false);
        panelHeaven.SetActive(false);
        panelFeedback.SetActive(false);

        textIntro.text = localization.introductionText;
        textInstruction.text = localization.selectTwoEmotionsText;

        buttonAnger.GetComponentInChildren<TextMeshProUGUI>().text = localization.anger;
        buttonFear.GetComponentInChildren<TextMeshProUGUI>().text = localization.fear;
        buttonJoy.GetComponentInChildren<TextMeshProUGUI>().text = localization.joy;
        buttonSadness.GetComponentInChildren<TextMeshProUGUI>().text = localization.sadness;

        textFeedback.text = "";
    }

    public void defineLanguage()
    {
        codeLanguage = LocalizationManager.Instance.CurrentLanguage;
        if (codeLanguage == "es") localization = localizacionES;
        else if (codeLanguage == "it") localization = localizacionIT;
        else if (codeLanguage == "de") localization = localizacionDE;
        else if (codeLanguage == "en") localization = localizacionEN;
        else if (codeLanguage == "fi") localization = localizacionFI;
        else localization = localizacionEN;
    }

    public void StartIntro()
    {
        panelIntro.SetActive(true);
        panelEmotionSelector.SetActive(false);
        panelHeaven.SetActive(false);
        panelFeedback.SetActive(false);

        selectedEmotions.Clear();
        currentHeavenStep = 0;
        answerPleasantness = "";
        answerEnergy = "";
        isAfterFirstNPC = true;
    }

    public void OnClickNextIntro()
    {
        panelIntro.SetActive(false);
        panelEmotionSelector.SetActive(true);
    }

    // Estos 4 métodos puedes conectarlos directamente en los botones del selector inicial.
    public void SelectAnger() => SelectEmotion(EmotionId.Anger);
    public void SelectFear() => SelectEmotion(EmotionId.Fear);
    public void SelectJoy() => SelectEmotion(EmotionId.Joy);
    public void SelectSadness() => SelectEmotion(EmotionId.Sadness);

    private void SelectEmotion(EmotionId emotion)
    {
        if (!selectedEmotions.Contains(emotion))
        {
            selectedEmotions.Add(emotion);

            if (selectedEmotions.Count == 1)
            {
                selectedEmotion1 = emotion;
            }
            else if (selectedEmotions.Count == 2)
            {
                selectedEmotion2 = emotion;
                canvasGateOfEmotions.SetActive(false);
                panelEmotionSelector.SetActive(false);
                npcManager.SpawnFirstNPC(selectedEmotion1);
            }
        }
    }

    public void StartFadeToHeavenAfterConversation()
    {
        StartCoroutine(FadeToHeaven());
    }

    private IEnumerator FadeToHeaven()
    {
        changeSceneAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(fadeDuration);
        StartHeavenSequence();
        changeSceneAnimator.SetTrigger("FadeIn");
    }

    public void StartHeavenSequence()
    {
        currentHeavenStep = 0;
        answerPleasantness = "";
        answerEnergy = "";
        canvasGateOfEmotions.SetActive(true);
        panelHeaven.SetActive(true);
        panelButtons.SetActive(true);
        panelFeedback.SetActive(false);
        panelEmotionSelector.SetActive(false);

        ShowCurrentHeavenQuestion();
    }

    private void ShowCurrentHeavenQuestion()
    {
        if (currentHeavenStep == 0)
        {
            textQuestion.text = localization.question1;
            textOption1.text = localization.pleasant;
            textOption2.text = localization.unpleasant;

            buttonOption1.onClick.RemoveAllListeners();
            buttonOption2.onClick.RemoveAllListeners();
            buttonOption1.onClick.AddListener(() => AnswerPleasantness(true));
            buttonOption2.onClick.AddListener(() => AnswerPleasantness(false));
        }
        else if (currentHeavenStep == 1)
        {
            textQuestion.text = localization.question2;
            textOption1.text = localization.highEnergy;
            textOption2.text = localization.lowEnergy;

            buttonOption1.onClick.RemoveAllListeners();
            buttonOption2.onClick.RemoveAllListeners();
            buttonOption1.onClick.AddListener(() => AnswerEnergy(true));
            buttonOption2.onClick.AddListener(() => AnswerEnergy(false));
        }
        else // step 2
        {
            textQuestion.text = localization.question3;
            panelButtons.SetActive(false);

            buttonAnger.onClick.RemoveAllListeners();
            buttonFear.onClick.RemoveAllListeners();
            buttonJoy.onClick.RemoveAllListeners();
            buttonSadness.onClick.RemoveAllListeners();

            buttonAnger.onClick.AddListener(() => FinalEmotionSelected(EmotionId.Anger));
            buttonFear.onClick.AddListener(() => FinalEmotionSelected(EmotionId.Fear));
            buttonJoy.onClick.AddListener(() => FinalEmotionSelected(EmotionId.Joy));
            buttonSadness.onClick.AddListener(() => FinalEmotionSelected(EmotionId.Sadness));

            panelEmotionSelector.SetActive(true);
        }
    }

    private void AnswerPleasantness(bool isPleasant)
    {
        answerPleasantness = isPleasant ? "pleasant" : "unpleasant";
        currentHeavenStep++;
        ShowCurrentHeavenQuestion();
    }

    private void AnswerEnergy(bool isHigh)
    {
        answerEnergy = isHigh ? "high" : "low";
        currentHeavenStep++;
        ShowCurrentHeavenQuestion();
    }

    private void FinalEmotionSelected(EmotionId id)
    {
        var correct = GetCorrectEmotionIds();
        if (correct.Contains(id)) ShowFeedback(true, id);
        else ShowFeedback(false, id);
    }

    private List<EmotionId> GetCorrectEmotionIds()
    {
        var result = new List<EmotionId>();

        if (answerPleasantness == "unpleasant" && answerEnergy == "low")
            result.Add(EmotionId.Sadness);
        else if (answerPleasantness == "pleasant" && answerEnergy == "high")
            result.Add(EmotionId.Joy);
        else if (answerPleasantness == "unpleasant" && answerEnergy == "high")
        {
            result.Add(EmotionId.Fear);
            result.Add(EmotionId.Anger);
        }

        return result;
    }

    private void ShowFeedback(bool isCorrect, EmotionId emotionId)
    {
        panelEmotionSelector.SetActive(false);
        panelFeedback.SetActive(true);

        if (!isCorrect)
        {
            textFeedback.text = localization.feedbackIncorrect;
            buttonNextFromFeedback.onClick.RemoveAllListeners();
            buttonNextFromFeedback.onClick.AddListener(() =>
            {
                panelFeedback.SetActive(false);
                panelEmotionSelector.SetActive(true);
            });
        }
        else
        {
            string localized = EmotionUtils.GetLocalizedName(localization, emotionId);
            string extra = GetFeedbackForEmotion(emotionId);
            textFeedback.text =
                $"Esto parece acertado... Esta bien sentir {localized}. {extra} Ahora, vuelve a la ciudad y encuentra una manera de seguir tu camino por el laberinto...";

            if (neonManager != null)
                neonManager.ChangeNeonsToEmotion(emotionId);

            buttonNextFromFeedback.onClick.RemoveAllListeners();
            buttonNextFromFeedback.onClick.AddListener(() =>
            {
                StartCoroutine(FadeBackToCity());
            });
        }
    }

    private string GetFeedbackForEmotion(EmotionId id)
    {
        switch (id)
        {
            case EmotionId.Anger: return localization.feedbackAnger;
            case EmotionId.Fear: return localization.feedbackFear;
            case EmotionId.Joy: return localization.feedbackJoy;
            case EmotionId.Sadness: return localization.feedbackSadness;
            default: return "";
        }
    }

    private IEnumerator FadeBackToCity()
    {
        changeSceneAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(fadeDuration);

        panelFeedback.SetActive(false);
        panelHeaven.SetActive(false);

        if (isAfterFirstNPC)
        {
            npcManager.SpawnSecondNPC(selectedEmotion2);
            isAfterFirstNPC = false;
        }
        else
        {
            var restantes = GetUnchosenEmotions(selectedEmotion1, selectedEmotion2);
            part2Manager.StartSecondStage(restantes);
        }

        canvasGateOfEmotions.SetActive(false);
        changeSceneAnimator.SetTrigger("FadeIn");
    }

    private List<EmotionId> GetUnchosenEmotions(EmotionId e1, EmotionId e2)
    {
        var all = new List<EmotionId> { EmotionId.Anger, EmotionId.Fear, EmotionId.Joy, EmotionId.Sadness };
        all.Remove(e1);
        all.Remove(e2);
        return all;
    }
}
