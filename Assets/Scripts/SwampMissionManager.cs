using UnityEngine;
using PixelCrushers.DialogueSystem;

public class SwampMissionManager : MonoBehaviour
{
    public static SwampMissionManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    // Llamado cuando se completa un pasillo
    public void CompleteTrial()
    {
        int completedTrials = DialogueLua.GetVariable("SwampTrialsCompleted").AsInt;
        completedTrials++;
        DialogueLua.SetVariable("SwampTrialsCompleted", completedTrials);
        Debug.Log("Pruebas completadas: " + completedTrials);

        // Comprobar si la misión se ha completado
        if (completedTrials >= 5)
        {
            Debug.Log("¡Misión completada!");
            QuestLog.SetQuestState("Complete the Swamp Trials", QuestState.Success);
        }
    }
}
