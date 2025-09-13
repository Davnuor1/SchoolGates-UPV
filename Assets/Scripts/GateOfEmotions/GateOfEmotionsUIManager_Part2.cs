using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GateOfEmotionsUIManager_Part2 : MonoBehaviour
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
    public GameObject canvasGateOfEmotionsPart2;
    public GameObject panelHeavenPart2;
    public GameObject panelCenterPart2;
    public TextMeshProUGUI textQuestionPart2;
    public TextMeshProUGUI textFeedbackPart2;

    [Header("Botones emociones")]
    public Button buttonGuessAnger;
    public Button buttonGuessFear;
    public Button buttonGuessJoy;
    public Button buttonGuessSadness;

    [Header("Botones respuesta jugador")]
    public Button buttonPlayerResponse1;
    public Button buttonPlayerResponse2;
    public Button buttonPlayerResponse3;

    [Header("Botón de avanzar")]
    public Button buttonNextPart2;

    [Header("NPC Manager")]
    public GateOfEmotionsNPCManager npcManager;

    [Header("Fade")]
    public Animator changeSceneAnimator;
    public float fadeDuration = 1.2f;

    [Header("Gestor de Neones")]
    public NeonEmotionManager neonManager;

    private List<string> emocionesRestantes = new();
    private int currentNPCIndex = 0;
    private string npcActualEmotion = "";

    public void Start()
    {
        defineLanguage();
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
    public void StartSecondStage(List<string> emocionesDescartadas)
    {
        emocionesRestantes = emocionesDescartadas;
        Debug.Log("Emociones restantes ya en parte 2:" + emocionesRestantes);
        currentNPCIndex = 0;
        canvasGateOfEmotionsPart2.SetActive(false);
        SpawnNextNPC();
    }

    private void SpawnNextNPC()
    {
        if (currentNPCIndex >= emocionesRestantes.Count)
        {
            Debug.Log("Parte 2 completada.");
            canvasGateOfEmotionsPart2.SetActive(false);
            return;
        }

        npcActualEmotion = emocionesRestantes[currentNPCIndex];

        if (currentNPCIndex == 0)
        {
            Debug.Log("Spawning:" + npcActualEmotion);
            npcManager.SpawnThirdNPC(npcActualEmotion);
        }
            
        else if (currentNPCIndex == 1)
            npcManager.SpawnFourthNPC(npcActualEmotion);
    }

    public void StartFadeToHeavenPart2()
    {
        StartCoroutine(FadeToHeavenPart2());
    }

    private IEnumerator FadeToHeavenPart2()
    {
        changeSceneAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(fadeDuration);

        ShowHeavenPart2();

        changeSceneAnimator.SetTrigger("FadeIn");
    }

    private void ShowHeavenPart2()
    {
        canvasGateOfEmotionsPart2.SetActive(true);
        panelHeavenPart2.SetActive(true);
        panelCenterPart2.SetActive(true);

        textQuestionPart2.text = localization.questionPart2;
        textFeedbackPart2.text = "";
        buttonNextPart2.gameObject.SetActive(false);

        buttonGuessAnger.gameObject.SetActive(true);
        buttonGuessFear.gameObject.SetActive(true);
        buttonGuessJoy.gameObject.SetActive(true);
        buttonGuessSadness.gameObject.SetActive(true);

        buttonPlayerResponse1.gameObject.SetActive(false);
        buttonPlayerResponse2.gameObject.SetActive(false);
        buttonPlayerResponse3.gameObject.SetActive(false);

        //  Asignar textos localizados a los botones de emoción
        buttonGuessAnger.GetComponentInChildren<TextMeshProUGUI>().text = localization.anger;
        buttonGuessFear.GetComponentInChildren<TextMeshProUGUI>().text = localization.fear;
        buttonGuessJoy.GetComponentInChildren<TextMeshProUGUI>().text = localization.joy;
        buttonGuessSadness.GetComponentInChildren<TextMeshProUGUI>().text = localization.sadness;

        buttonGuessAnger.onClick.RemoveAllListeners();
        buttonGuessFear.onClick.RemoveAllListeners();
        buttonGuessJoy.onClick.RemoveAllListeners();
        buttonGuessSadness.onClick.RemoveAllListeners();

        buttonGuessAnger.onClick.AddListener(() => GuessEmotion("ira"));
        buttonGuessFear.onClick.AddListener(() => GuessEmotion("miedo"));
        buttonGuessJoy.onClick.AddListener(() => GuessEmotion("felicidad"));
        buttonGuessSadness.onClick.AddListener(() => GuessEmotion("tristeza"));
    }

    private void GuessEmotion(string guess)
    {
        if (guess.ToLower() == npcActualEmotion.ToLower())
        {
            textFeedbackPart2.text = localization.respuestaJugador1;
            buttonNextPart2.gameObject.SetActive(true);

            if (neonManager != null)
                neonManager.ChangeNeonsToEmotion(npcActualEmotion);

            buttonNextPart2.onClick.RemoveAllListeners();
            buttonNextPart2.onClick.AddListener(ShowNPCResponse);
        }
        else
        {
            textFeedbackPart2.text = localization.feedbackIncorrectPart2;
            buttonNextPart2.gameObject.SetActive(false);
        }
    }

    private void ShowNPCResponse()
    {
        textFeedbackPart2.text = localization.respuestaNPC1;

        buttonGuessAnger.gameObject.SetActive(false);
        buttonGuessFear.gameObject.SetActive(false);
        buttonGuessJoy.gameObject.SetActive(false);
        buttonGuessSadness.gameObject.SetActive(false);

        buttonPlayerResponse1.gameObject.SetActive(true);
        buttonPlayerResponse2.gameObject.SetActive(true);
        buttonPlayerResponse3.gameObject.SetActive(true);

        buttonPlayerResponse1.GetComponentInChildren<TextMeshProUGUI>().text = localization.botonesRespuestaJugador2[0];
        buttonPlayerResponse2.GetComponentInChildren<TextMeshProUGUI>().text = localization.botonesRespuestaJugador2[1];
        buttonPlayerResponse3.GetComponentInChildren<TextMeshProUGUI>().text = localization.botonesRespuestaJugador2[2];

        buttonNextPart2.gameObject.SetActive(false);

        buttonPlayerResponse1.onClick.RemoveAllListeners();
        buttonPlayerResponse2.onClick.RemoveAllListeners();
        buttonPlayerResponse3.onClick.RemoveAllListeners();

        buttonPlayerResponse1.onClick.AddListener(FinalFeedback);
        buttonPlayerResponse2.onClick.AddListener(FinalFeedback);
        buttonPlayerResponse3.onClick.AddListener(FinalFeedback);
    }

    private void FinalFeedback()
    {
        string feedback = GetFeedbackForEmotion(npcActualEmotion.ToLower());
        textFeedbackPart2.text = feedback;
        buttonNextPart2.gameObject.SetActive(true);

        buttonNextPart2.onClick.RemoveAllListeners();
        buttonNextPart2.onClick.AddListener(() =>
        {
            StartCoroutine(FadeBackToCityPart2());
        });
    }

    private IEnumerator FadeBackToCityPart2()
    {
        changeSceneAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(fadeDuration);

        panelHeavenPart2.SetActive(false);
        canvasGateOfEmotionsPart2.SetActive(false);
        changeSceneAnimator.SetTrigger("FadeIn");
        currentNPCIndex++;
        SpawnNextNPC();
    }

    private string GetFeedbackForEmotion(string emotion)
    {
        return emotion switch
        {
            "ira" => localization.feedbackAnger_Part2,
            "miedo" => localization.feedbackFear_Part2,
            "felicidad" => localization.feedbackJoy_Part2,
            "tristeza" => localization.feedbackSadness_Part2,
            _ => ""
        };
    }
}
