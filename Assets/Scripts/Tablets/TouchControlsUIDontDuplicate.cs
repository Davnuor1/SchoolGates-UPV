using UnityEngine;

public class TouchControlsUIDontDuplicate : MonoBehaviour
{
    private static TouchControlsUIDontDuplicate instance;

    void Awake()
    {
        if (instance != null && instance != this)
        {
            Debug.LogWarning("TouchControlsUI duplicado detectado y eliminado.");
            Destroy(gameObject);
            return;
        }

        instance = this;
        // NO necesitas otro DontDestroyOnLoad aquí si ya se hizo desde el GameManager
    }
}
