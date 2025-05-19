using UnityEngine;

public class TouchInitializer : MonoBehaviour
{
    public GameObject touchControlsUI;

    void Start()
    {
        if (DeviceDetector.isTouchDevice && touchControlsUI != null)
        {
            touchControlsUI.SetActive(true);
            Debug.Log("Dispositivo tactil detectado: activando interfaz tactil.");
        }
        else
        {
            Debug.Log("No se detecto dispositivo táctil.");
        }
    }
}