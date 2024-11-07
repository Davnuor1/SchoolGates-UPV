using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableObject : MonoBehaviour
{
    [SerializeField] private string portalID; // Identificación única para cada portal

    private void Start()
    {
        int portalState = PlayerPrefs.GetInt(portalID, 1);
        Debug.Log($"[ActivableObject] Portal {portalID} estado al iniciar: {portalState}");
        if (portalState == 1)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }

    public void ToggleActivar()
    {
        // Cambia el estado del portal y guarda el estado en PlayerPrefs
        bool isActive = !gameObject.activeSelf;
        gameObject.SetActive(isActive);
        PlayerPrefs.SetInt(portalID, isActive ? 1 : 0);

        // Obtiene la lista existente de portal keys y asegura que este portalID se guarde solo una vez
        var existingKeys = PlayerPrefs.GetString("PortalKeys", "");
        if (!existingKeys.Contains(portalID))
        {
            existingKeys = string.IsNullOrEmpty(existingKeys) ? portalID : existingKeys + "," + portalID;
            PlayerPrefs.SetString("PortalKeys", existingKeys);
            Debug.Log($"[ToggleActivar] Portal ID '{portalID}' agregado a PortalKeys: {existingKeys}");
        }
        else
        {
            Debug.Log($"[ToggleActivar] Portal ID '{portalID}' ya estaba en PortalKeys: {existingKeys}");
        }

        PlayerPrefs.Save(); // Asegura que los cambios se guarden
    }


    public static void ResetPortalStates()
    {
        Debug.Log($"portalKeys raw data: {PlayerPrefs.GetString("PortalKeys", "")}");
        var portalKeys = PlayerPrefs.GetString("PortalKeys", "").Split(',');
        foreach (var key in portalKeys)
        {
            if (!string.IsNullOrEmpty(key))
            {
                PlayerPrefs.DeleteKey(key);
                Debug.Log($"[DevelopmentReset] Clave eliminada: {key}");
            }
        }
        PlayerPrefs.Save(); // Guarda los cambios
    }
}
