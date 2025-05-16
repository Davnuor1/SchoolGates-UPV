using UnityEngine;
using PixelCrushers.DialogueSystem;

public class FrutaManager : MonoBehaviour
{
    [SerializeField] private float interactionRadius = 2.0f; // Ajustable desde el inspector para definir el radio de interacción

    private void Start()
    {
        // Registra las funciones globalmente en Lua solo una vez
        Lua.RegisterFunction("EntregarFruta", this, this.GetType().GetMethod("EntregarFruta"));
        Lua.RegisterFunction("HayFrutaSiguiendo", this, this.GetType().GetMethod("HayFrutaSiguiendo"));
        Lua.RegisterFunction("DeactivateEmoticonAfterDelay", this, this.GetType().GetMethod("DeactivateEmoticonAfterDelay"));
    }

    // Método para entregar la fruta al NPC más cercano al jugador
    public void EntregarFruta()
    {
        GameObject player = GameObject.FindWithTag("Player");
        if (player == null)
        {
            Debug.LogWarning("No se encontró el objeto del jugador.");
            return;
        }

        // Encuentra el NPC más cercano al jugador dentro del radio de interacción
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(player.transform.position, interactionRadius);
        NPCLaberinto nearestNPC = null;
        float nearestDistance = Mathf.Infinity;

        foreach (Collider2D hitCollider in hitColliders)
        {
            NPCLaberinto npc = hitCollider.GetComponent<NPCLaberinto>();
            if (npc != null)
            {
                float distance = Vector3.Distance(player.transform.position, npc.transform.position);
                if (distance < nearestDistance)
                {
                    nearestNPC = npc;
                    nearestDistance = distance;
                }
            }
        }

        // Llama a EntregarFruta en el NPC más cercano, si existe
        if (nearestNPC != null)
        {
            // Inicia la conversación con el NPC para mostrar el diálogo primero
            DialogueManager.StartConversation("NPCFruta", nearestNPC.transform);

            // Ahora intentamos desactivar una fruta siguiendo al jugador
            bool frutaDesactivada = nearestNPC.EntregarFruta();

            // Solo incrementa el número de frutas desactivadas si realmente se desactivó una
            if (frutaDesactivada)
            {
                GatePeopleController.Instance.IncrementarFrutasDesactivadas();
            }
        }
        else
        {
            Debug.Log("No hay ningún NPC dentro del radio de interacción.");
        }
    }

    public bool HayFrutaSiguiendo()
    {
        // Encuentra todas las frutas en la escena
        FrutaLaberintoController[] frutas = FindObjectsOfType<FrutaLaberintoController>();

        // Busca la fruta que está siguiendo al jugador
        foreach (FrutaLaberintoController fruta in frutas)
        {
            if (fruta.isFollowing)
            {
                return true; // Retorna verdadero si hay al menos una fruta siguiendo al jugador
            }
        }
        return false; // Retorna falso si no se encontró ninguna fruta siguiendo al jugador
    }

    public void DeactivateEmoticonAfterDelay()
    {
        // Esta función se llama desde Lua pero no necesita lógica adicional aquí
        // La funcionalidad principal de desactivación está en NPCLaberinto.
        Debug.Log("Función DeactivateEmoticonAfterDelay registrada para Lua pero no se ejecuta directamente desde aquí.");
    }

    private void Update()
    {
        // Detecta si el jugador presiona la tecla E para interactuar
        if (VirtualInput.GetKeyDownE())
        {
            EntregarFruta();
        }
    }
}
