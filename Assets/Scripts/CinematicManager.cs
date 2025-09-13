using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CinematicManager : MonoBehaviour
{
    //public CinematicData cinematicData;
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
    private string codeLanguage;
    private void Awake()
    {
        fadePanel.alpha = 1; // Asegurar que empieza en negro
        fadePanel.gameObject.SetActive(true);

        // Desactivar imagen y texto para evitar que se vean antes del primer fade in
        vignetteImage.gameObject.SetActive(false);
        textPanel.SetActive(false);
    }

    private void Start()
    {
        defineLanguage();
        if (cinematicData != null)
        {
            nextButtonText.text = cinematicData.nextButtonText;
            StartCoroutine(StartCinematic());
        }
    }
    public void defineLanguage()
    {
        codeLanguage = LocalizationManager.Instance.CurrentLanguage;
        if (codeLanguage == "es") { cinematicData = localizacionES; }
        else if (codeLanguage == "it") { cinematicData = localizacionIT; }
        else if (codeLanguage == "de") { cinematicData = localizacionDE; }
        else if (codeLanguage == "en") { cinematicData = localizacionEN; }
        else if (codeLanguage == "fi") { cinematicData = localizacionFI; }
    }
    private IEnumerator StartCinematic()
    {
        yield return FadeIn(); // Hacer fade in
        vignetteImage.gameObject.SetActive(true);
        textPanel.SetActive(true);
        ShowVignette(0);
    }

    public void ShowVignette(int index)
    {
        if (index >= 0 && index < cinematicData.vignettes.Count)
        {
            vignetteImage.sprite = cinematicData.vignettes[index].image;
            dialogueText.text = cinematicData.vignettes[index].text;
            currentVignetteIndex = index;
        }
        else
        {
            StartCoroutine(FadeOutAndEnd()); // Fade out final antes de cerrar la cinemática
        }
    }

    public void NextVignette()
    {
        if (!isTransitioning)
        {
            StartCoroutine(TransitionToNextVignette());
        }
    }

    private IEnumerator TransitionToNextVignette()
    {
        isTransitioning = true;
        yield return FadeOut(); // Fade Out antes de cambiar de viñeta
        ShowVignette(currentVignetteIndex + 1);
        yield return FadeIn(); // Fade In después de cambiar
        isTransitioning = false;
    }

    private IEnumerator FadeIn()
    {
        fadePanel.gameObject.SetActive(true);
        float alpha = 1f;
        while (alpha > 0)
        {
            alpha -= Time.deltaTime * 2;
            fadePanel.alpha = alpha;
            yield return null;
        }
        fadePanel.alpha = 0;
        fadePanel.gameObject.SetActive(false);
    }

    private IEnumerator FadeOut()
    {
        fadePanel.gameObject.SetActive(true);
        float alpha = 0f;
        while (alpha < 1)
        {
            alpha += Time.deltaTime * 2;
            fadePanel.alpha = alpha;
            yield return null;
        }
        fadePanel.alpha = 1;
    }

    private IEnumerator FadeOutAndEnd()
    {
        //  Desactivar imagen y texto antes del fade out final
        vignetteImage.gameObject.SetActive(false);
        textPanel.SetActive(false);

        yield return FadeOut();
        EndCinematic();
    }

    private void EndCinematic()
    {
        gameObject.SetActive(false);
        PlayerPrefs.SetInt("Cinematic_" + cinematicData.name, 1);
    }
}
