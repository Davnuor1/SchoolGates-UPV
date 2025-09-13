using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicTriggerCollider : MonoBehaviour
{
    [Header("Configuración")]
    public string uniqueID; // ID único para este trigger
    public CinematicManager cinematicManager; // Referencia al manager de la cinemática
    //public CinematicData cinematicData; // Referencia al ScriptableObject
    [Header("Traducciones")]
    [SerializeField] private CinematicData localizacionES;
    [SerializeField] private CinematicData localizacionIT;
    [SerializeField] private CinematicData localizacionDE;
    [SerializeField] private CinematicData localizacionEN;
    [SerializeField] private CinematicData localizacionFI;
    public CinematicData cinematicData;
    private string codeLanguage;

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
        if (collision.CompareTag("Player"))
        {
            Debug.Log("colision trigger cinematica detectada");
            cinematicManager.gameObject.SetActive(true); // Activa la cinemática
            PlayerPrefs.SetInt("TriggerUsed_" + uniqueID, 1); // Marca como usado

            // Guardar el ID en la lista de triggers usados si no está ya
            string usedTriggers = PlayerPrefs.GetString("AllUsedTriggerIDs", "");
            if (!usedTriggers.Contains(uniqueID + ";")) // Añade el separador para evitar falsos positivos
            {
                usedTriggers += uniqueID + ";";
                PlayerPrefs.SetString("AllUsedTriggerIDs", usedTriggers);
            }

            PlayerPrefs.Save();
            this.gameObject.SetActive(false); // Desactiva el trigger
        }
    }

    // Método para reiniciar SOLO este trigger
    public void ResetTrigger()
    {
        PlayerPrefs.DeleteKey("TriggerUsed_" + uniqueID);

        // También elimina este ID de la lista general
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
