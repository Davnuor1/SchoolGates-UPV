using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class UILocalizedTextLegacy : MonoBehaviour
{
    public SimpleTextTable table;
    public string key;

    private Text label;

    private void Awake()
    {
        label = GetComponent<Text>();
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
