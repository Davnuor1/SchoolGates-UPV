using UnityEngine;
using PixelCrushers.DialogueSystem;

public class NPCProximityTrigger : MonoBehaviour
{
    [Tooltip("Nombre de la conversación que se iniciará al interactuar.")]
    public string conversationName;  // Nombre de la conversación que se iniciará, asignable desde el Inspector

    public float interactionDistance = 2f;  // Distancia mínima para la interacción
    private GameObject player;
    private bool isPlayerNearby = false;
    

    void Start()
    {
        //DialogueManager.SetLanguage("es");
        //Debug.Log("Lenguaje cambiado a español");
        // Intentar encontrar al jugador al inicio
        FindPlayer();
    }

    void Update()
    {
        // Si no se ha encontrado el jugador, intentar encontrarlo
        if (player == null)
        {
            FindPlayer();
            return;  // No seguir ejecutando si aún no hay jugador
        }

        // Comprobar la distancia y la tecla de interacción
        if (Vector2.Distance(transform.position, player.transform.position) <= interactionDistance)
        {
            isPlayerNearby = true;
            if (VirtualInput.GetKeyDownE())
            {
                StartConversation();
            }
        }
        else
        {
            isPlayerNearby = false;
        }
    }

    void FindPlayer()
    {
        // Buscar al jugador por su tag
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("Jugador no encontrado. Asegúrate de que el objeto jugador tiene el tag 'Player'.");
        }
    }

    void StartConversation()
    {
        if (!string.IsNullOrEmpty(conversationName))
        {
            // Iniciar la conversación desde el Dialogue System
            DialogueManager.StartConversation(conversationName);
        }
        else
        {
            Debug.LogWarning("El nombre de la conversación no está asignado.");
        }
    }
    
}
