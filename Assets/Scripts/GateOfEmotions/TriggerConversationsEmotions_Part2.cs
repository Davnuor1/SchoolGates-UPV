using UnityEngine;
using PixelCrushers.DialogueSystem;

public class TriggerConversationsEmotions_Part2 : MonoBehaviour
{
    public string conversationName = "DefaultConversation";
    public GateOfEmotionsUIManager_Part2 part2Manager;

    private Collider2D triggerCollider;
    private bool isActiveConversation = false;
    private bool thisStartedConversation = false;

    private void Start()
    {
        triggerCollider = GetComponent<Collider2D>();
        if (part2Manager == null)
        {
            Debug.LogWarning($"{name}: No se ha asignado el GateOfEmotionsUIManager_Part2.");
        }
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
        }

        if (part2Manager != null)
        {
            part2Manager.StartFadeToHeavenPart2();
        }

        thisStartedConversation = false;
    }
}
