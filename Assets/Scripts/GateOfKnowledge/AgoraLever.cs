using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AgoraLever : MonoBehaviour
{
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            AgoraAltarManager.Instance.TryFusion();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Vector3.Distance(other.transform.position, transform.position) <= 1f)
            playerInRange = true;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            playerInRange = false;
    }
}
