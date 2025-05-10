using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CinematicTriggerCollider : MonoBehaviour
{
    public CinematicManager cinematicManager; // Referencia al manager de la cinemática
    public CinematicData cinematicData; // Referencia al ScriptableObject de la cinemática

   
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            cinematicManager.gameObject.SetActive(true); // Activar cinemática
        }
            
    }
}

