using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class EmotionsGardenUI : MonoBehaviour
{
    [Header("UI")]
    public GameObject canvasGateOfEmotions;
    public GameObject panelGardenIntro;
    public TextMeshProUGUI textGarden;
    public Button buttonNext;

    [Header("Localization")]
    //public EmotionsFrustrationLocalization localization;
    [Header("Traducciones")]
    [SerializeField] private EmotionsFrustrationLocalization localizacionES;
    [SerializeField] private EmotionsFrustrationLocalization localizacionIT;
    [SerializeField] private EmotionsFrustrationLocalization localizacionDE;
    [SerializeField] private EmotionsFrustrationLocalization localizacionEN;
    [SerializeField] private EmotionsFrustrationLocalization localizacionFI;
    public EmotionsFrustrationLocalization localization;
    private string codeLanguage;

    private int currentStep = 0;

    private void Start()
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
    public void StartGardenIntro()
    {
        canvasGateOfEmotions.SetActive(true);
        panelGardenIntro.SetActive(true);
        currentStep = 0;

        textGarden.text = localization.gardenText1;
        buttonNext.GetComponentInChildren<TextMeshProUGUI>().text = localization.gardenNextButtonText;

        buttonNext.onClick.RemoveAllListeners();
        buttonNext.onClick.AddListener(NextStep);
    }

    private void NextStep()
    {
        currentStep++;

        if (currentStep == 1)
        {
            textGarden.text = localization.gardenText2;
        }
        else
        {
            panelGardenIntro.SetActive(false);
            canvasGateOfEmotions.SetActive(false);
        }
    }
}
