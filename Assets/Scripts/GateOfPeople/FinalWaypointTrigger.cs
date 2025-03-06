using UnityEngine;

public class FinalWaypointTrigger : MonoBehaviour
{
    public NPCTranslatorMovement npcScript; // Referencia al NPC a monitorear

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == npcScript.gameObject)
        {
            Debug.Log(npcScript.npcID + " - Ha llegado al último waypoint.");

            // Activar el script del NPC
            if (npcScript.scriptToEnable != null)
            {
                npcScript.scriptToEnable.enabled = true;
                Debug.Log(npcScript.npcID + " - Se ha activado el script: " + npcScript.scriptToEnable.GetType().Name);
            }

            // Marcar el pasillo como completado
            SwampMissionManager.instance.CompleteTrial();

            // Desactivar el trigger para evitar activaciones repetidas
            gameObject.SetActive(false);
        }
    }
}
