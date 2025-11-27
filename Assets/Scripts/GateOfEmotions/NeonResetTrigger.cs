using UnityEngine;

public class NeonResetTrigger : MonoBehaviour
{
    public NeonEmotionManager neonManager;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && neonManager != null)
        {
            // Sirve con cualquiera de las dos (tenemos ambos métodos):
            neonManager.ResetNeonsToBase();
            // neonManager.RestoreNeonsToBase();
            gameObject.SetActive(false);
        }
    }
}
