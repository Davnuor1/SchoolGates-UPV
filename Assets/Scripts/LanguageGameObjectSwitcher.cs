using UnityEngine;

public class LanguageGameObjectSwitcher : MonoBehaviour
{
    [Header("GameObjects por idioma")]
    [SerializeField] private GameObject goES;
    [SerializeField] private GameObject goEN;
    [SerializeField] private GameObject goDE;
    [SerializeField] private GameObject goIT;
    [SerializeField] private GameObject goFI;

    [Header("Opciones")]
    [SerializeField] private bool includeThisObject = false;
    [SerializeField] private bool runOnStart = true;
    [SerializeField] private bool debugLog = false;

    private void Start()
    {
        if (runOnStart)
        {
            ApplyLanguage();
        }
    }

    // Llamalo si cambias el idioma en runtime y quieres refrescar.
    public void ApplyLanguage()
    {
        string lang = "en";

        if (LocalizationManager.Instance != null)
        {
            lang = LocalizationManager.Instance.CurrentLanguage;
        }

        if (string.IsNullOrEmpty(lang)) lang = "en";
        lang = lang.Trim().ToLowerInvariant();

        // Desactiva todo primero
        SetAll(false);

        // Activa el que corresponda
        if (lang == "es")
            SafeSet(goES, true);
        else if (lang == "en")
            SafeSet(goEN, true);
        else if (lang == "de")
            SafeSet(goDE, true);
        else if (lang == "it")
            SafeSet(goIT, true);
        else if (lang == "fi")
            SafeSet(goFI, true);
        else
            SafeSet(goEN, true); // fallback

        if (includeThisObject)
            gameObject.SetActive(true);

        if (debugLog)
            Debug.Log("LanguageGameObjectSwitcher -> lang=" + lang);
    }

    private void SetAll(bool active)
    {
        SafeSet(goES, active);
        SafeSet(goEN, active);
        SafeSet(goDE, active);
        SafeSet(goIT, active);
        SafeSet(goFI, active);
    }

    private void SafeSet(GameObject go, bool active)
    {
        if (go != null) go.SetActive(active);
    }
}
