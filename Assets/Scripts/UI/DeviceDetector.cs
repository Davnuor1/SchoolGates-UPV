using UnityEngine;

public class DeviceDetector : MonoBehaviour
{
    public static bool isTouchDevice = false;

    [Header("Solo para pruebas")]
    public GameObject verSoloEnTablet;
    public GameObject touchControlsUI;

    public void SetTouchMode(string value)
    {
        isTouchDevice = value == "true";
        Debug.Log("¿Dispositivo táctil?: " + isTouchDevice);

        if (touchControlsUI != null)
        {
            touchControlsUI.SetActive(isTouchDevice);
        }
    }

    private void Start()
    {
#if UNITY_WEBGL && !UNITY_EDITOR
        DetectTouchDevice();
#endif
    }

    [System.Runtime.InteropServices.DllImport("__Internal")]
    private static extern void DetectTouchDevice();
}
