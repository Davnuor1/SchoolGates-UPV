using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class UILocalizedText : MonoBehaviour
{
    public SimpleTextTable table;
    public string key;

    private TextMeshProUGUI label;

    private void Awake()
    {
        label = GetComponent<TextMeshProUGUI>();
    }

    private void OnEnable()
    {
        if (LocalizationManager.Instance != null)
            LocalizationManager.Instance.OnLanguageChanged += Refresh;

        Refresh();
    }

    private void OnDisable()
    {
        if (LocalizationManager.Instance != null)
            LocalizationManager.Instance.OnLanguageChanged -= Refresh;
    }

    public void Refresh()
    {
        if (label == null || table == null) return;

        string lang = (LocalizationManager.Instance != null)
            ? LocalizationManager.Instance.CurrentLanguage
            : "es";

        label.text = table.Get(key, lang);
    }
}
