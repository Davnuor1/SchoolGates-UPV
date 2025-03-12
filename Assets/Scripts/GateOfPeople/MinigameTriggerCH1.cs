using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameTriggerCH1 : MonoBehaviour
{
    private bool minigameStarted = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!minigameStarted && other.CompareTag("Player"))
        {
            FindObjectOfType<WiseManPracticesGameManager>().ShowNextVignette();
            minigameStarted = true;

            // Opcional: Desactivar el trigger después de activarlo
            gameObject.SetActive(false);
        }
    }
}
