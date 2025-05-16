using UnityEngine;

public static class VirtualInput
{
    private static bool virtualEPressed = false;

    /// <summary>
    /// Llamado por TouchInputBridge para indicar que se ha pulsado la E táctil.
    /// </summary>
    public static void PressVirtualE()
    {
        virtualEPressed = true;
    }

    /// <summary>
    /// Devuelve true una vez por frame si se pulsó la tecla E real o la virtual.
    /// </summary>
    public static bool GetKeyDownE()
    {
        return virtualEPressed || Input.GetKeyDown(KeyCode.E);
    }

    /// <summary>
    /// Llamado desde un MonoBehaviour al final del frame (LateUpdate) para reiniciar el estado.
    /// </summary>
    public static void ResetInput()
    {
        virtualEPressed = false;
    }
}
