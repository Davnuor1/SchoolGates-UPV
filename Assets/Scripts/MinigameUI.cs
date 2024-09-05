using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameUI : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject UIminijuego;
    private PlayerMovement playerMovement;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        UIminijuego.SetActive(true);
        playerMovement=GameManager.instance.player.GetComponent<PlayerMovement>();
        playerMovement.enabled = false;
        this.gameObject.SetActive(false);
    }
}
