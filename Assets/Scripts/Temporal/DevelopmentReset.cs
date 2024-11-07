using UnityEngine;

public class DevelopmentReset : MonoBehaviour
{
    private void Awake()
    {
        Debug.Log("DevelopmentReset Awake called");
        // Llama a la función ResetPortalStates al iniciar el juego
        ActivableObject.ResetPortalStates();
    }
}
