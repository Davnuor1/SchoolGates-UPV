using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class EmotionsFrustrationMeditation : MonoBehaviour
{
    [Header("Panel UI")]
    public GameObject panelMeditation;
    public TextMeshProUGUI textMessage;
    public Button buttonNext;
    public Image imageBird;

    [Header("Fade")]
    public Animator fadeAnimator;
    public float fadeDuration = 1.2f;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip bellClip;
    public AudioClip breathingClip;

    [Header("Canvas del Minijuego")]
    public GameObject canvasGateOfEmotions;

    [Header("Duraciones (segundos)")]
    public float closeEyesDuration = 5f;
    public float breathingDuration = 5f;
    public float pauseDuration = 5f;

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
    [Header("Overlay negro estático")]
    public GameObject blackOverlay;

    private List<string> messages = new();
    private int currentIndex = 0;
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
    public void StartMeditation()
    {
        canvasGateOfEmotions.SetActive(true);
        panelMeditation.SetActive(true);
        currentIndex = 0;

        messages.Clear();
        messages.Add(localization.meditationDialogue1);
        messages.Add(localization.meditationDialogue2);
        messages.Add(localization.meditationDialogue3);
        messages.Add(localization.meditationInstruction1); // Cerrar ojos
        messages.Add(localization.meditationInstruction2); // Respirar
        messages.Add(localization.meditationInstruction3); // Esperar
        messages.Add(localization.meditationFinalText1);
        messages.Add(localization.meditationFinalText2);
        messages.Add(localization.meditationFinalText3);

        textMessage.text = messages[0];
        buttonNext.GetComponentInChildren<TextMeshProUGUI>().text = localization.meditationNextButtonText;
        buttonNext.onClick.RemoveAllListeners();
        buttonNext.onClick.AddListener(NextDialogue);
    }

    private void NextDialogue()
    {
        currentIndex++;

        if (currentIndex >= messages.Count)
        {
            EndMeditation();
            return;
        }

        switch (currentIndex)
        {
            case 3:
                StartCoroutine(FadeAndWaitThenPlayBell(messages[currentIndex]));
                break;
            case 4:
                StartCoroutine(PlayBreathingWithMessage(messages[currentIndex], breathingDuration));
                break;
            case 5:
                StartCoroutine(FadeAndWaitWithBreathing(messages[currentIndex], pauseDuration));
                break;
            default:
                textMessage.text = messages[currentIndex];
                break;
        }
    }

    private IEnumerator FadeAndWaitThenPlayBell(string message)
    {
        buttonNext.interactable = false;
        textMessage.text = message;
        fadeAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(fadeDuration);
        blackOverlay.SetActive(true);

        

        yield return new WaitForSeconds(closeEyesDuration);

        audioSource.loop = false;
        audioSource.clip = bellClip;
        audioSource.Play();

        fadeAnimator.SetTrigger("FadeIn");
        blackOverlay.SetActive(false); //Desactivar pantalla negra
        yield return new WaitForSeconds(fadeDuration);

        buttonNext.interactable = true;
    }

    private IEnumerator PlayBreathingWithMessage(string message, float duration)
    {
        textMessage.text = message;
        buttonNext.interactable = false;

        audioSource.loop = true;
        audioSource.clip = breathingClip;
        audioSource.Play();

        yield return new WaitForSeconds(duration);

        audioSource.Stop();
        audioSource.loop = false;

        buttonNext.interactable = true;
    }

    private IEnumerator FadeAndWaitWithBreathing(string message, float duration)
    {
        buttonNext.interactable = false;
        fadeAnimator.SetTrigger("FadeOut");
        yield return new WaitForSeconds(fadeDuration);

        textMessage.text = message;

        audioSource.loop = true;
        audioSource.clip = breathingClip;
        audioSource.Play();

        yield return new WaitForSeconds(duration);

        audioSource.Stop();
        audioSource.loop = false;

        fadeAnimator.SetTrigger("FadeIn");
        yield return new WaitForSeconds(fadeDuration);

        buttonNext.interactable = true;
    }

    private void EndMeditation()
    {
        panelMeditation.SetActive(false);
        canvasGateOfEmotions.SetActive(false);
        Debug.Log("Meditación completada.");
    }
}
