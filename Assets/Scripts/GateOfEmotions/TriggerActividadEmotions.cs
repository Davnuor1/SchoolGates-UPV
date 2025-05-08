using UnityEngine;
using PixelCrushers.DialogueSystem;

public class TriggerActividadEmotions : MonoBehaviour
{
    public string conversationName = "DefaultConversation";

    private Collider2D triggerCollider;
    private bool isActiveConversation = false;
    private bool thisStartedConversation = false;

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
            thisStartedConversation = true;
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
        if (!thisStartedConversation) return;

        if (isActiveConversation && triggerCollider != null)
        {
            triggerCollider.enabled = false;
            gameObject.SetActive(false);
        }

        thisStartedConversation = false;
    }
}
