using UnityEngine;
using PixelCrushers.DialogueSystem;

public class TranslationTrigger : MonoBehaviour
{
    public string conversationName = "WheelchairHelp"; // Nombre de la conversación asignada en el inspector
    private Collider2D boxCollider;
    private bool isActiveConversation = false; // Variable para rastrear si este trigger activó la conversación

    private void Start()
    {
        boxCollider = GetComponent<Collider2D>(); // Obtener referencia al BoxCollider2D
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActiveConversation)
        {
            isActiveConversation = true; // Marcar que este trigger activó la conversación
            DialogueManager.StartConversation(conversationName);
        }
    }

    private void OnEnable()
    {
        // Suscribirse al evento de finalización de la conversación
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.conversationEnded += OnConversationEnd;
        }
    }

    private void OnDisable()
    {
        // Desuscribirse del evento cuando el objeto se desactive
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.conversationEnded -= OnConversationEnd;
        }
    }

    private void OnConversationEnd(Transform actor)
    {
        // Solo desactivar el collider si esta instancia fue la que inició la conversación
        if (isActiveConversation && boxCollider != null)
        {
            boxCollider.enabled = false; // Desactiva el collider solo de este objeto
            Debug.Log(gameObject.name + ": Collider desactivado para evitar reiniciar la conversación.");
        }
    }
}
