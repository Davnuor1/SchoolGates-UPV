using UnityEngine;

public class CardMinigameTrigger : MonoBehaviour
{
    [SerializeField] private CardMinigameManager manager;
    [SerializeField] private bool oneShot = true;
    private bool used = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (used && oneShot) return;
        if (!other.CompareTag("Player")) return;

        if (manager != null)
        {
            Debug.Log("CardMinigame iniciado");
            manager.OpenMinigame();
            used = true;
        }
    }
}
