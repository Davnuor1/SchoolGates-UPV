using UnityEngine;

[CreateAssetMenu(menuName = "Config/Backend Config", fileName = "BackendConfig")]
public class BackendConfig : ScriptableObject
{
    [Header("Apps Script WebApp")]
    public string apiUrl;      // URL de la implementación activa (…/exec)
    public string apiKey;      // La misma clave que pusiste en Apps Script
    public string versionId;   // "v0", "v1", … "v5"

    [Header("Juego")]
    public string[] allowedGates;  // Lo usaremos en la tarea 16
    public bool debugLogging = false;
}
