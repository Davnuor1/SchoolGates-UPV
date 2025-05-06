using UnityEngine;

public class NeonResetTrigger : MonoBehaviour
{
    public NeonEmotionManager neonManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && neonManager != null)
        {
            neonManager.RestoreNeonsToBase();
            gameObject.SetActive(false); // Opcional: desactivar el trigger después de usarlo
        }
    }
}
