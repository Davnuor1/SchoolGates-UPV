using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class CinematicManager : MonoBehaviour
{
    [Header("UI")]
    public Image vignetteImage;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;
    public TextMeshProUGUI nextButtonText;
    public GameObject textPanel;
    public CanvasGroup fadePanel;

    private int currentVignetteIndex = 0;
    private bool isTransitioning = false;

    [Header("Traducciones")]
    [SerializeField] private CinematicData localizacionES;
    [SerializeField] private CinematicData localizacionIT;
    [SerializeField] private CinematicData localizacionDE;
    [SerializeField] private CinematicData localizacionEN;
    [SerializeField] private CinematicData localizacionFI;
    public CinematicData cinematicData;

    [Header("Clave de guardado (opcional)")]
    [Tooltip("Si lo rellenas, se usará esta clave para PlayerPrefs. Ideal para resetear desde scripts de desarrollo.")]
    [SerializeField] private string cinematicId = "";

    [Tooltip("Incluye el idioma en la clave cuando usas cinematicId. Desactiva si quieres que ver-la-una-vez aplique a todos los idiomas.")]
    [SerializeField] private bool includeLanguageInId = true;

    private const string CIN_NAMESPACE = "CIN_v1";
    private string codeLanguage = "es";
    private string seenKey;

    private void Awake()
    {
        if (fadePanel != null)
        {
            fadePanel.alpha = 1f;
            fadePanel.gameObject.SetActive(true);
        }
        if (vignetteImage != null) vignetteImage.gameObject.SetActive(false);
        if (textPanel != null) textPanel.SetActive(false);
    }

    private void Start()
    {
        defineLanguage();

        // Construir la clave
        seenKey = BuildSeenKey(cinematicId, codeLanguage, cinematicData);

        // Si ya se vio, salir
        if (PlayerPrefs.GetInt(seenKey, 0) == 1)
        {
            gameObject.SetActive(false);
            return;
        }

        // Preparar UI y arrancar
        if (cinematicData != null)
        {
            if (nextButtonText != null) nextButtonText.text = cinematicData.nextButtonText;
            if (nextButton != null)
            {
                nextButton.onClick.RemoveListener(NextVignette);
                nextButton.onClick.AddListener(NextVignette);
            }
            StartCoroutine(StartCinematic());
        }
        else
        {
            Debug.LogWarning("CinematicManager: 'cinematicData' no asignado para el idioma actual.");
            gameObject.SetActive(false);
        }
    }

    private string BuildSeenKey(string id, string lang, CinematicData data)
    {
        if (!string.IsNullOrEmpty(id))
        {
            // Clave basada en ID manual
            if (includeLanguageInId)
                return $"CIN_{CIN_NAMESPACE}_{id}_{lang}";
            else
                return $"CIN_{CIN_NAMESPACE}_{id}";
        }
        else
        {
            // Clave automática: producto + escena + idioma + nombre del asset
            string sceneName = SceneManager.GetActiveScene().name;
            string cinematicName = (data != null ? data.name : "DefaultCinematic");
            return $"CIN_{CIN_NAMESPACE}_{Application.productName}_{sceneName}_{lang}_{cinematicName}";
        }
    }

    public void defineLanguage()
    {
        codeLanguage = (LocalizationManager.Instance != null ? LocalizationManager.Instance.CurrentLanguage : "es");
        if (codeLanguage == "es") cinematicData = localizacionES;
        else if (codeLanguage == "it") cinematicData = localizacionIT;
        else if (codeLanguage == "de") cinematicData = localizacionDE;
        else if (codeLanguage == "en") cinematicData = localizacionEN;
        else if (codeLanguage == "fi") cinematicData = localizacionFI;
        else cinematicData = localizacionES;
    }

    private IEnumerator StartCinematic()
    {
        yield return FadeIn();
        if (vignetteImage != null) vignetteImage.gameObject.SetActive(true);
        if (textPanel != null) textPanel.SetActive(true);
        ShowVignette(0);
    }

    public void ShowVignette(int index)
    {
        if (cinematicData == null || cinematicData.vignettes == null)
        {
            StartCoroutine(FadeOutAndEnd());
            return;
        }

        if (index >= 0 && index < cinematicData.vignettes.Count)
        {
            if (vignetteImage != null) vignetteImage.sprite = cinematicData.vignettes[index].image;
            if (dialogueText != null) dialogueText.text = cinematicData.vignettes[index].text;
            currentVignetteIndex = index;
        }
        else
        {
            StartCoroutine(FadeOutAndEnd());
        }
    }

    public void NextVignette()
    {
        if (!isTransitioning) StartCoroutine(TransitionToNextVignette());
    }

    private IEnumerator TransitionToNextVignette()
    {
        isTransitioning = true;
        yield return FadeOut();
        ShowVignette(currentVignetteIndex + 1);
        yield return FadeIn();
        isTransitioning = false;
    }

    private IEnumerator FadeIn()
    {
        if (fadePanel == null) yield break;
        fadePanel.gameObject.SetActive(true);
        float alpha = 1f;
        while (alpha > 0f)
        {
            alpha -= Time.deltaTime * 2f;
            fadePanel.alpha = alpha;
            yield return null;
        }
        fadePanel.alpha = 0f;
        fadePanel.gameObject.SetActive(false);
    }

    private IEnumerator FadeOut()
    {
        if (fadePanel == null) yield break;
        fadePanel.gameObject.SetActive(true);
        float alpha = 0f;
        while (alpha < 1f)
        {
            alpha += Time.deltaTime * 2f;
            fadePanel.alpha = alpha;
            yield return null;
        }
        fadePanel.alpha = 1f;
    }

    private IEnumerator FadeOutAndEnd()
    {
        if (vignetteImage != null) vignetteImage.gameObject.SetActive(false);
        if (textPanel != null) textPanel.SetActive(false);
        yield return FadeOut();
        EndCinematic();
    }

    private void EndCinematic()
    {
        PlayerPrefs.SetInt(seenKey, 1);
        PlayerPrefs.Save();
        gameObject.SetActive(false);
    }

    // ====== Reset helpers para usar desde DevelopmentReset u otros ======

    public static void ResetCinematicById(string id, bool includeLanguage, string languageIfNeeded)
    {
        if (string.IsNullOrEmpty(id)) return;
        string lang = includeLanguage ? (string.IsNullOrEmpty(languageIfNeeded) ? "es" : languageIfNeeded) : "";
        string key = includeLanguage ? $"CIN_{CIN_NAMESPACE}_{id}_{lang}" : $"CIN_{CIN_NAMESPACE}_{id}";
        PlayerPrefs.DeleteKey(key);
        PlayerPrefs.Save();
        Debug.Log("Cinematic reset by id: " + key);
    }

    public static void ResetCinematicAuto(string productName, string sceneName, string language, string cinematicAssetName)
    {
        string key = $"CIN_{CIN_NAMESPACE}_{productName}_{sceneName}_{language}_{cinematicAssetName}";
        PlayerPrefs.DeleteKey(key);
        PlayerPrefs.Save();
        Debug.Log("Cinematic reset auto key: " + key);
    }
}
