using UnityEngine;

public class EmotionsFrustrationTrigger : MonoBehaviour
{
    public EmotionsFrustrationManager frustrationManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            frustrationManager.StartFrustrationMinigame();
            gameObject.SetActive(false); // Desactivar el trigger tras activarlo
        }
    }
}
