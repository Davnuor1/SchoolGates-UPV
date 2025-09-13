using UnityEngine;
using System;

public class LocalizationManager : MonoBehaviour
{
    public static LocalizationManager Instance { get; private set; }

    public string CurrentLanguage { get; private set; } = "es";

    public event Action OnLanguageChanged;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void SetLanguage(string lang)
    {
        if (string.IsNullOrEmpty(lang)) return;
        if (lang == CurrentLanguage) return;

        CurrentLanguage = lang;

        // Aplica al Dialogue System (asegúrate de que tu DB tiene estos idiomas):
        PixelCrushers.DialogueSystem.DialogueManager.SetLanguage(lang);
        Debug.Log("Lenguaje cambiado a" + lang);

        if (OnLanguageChanged != null) OnLanguageChanged.Invoke();
    }
}
