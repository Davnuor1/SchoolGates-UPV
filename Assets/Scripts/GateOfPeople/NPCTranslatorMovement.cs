using System.Collections;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class NPCTranslatorMovement : MonoBehaviour
{
    public Transform[] waypoints;
    public float fadeDuration = 1f; // Duración del fade
    public Animator changeSceneAnimator; // Asigna el Animator del canvas de fade
    public string npcID; // Identificador único para cada NPC
    public MonoBehaviour scriptToEnable; // Script que se activará al final del pasillo

    private int currentWaypointIndex = 0;

    private void Start()
    {
        transform.position = waypoints[currentWaypointIndex].position;

        // Registrar la función MoveToNext en LUA con un identificador único para cada NPC
        Lua.RegisterFunction("MoveToNext_" + npcID, this, typeof(NPCTranslatorMovement).GetMethod("MoveToNext"));
    }

    private void OnDestroy()
    {
        // Eliminar la función LUA para evitar referencias inválidas
        Lua.UnregisterFunction("MoveToNext_" + npcID);
    }

    public void MoveToNext()
    {
        Debug.Log(npcID + " - MoveToNext() ha sido llamada.");

        if (currentWaypointIndex < waypoints.Length - 1)
        {
            currentWaypointIndex++;
            StartCoroutine(TeleportToNextWaypoint());
        }
        else
        {
            Debug.Log(npcID + " ha llegado al final del pasillo.");
            SwampMissionManager.instance.CompleteTrial();

            // Activar el script cuando llegue al último waypoint
            if (scriptToEnable != null)
            {
                scriptToEnable.enabled = true;
                Debug.Log(npcID + " - Se ha activado el script: " + scriptToEnable.GetType().Name);
            }
        }
    }

    private IEnumerator TeleportToNextWaypoint()
    {
        Debug.Log(npcID + " - Iniciando teletransporte...");

        // Activar el FadeOut
        if (changeSceneAnimator != null)
        {
            changeSceneAnimator.SetTrigger("FadeOut");
        }

        // Esperar a que se complete el fade
        yield return new WaitForSeconds(fadeDuration);

        // Teletransportar al NPC al siguiente waypoint
        transform.position = waypoints[currentWaypointIndex].position;

        Debug.Log(npcID + " - Teletransportado a: " + waypoints[currentWaypointIndex].position);

        // Activar el FadeIn
        if (changeSceneAnimator != null)
        {
            changeSceneAnimator.SetTrigger("FadeIn");
        }

        yield return new WaitForSeconds(0.5f);
        Debug.Log(npcID + " - Teletransporte completo.");
    }
}
