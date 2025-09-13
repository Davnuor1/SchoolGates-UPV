using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class GateOfEmotionsUIManager : MonoBehaviour
{
    [Header("Localización")]
    //public GateOfEmotionsLocalization localization;
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

    private List<string> selectedEmotions = new List<string>();
    private string selectedEmotion1 = "";
    private string selectedEmotion2 = "";
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
        if (codeLanguage == "es") { localization = localizacionES; }
        else if (codeLanguage == "it") { localization = localizacionIT; }
        else if (codeLanguage == "de") { localization = localizacionDE; }
        else if (codeLanguage == "en") { localization = localizacionEN; }
        else if (codeLanguage == "fi") { localization = localizacionFI; }
    }
    public void StartIntro()
    {
        panelIntro.SetActive(true);
        panelEmotionSelector.SetActive(false);
        panelHeaven.SetActive(false);
        panelFeedback.SetActive(false);

        selectedEmotions.Clear();
        selectedEmotion1 = "";
        selectedEmotion2 = "";
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

    public void SelectEmotion(string emotion)
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
                // El cielo se mostrará después de la conversación automáticamente
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

        // Aplica el efecto visual de los neones con la emoción sentida en el cielo anterior


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
        else if (currentHeavenStep == 2)
        {
            textQuestion.text = localization.question3;
            panelButtons.SetActive(false);

            buttonAnger.onClick.RemoveAllListeners();
            buttonFear.onClick.RemoveAllListeners();
            buttonJoy.onClick.RemoveAllListeners();
            buttonSadness.onClick.RemoveAllListeners();

            buttonAnger.onClick.AddListener(() => FinalEmotionSelected(localization.anger));
            buttonFear.onClick.AddListener(() => FinalEmotionSelected(localization.fear));
            buttonJoy.onClick.AddListener(() => FinalEmotionSelected(localization.joy));
            buttonSadness.onClick.AddListener(() => FinalEmotionSelected(localization.sadness));

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

    private void FinalEmotionSelected(string emotion)
    {
        List<string> correctEmotions = GetCorrectEmotions();

        if (correctEmotions.Contains(emotion.ToLower()))
        {
            ShowFeedback(true, emotion.ToLower());
        }
        else
        {
            ShowFeedback(false, "");
        }
    }

    private List<string> GetCorrectEmotions()
    {
        var result = new List<string>();

        if (answerPleasantness == "unpleasant" && answerEnergy == "low")
            result.Add(localization.sadness.ToLower());
        else if (answerPleasantness == "pleasant" && answerEnergy == "high")
            result.Add(localization.joy.ToLower());
        else if (answerPleasantness == "unpleasant" && answerEnergy == "high")
        {
            result.Add(localization.fear.ToLower());
            result.Add(localization.anger.ToLower()); //  ahora también es válida
        }

        return result;
    }


    private void ShowFeedback(bool isCorrect, string emotion)
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
            string feedback = GetFeedbackForEmotion(emotion);
            textFeedback.text = $"Esto parece acertado... Esta bien sentir {emotion}. {feedback} Ahora, vuelve a la ciudad y encuentra una manera de seguir tu camino por el laberinto...";


            if (neonManager != null)
            {
                neonManager.ChangeNeonsToEmotion(emotion);
            }

            buttonNextFromFeedback.onClick.RemoveAllListeners();
            buttonNextFromFeedback.onClick.AddListener(() =>
            {
                StartCoroutine(FadeBackToCity());
            });
        }
    }

    private string GetFeedbackForEmotion(string emotion)
    {
        switch (emotion.ToLower())
        {
            case "ira": return localization.feedbackAnger;
            case "miedo": return localization.feedbackFear;
            case "felicidad": return localization.feedbackJoy;
            case "tristeza": return localization.feedbackSadness;
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
            // Aquí podrías activar el final del minijuego o una nueva etapa
            Debug.Log("Has completado ambas situaciones del Gate of Emotions.");
            // Parte 1 completada. Lanzamos la Parte 2.
            List<string> emocionesRestantes = GetUnchosenEmotions(selectedEmotion1, selectedEmotion2);
            Debug.Log("Emocionesrestantes:" + emocionesRestantes);
            part2Manager.StartSecondStage(emocionesRestantes);

        }
        canvasGateOfEmotions.SetActive(false);
        changeSceneAnimator.SetTrigger("FadeIn");
    }
    private List<string> GetUnchosenEmotions(string e1, string e2)
    {
        var todas = new List<string> { "ira", "miedo", "felicidad", "tristeza" };
        todas.Remove(e1.ToLower());
        todas.Remove(e2.ToLower());
        return todas;
    }
    
}
