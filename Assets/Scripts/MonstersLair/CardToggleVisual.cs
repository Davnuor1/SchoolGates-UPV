using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class CardToggleVisual : MonoBehaviour
{
    [SerializeField] private Image target;     // p.ej. CardImage o un borde
    [SerializeField] private Color onColor = Color.white;
    [SerializeField] private Color offColor = new Color(1f, 1f, 1f, 0.6f);

    private Toggle t;

    private void Awake()
    {
        t = GetComponent<Toggle>();
        if (t != null)
            t.onValueChanged.AddListener(OnToggleChanged);
    }

    private void OnEnable()
    {
        Apply(t != null && t.isOn);
    }

    private void OnDestroy()
    {
        if (t != null)
            t.onValueChanged.RemoveListener(OnToggleChanged);
    }

    private void OnToggleChanged(bool isOn)
    {
        Apply(isOn);
    }

    private void Apply(bool isOn)
    {
        if (target != null)
            target.color = isOn ? onColor : offColor;
    }
}
