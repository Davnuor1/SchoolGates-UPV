using UnityEngine;
using UnityEngine.UI;

public class LanguageFlagsUI : MonoBehaviour
{
    [System.Serializable]
    public struct FlagEntry
    {
        public string code;            // "es", "en", "de", "it", "fi"
        public Button button;          // botón con la bandera
        public GameObject selectedIndicator; // opcional, para resaltar el activo
    }

    [SerializeField] private FlagEntry[] flags;

    private void Start()
    {
        // Suscribir clicks
        for (int i = 0; i < flags.Length; i++)
        {
            int idx = i;
            if (flags[idx].button != null)
            {
                flags[idx].button.onClick.AddListener(() => OnClickFlag(flags[idx].code));
            }
        }

        // Estado inicial según el UserData del TAN (o "es")
        string code = "es";
        if (UserDataManager.Instance != null && UserDataManager.Instance.currentUserData != null)
        {
            if (!string.IsNullOrEmpty(UserDataManager.Instance.currentUserData.languageCode))
                code = UserDataManager.Instance.currentUserData.languageCode;
        }
        ApplyLanguage(code);
        RefreshIndicators(code);
    }

    private void OnDestroy()
    {
        for (int i = 0; i < flags.Length; i++)
        {
            if (flags[i].button != null)
                flags[i].button.onClick.RemoveAllListeners();
        }
    }

    private void OnClickFlag(string code)
    {
        ApplyLanguage(code);

        // Persistir en UserData del TAN actual
        if (UserDataManager.Instance != null && UserDataManager.Instance.currentUserData != null)
        {
            UserDataManager.Instance.currentUserData.languageCode = code;
            LocalJsonSave.SaveUserData(UserDataManager.Instance.currentUserData);
        }

        RefreshIndicators(code);
    }

    private void ApplyLanguage(string code)
    {
        if (LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.SetLanguage(code);
            Debug.Log("Lenguaje cambiado a"+code);
        }
    }

    private void RefreshIndicators(string activeCode)
    {
        for (int i = 0; i < flags.Length; i++)
        {
            if (flags[i].selectedIndicator != null)
            {
                flags[i].selectedIndicator.SetActive(flags[i].code == activeCode);
            }
        }
    }
}
