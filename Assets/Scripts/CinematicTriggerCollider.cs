using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicTriggerCollider : MonoBehaviour
{
    [Header("Configuración")]
    public string uniqueID; // ID único para este trigger
    public CinematicManager cinematicManager; // Referencia al manager de la cinemática

    [Header("Traducciones")]
    [SerializeField] private CinematicData localizacionES;
    [SerializeField] private CinematicData localizacionIT;
    [SerializeField] private CinematicData localizacionDE;
    [SerializeField] private CinematicData localizacionEN;
    [SerializeField] private CinematicData localizacionFI;
    public CinematicData cinematicData;
    private string codeLanguage;

    [Header("finales")]
    [SerializeField] public CinematicManager final01;
    [SerializeField] public CinematicManager final02;
    [SerializeField] public CinematicManager final03;
    [SerializeField] public CinematicManager final04;
    [SerializeField] public CinematicManager final05;

    // Si lo rellenas a mano en el inspector tendrá prioridad. Si no, se toma de UserData.finalsChosen[0].
    public string finalId;

    private void Start()
    {
        defineLanguage();

        // Comprobar si este trigger ya fue usado
        if (PlayerPrefs.GetInt("TriggerUsed_" + uniqueID, 0) == 1)
        {
            this.gameObject.SetActive(false);
        }
    }

    public void defineLanguage()
    {
        codeLanguage = LocalizationManager.Instance.CurrentLanguage;
        if (codeLanguage == "es") { cinematicData = localizacionES; }
        else if (codeLanguage == "it") { cinematicData = localizacionIT; }
        else if (codeLanguage == "de") { cinematicData = localizacionDE; }
        else if (codeLanguage == "en") { cinematicData = localizacionEN; }
        else if (codeLanguage == "fi") { cinematicData = localizacionFI; }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player")) return;

        // 1) Resolver final: si no viene del inspector, coger el primero válido de UserData.finalsChosen.
        if (string.IsNullOrEmpty(finalId))
        {
            finalId = GetFinalFromUserData();
        }

        // 2) Asignar el CinematicManager correcto según finalId
        AssignCinematicManagerByFinalId();

        if (cinematicManager == null)
        {
            Debug.LogWarning($"CinematicTriggerCollider: cinematicManager no asignado para finalId='{finalId}'. Revisa referencias o UserData.finalsChosen.");
            return;
        }

        Debug.Log("colision trigger cinematica detectada");
        cinematicManager.gameObject.SetActive(true); // Activa la cinemática

        PlayerPrefs.SetInt("TriggerUsed_" + uniqueID, 1); // Marca como usado

        // Guardar el ID en la lista de triggers usados si no está ya
        string usedTriggers = PlayerPrefs.GetString("AllUsedTriggerIDs", "");
        if (!usedTriggers.Contains(uniqueID + ";"))
        {
            usedTriggers += uniqueID + ";";
            PlayerPrefs.SetString("AllUsedTriggerIDs", usedTriggers);
        }

        PlayerPrefs.Save();
        this.gameObject.SetActive(false); // Desactiva el trigger
    }

    // Lee el primer final válido desde UserDataManager.Instance.currentUserData.finalsChosen
    private string GetFinalFromUserData()
    {
        if (UserDataManager.Instance == null ||
            UserDataManager.Instance.currentUserData == null)
            return string.Empty;

        var arr = UserDataManager.Instance.currentUserData.finalsChosen;
        if (arr == null || arr.Length == 0) return string.Empty;

        // Devuelve el primer valor no vacío
        for (int i = 0; i < arr.Length; i++)
        {
            var s = arr[i];
            if (!string.IsNullOrEmpty(s)) return s.Trim();
        }
        return string.Empty;
    }

    // Mapea "Final1".."Final5" a final01..final05
    private void AssignCinematicManagerByFinalId()
    {
        if (string.IsNullOrEmpty(finalId))
        {
            cinematicManager = null;
            return;
        }

        string id = finalId.Trim();

        switch (id)
        {
            case "Final1":
            case "final1":
            case "FINAL1":
                cinematicManager = final01; break;

            case "Final2":
            case "final2":
            case "FINAL2":
                cinematicManager = final02; break;

            case "Final3":
            case "final3":
            case "FINAL3":
                cinematicManager = final03; break;

            case "Final4":
            case "final4":
            case "FINAL4":
                cinematicManager = final04; break;

            case "Final5":
            case "final5":
            case "FINAL5":
                cinematicManager = final05; break;

            default:
                Debug.LogWarning($"CinematicTriggerCollider: finalId '{finalId}' no reconocido. Esperaba 'Final1'..'Final5'.");
                cinematicManager = null;
                break;
        }
    }

    // Método para reiniciar SOLO este trigger
    public void ResetTrigger()
    {
        PlayerPrefs.DeleteKey("TriggerUsed_" + uniqueID);

        string usedTriggers = PlayerPrefs.GetString("AllUsedTriggerIDs", "");
        usedTriggers = usedTriggers.Replace(uniqueID + ";", "");
        PlayerPrefs.SetString("AllUsedTriggerIDs", usedTriggers);

        PlayerPrefs.Save();
        this.gameObject.SetActive(true);
    }

    // Método estático para reiniciar todos los triggers
    public static void ResetAllTriggers()
    {
        string usedTriggers = PlayerPrefs.GetString("AllUsedTriggerIDs", "");
        string[] triggerIDs = usedTriggers.Split(';');

        foreach (string id in triggerIDs)
        {
            if (!string.IsNullOrEmpty(id))
            {
                PlayerPrefs.DeleteKey("TriggerUsed_" + id);
            }
        }

        PlayerPrefs.DeleteKey("AllUsedTriggerIDs");
        PlayerPrefs.Save();
    }

    // Método estático para reiniciar un trigger específico por ID (sin instancia del script)
    public static void ResetTriggerByID(string id)
    {
        PlayerPrefs.DeleteKey("TriggerUsed_" + id);

        string usedTriggers = PlayerPrefs.GetString("AllUsedTriggerIDs", "");
        usedTriggers = usedTriggers.Replace(id + ";", "");
        PlayerPrefs.SetString("AllUsedTriggerIDs", usedTriggers);

        PlayerPrefs.Save();
    }
}
