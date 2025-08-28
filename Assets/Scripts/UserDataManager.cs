using UnityEngine;

public class UserDataManager : MonoBehaviour
{
    public UserData currentUserData;
    private float sessionStartTime;
    private float lastSavedTime;

    public void Init(string tan)
    {
        var loaded = LocalJsonSave.LoadUserData(tan);

        if (loaded != null)
        {
            currentUserData = loaded;
            // OJO: el estado del Dialogue System lo cargaremos con PixelCrushers.SaveSystem (apartado B),
            // así evitamos aplicar dos veces. Si prefieres, podrías aplicar aquí:
            // ApplyDialogueSystemStateIfPresent();
            Debug.Log("UserData cargado para TAN: " + tan);
        }
        else
        {
            CreateNewUserData(tan);
            Debug.Log("No había UserData. Se crea nuevo para TAN: " + tan);
        }

        sessionStartTime = Time.time;
        lastSavedTime = Time.time;
    }


    public void CreateNewUserData(string tan)
    {
        currentUserData = new UserData();
        currentUserData.tan = tan;
        currentUserData.totalPlayTime = 0f;
        currentUserData.currentWorldIndex = 0;
        currentUserData.challengesCompleted = new int[4];
        currentUserData.totalChallengesCompleted = 0;
        currentUserData.timesGameOpened = 1;
    }

    public void UpdatePlayTime()
    {
        float timeSinceLastSave = Time.time - lastSavedTime;
        currentUserData.totalPlayTime += timeSinceLastSave;
        lastSavedTime = Time.time;

        Debug.Log(" Tiempo añadido: " + timeSinceLastSave + "s | Tiempo total: " + currentUserData.totalPlayTime + "s");
    }

    // Úsalo antes de guardar
    public void SaveAndUpdateTime()
    {
        UpdatePlayTime();
        SnapshotDialogueSystemState();

        // Aquí, en Fase 3, añadiremos la escritura a disco:
         //SaveSystem.SaveUserData(currentUserData);
    }
    private void OnApplicationQuit()
    {
        //  Solo se ejecuta fuera de WebGL
        // Útil para pruebas locales o builds de escritorio
        SaveAndUpdateTime();
    }
    // Captura el estado actual del Dialogue System dentro de currentUserData
    public void SnapshotDialogueSystemState()
    {
        if (currentUserData == null) return;
        currentUserData.dialogueSystemSaveData = PixelCrushers.DialogueSystem.PersistentDataManager.GetSaveData();
        Debug.Log("Dialogue System snapshot guardado en UserData (longitud): " + (currentUserData.dialogueSystemSaveData != null ? currentUserData.dialogueSystemSaveData.Length : 0));
    }

    // Aplica el estado del Dialogue System si existe en currentUserData
    public void ApplyDialogueSystemStateIfPresent()
    {
        if (currentUserData == null) return;
        if (!string.IsNullOrEmpty(currentUserData.dialogueSystemSaveData))
        {
            PixelCrushers.DialogueSystem.PersistentDataManager.ApplySaveData(currentUserData.dialogueSystemSaveData);
            Debug.Log("Dialogue System snapshot aplicado desde UserData.");
        }
    }

}
