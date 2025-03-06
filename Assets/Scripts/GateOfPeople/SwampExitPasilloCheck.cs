using PixelCrushers.DialogueSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwampExitPasilloCheck : MonoBehaviour
{
    public string questName;
    public int numRequired = 3;
    public GameObject barrera;
    public string nameVariable;
    public GameObject nextNPC;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Debug.Log("primer IF");
            int peopleRescued = DialogueLua.GetVariable(nameVariable).asInt;

            if (peopleRescued >= numRequired)
            {

                Debug.Log("Puedes pasar al siguiente área.");
                // Lógica para permitir el paso, como abrir una puerta o cargar una nueva escena
                barrera.SetActive(false);
                //nextNPC.SetActive(true);
            }
            else
            {
                DialogueManager.ShowAlert("You should help the person behind before advance!");
            }
        }
    }
}
