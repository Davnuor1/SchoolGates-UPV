using UnityEngine;
using PixelCrushers.DialogueSystem;

public class TriggerConversationsEmotions : MonoBehaviour
{
    public string conversationName = "DefaultConversation";
    public GateOfEmotionsUIManager uiManager;

    private Collider2D triggerCollider;
    private bool isActiveConversation = false;

    private void Start()
    {
        triggerCollider = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !isActiveConversation)
        {
            isActiveConversation = true;
            DialogueManager.StartConversation(conversationName);
        }
    }

    private void OnEnable()
    {
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.conversationEnded += OnConversationEnd;
        }
    }

    private void OnDisable()
    {
        if (DialogueManager.instance != null)
        {
            DialogueManager.instance.conversationEnded -= OnConversationEnd;
        }
    }

    private void OnConversationEnd(Transform actor)
    {
        if (isActiveConversation && triggerCollider != null)
        {
            triggerCollider.enabled = false;

            if (uiManager != null)
            {
                uiManager.StartFadeToHeavenAfterConversation();
            }
        }
    }
}
