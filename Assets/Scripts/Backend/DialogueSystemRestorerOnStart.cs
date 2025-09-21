using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class DialogueSystemRestorerOnStart : MonoBehaviour
{
    private IEnumerator Start()
    {
        // Espera 1 frame para asegurarte de que Dialogue Manager y Savers ya se han inicializado
        yield return null;

        var udm = UserDataManager.Instance;
        if (udm != null && udm.currentUserData != null)
        {
            string data = udm.currentUserData.dialogueSystemSaveData;
            if (!string.IsNullOrEmpty(data))
            {
                // Restaura TODO el estado de Dialogue System desde el string
                PersistentDataManager.ApplySaveData(data);
                Debug.Log("Dialogue System restaurado desde snapshot (longitud " + data.Length + ")");
            }
        }

        Destroy(this); // ya no hace falta
    }
}
