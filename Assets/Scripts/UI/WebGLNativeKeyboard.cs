using UnityEngine;
using TMPro;
using System.Runtime.InteropServices;

public class WebGLNativeKeyboard : MonoBehaviour
{
#if UNITY_WEBGL && !UNITY_EDITOR
    [DllImport("__Internal")] private static extern void MK_Init();
    [DllImport("__Internal")] private static extern void MK_Show(string type);
    [DllImport("__Internal")] private static extern void MK_Hide();
    [DllImport("__Internal")] private static extern void MK_SetValue(string val);
    [DllImport("__Internal")] private static extern void MK_SetReceiver(string goName);
#endif

    [SerializeField] private TMP_InputField target;
    [Header("Behaviour")]
    public bool onlyOnTouchDevices = true; // usa tu DeviceDetector.isTouchDevice si quieres

    private void Awake()
    {
        if (target == null) target = GetComponent<TMP_InputField>();
#if UNITY_WEBGL && !UNITY_EDITOR
        // Crea el input HTML invisible si no existe
        MK_Init();
#endif
    }

    private void OnEnable()
    {
        if (target != null)
        {
            target.onSelect.AddListener(OnSelectInput);
            target.onDeselect.AddListener(OnDeselectInput);
            target.onValueChanged.AddListener(OnValueChanged);
        }
    }

    private void OnDisable()
    {
        if (target != null)
        {
            target.onSelect.RemoveListener(OnSelectInput);
            target.onDeselect.RemoveListener(OnDeselectInput);
            target.onValueChanged.RemoveListener(OnValueChanged);
        }
    }

    private void OnSelectInput(string _)
    {
        // Solo abrir teclado en táctil si así lo decides
        if (onlyOnTouchDevices && !DeviceDetector.isTouchDevice) return;

#if UNITY_WEBGL && !UNITY_EDITOR
        // Este componente será el receptor de las teclas a partir de ahora
        MK_SetReceiver(gameObject.name);

        // Sincroniza valor actual al input HTML
        MK_SetValue(target.text);

        // Tipo de teclado: como no quieres ocultar password, usa "text"
        // (si quisieras asteriscos, pondrías: target.contentType == Password ? "password" : "text")
        MK_Show("text");
#endif
    }

    private void OnDeselectInput(string _)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        MK_Hide();
        // Opcional: limpiar receptor para evitar que otra escritura llegue a este GO por error
        MK_SetReceiver(null);
#endif
    }

    private void OnValueChanged(string s)
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        // Mantén sincronizado el HTML input si el usuario escribe con teclado físico en desktop
        MK_SetValue(s);
#endif
    }

    // Llamado desde JS cuando el usuario teclea en el teclado nativo
    public void OnMobileKeyboardInput(string value)
    {
        if (target == null) return;
        target.text = value;
        target.caretPosition = value.Length;
        target.ForceLabelUpdate();
    }
}
