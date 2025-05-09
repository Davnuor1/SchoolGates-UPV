using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerStartGarden : MonoBehaviour
{
    public EmotionsGardenUI gardenUI;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            gardenUI.StartGardenIntro();
            gameObject.SetActive(false);
        }
    }
}

