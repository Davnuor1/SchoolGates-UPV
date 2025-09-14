using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using PixelCrushers.DialogueSystem;

public class UserDataManager : MonoBehaviour
{
    public static UserDataManager Instance { get; private set; }

    public UserData currentUserData;

    private float sessionStartTime;
    private float lastSavedTime;

    private string currentGateId = null;
    private float gateSessionStartRealtime = 0f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void Init(string tan)
    {
        var loaded = LocalJsonSave.LoadUserData(tan);
        if (loaded != null)
        {
            currentUserData = loaded;
            // Si en este flujo NO vas a cargar con SaveSystem.LoadFromSlot(),
            // podrías aplicar aquí el snapshot DS:
            // ApplyDialogueSystemStateIfPresent();
        }
        else
        {
            CreateNewUserData(tan);
        }

        sessionStartTime = Time.time;
        lastSavedTime = Time.time;
    }

    public void SetPassword(string plainPassword)
    {
        if (currentUserData == null) return;
        currentUserData.password = plainPassword ?? "";
    }

    public void CreateNewUserData(string tan)
    {
        currentUserData = new UserData();
        currentUserData.tan = tan;
        currentUserData.password = "";
        currentUserData.totalPlayTime = 0f;
        currentUserData.timesGameOpened = 1;

        currentUserData.gateTimes = new UserData.GateTimeEntry[0];
        currentUserData.completedGates = new string[0];
        currentUserData.finalsChosen = new string[0];
        currentUserData.miniquestsCompletedCache = 0;
        currentUserData.experiencePoints = 0;
        currentUserData.integrityPoints = 0;
        currentUserData.positivePresencePoints = 0;
        currentUserData.dialogueSystemSaveData = "";

        currentUserData.languageCode = "es";

    }

    // Tiempo total sin duplicar
    public void UpdatePlayTime()
    {
        float delta = Time.time - lastSavedTime;
        currentUserData.totalPlayTime += delta;
        lastSavedTime = Time.time;
    }

    // Control de tiempos por gate
    public void BeginGateSession(string gateId)
    {
        if (string.IsNullOrEmpty(gateId)) return;

        // Si estábamos en otro gate, acumula y cierra ese tramo sin marcar completado
        if (!string.IsNullOrEmpty(currentGateId) && currentGateId != gateId)
        {
            AccumulateCurrentGateElapsed(false);
        }

        currentGateId = gateId;
        gateSessionStartRealtime = Time.realtimeSinceStartup;
        Debug.Log("BeginGateSession: " + gateId);
    }

    public void EndGateSession(bool markCompleted)
    {
        AccumulateCurrentGateElapsed(markCompleted);
        currentGateId = null;
        gateSessionStartRealtime = 0f;
    }

    // Llamar antes de guardar para sumar el tiempo jugado desde el último snapshot
    public void SnapshotGateElapsedForSave()
    {
        AccumulateCurrentGateElapsed(false);
        if (!string.IsNullOrEmpty(currentGateId))
        {
            gateSessionStartRealtime = Time.realtimeSinceStartup;
        }
    }

    private void AccumulateCurrentGateElapsed(bool markCompleted)
    {
        if (string.IsNullOrEmpty(currentGateId) || gateSessionStartRealtime <= 0f) return;

        float elapsed = Time.realtimeSinceStartup - gateSessionStartRealtime;
        if (elapsed < 0f) elapsed = 0f;

        AddGateSeconds(currentGateId, elapsed);
        if (markCompleted) AddCompletedGate(currentGateId);

        Debug.Log("Gate " + currentGateId + " + " + elapsed + "s (total: " + GetGateSeconds(currentGateId) + "s)");
    }

    private void AddGateSeconds(string gateId, float seconds)
    {
        if (seconds <= 0f) return;

        var list = new List<UserData.GateTimeEntry>(currentUserData.gateTimes);
        var entry = list.FirstOrDefault(e => e.gateId == gateId);
        if (entry == null)
        {
            entry = new UserData.GateTimeEntry { gateId = gateId, seconds = 0f };
            list.Add(entry);
        }
        entry.seconds += seconds;
        currentUserData.gateTimes = list.ToArray();
    }

    private void AddCompletedGate(string gateId)
    {
        var set = new HashSet<string>(currentUserData.completedGates);
        set.Add(gateId);
        currentUserData.completedGates = set.ToArray();
    }

    public float GetGateSeconds(string gateId)
    {
        var e = currentUserData.gateTimes.FirstOrDefault(x => x.gateId == gateId);
        return e != null ? e.seconds : 0f;
    }

    // Copia opcional desde Dialogue System para Excel/analítica
    private void SnapshotMiniquestsFromDialogueSystem()
    {
        try
        {
            int v = DialogueLua.GetVariable("MiniquestsCompleted").asInt;
            currentUserData.miniquestsCompletedCache = v;
        }
        catch
        {
            // Ignorar si la variable no existe
        }
    }

    // Snapshot DS para guardar en JSON
    public void SnapshotDialogueSystemState()
    {
        currentUserData.dialogueSystemSaveData = PersistentDataManager.GetSaveData();
    }

    // Aplicar snapshot DS desde JSON
    public void ApplyDialogueSystemStateIfPresent()
    {
        if (!string.IsNullOrEmpty(currentUserData.dialogueSystemSaveData))
        {
            PersistentDataManager.ApplySaveData(currentUserData.dialogueSystemSaveData);
        }
    }

    // Punto único de guardado de tus datos + snapshot DS
    public void SaveAndUpdateTime()
    {
        UpdatePlayTime();
        SnapshotGateElapsedForSave();
        SnapshotDialogueSystemState();
        SnapshotMiniquestsFromDialogueSystem();
        LocalJsonSave.SaveUserData(currentUserData);
    }

    // Registrar finales cuando se elijan
    public void RegisterFinalChosen(string finalId)
    {
        var list = new List<string>(currentUserData.finalsChosen);
        list.Add(finalId);
        currentUserData.finalsChosen = list.ToArray();
    }
    public bool IsSkillUnlocked(string skillId)
    {
        if (currentUserData == null || currentUserData.unlockedSkills == null) return false;
        return System.Array.IndexOf(currentUserData.unlockedSkills, skillId) >= 0;
    }

    // Desbloquea y devuelve true si realmente cambia algo
    public bool UnlockSkillId(string skillId, bool saveNow = false)
    {
        if (string.IsNullOrEmpty(skillId) || currentUserData == null) return false;

        if (IsSkillUnlocked(skillId)) return false;

        var list = new List<string>(currentUserData.unlockedSkills ?? new string[0]);
        list.Add(skillId);
        currentUserData.unlockedSkills = list.ToArray();

        if (saveNow)
        {
            // guardado ligero: no hace falta tocar PixelCrushers ahora mismo
            LocalJsonSave.SaveUserData(currentUserData);
        }
        return true;
    }
}
