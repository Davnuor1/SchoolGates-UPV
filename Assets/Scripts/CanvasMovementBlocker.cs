using UnityEngine;

public class CanvasMovementBlocker : MonoBehaviour
{
    public TogglePlayerMovement togglePlayerMovement;

    private void OnEnable()
    {
        if (togglePlayerMovement != null)
        {
            togglePlayerMovement.ToggleMovementPlayerOFF();
        }
    }

    private void OnDisable()
    {
        if (togglePlayerMovement != null)
        {
            togglePlayerMovement.ToggleMovementPlayerON();
        }
    }
}
