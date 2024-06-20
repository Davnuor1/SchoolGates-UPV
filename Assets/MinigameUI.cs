using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject UIminijuego;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UIminijuego.SetActive(true);
    }
}
