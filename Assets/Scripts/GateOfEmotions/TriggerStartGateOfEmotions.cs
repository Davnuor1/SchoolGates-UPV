using UnityEngine;

public class TriggerStartGateOfEmotions : MonoBehaviour
{
    [Header("UI Manager del Minijuego")]
    public GateOfEmotionsUIManager uiManager;

    [Header("Canvas del minijuego (opcional para activar/desactivar)")]
    public GameObject canvasGateOfEmotions;

    private bool hasTriggered = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (hasTriggered) return;

        if (collision.CompareTag("Player"))
        {
            hasTriggered = true;

            if (canvasGateOfEmotions != null)
                canvasGateOfEmotions.SetActive(true);

            if (uiManager != null)
                uiManager.StartIntro(); // Método que mostrará Panel_Intro (lo hacemos ahora)

            // Si usas movimiento del jugador, puedes desactivarlo aquí si quieres
            // collision.GetComponent<PlayerMovement>().enabled = false;
        }
    }
}
