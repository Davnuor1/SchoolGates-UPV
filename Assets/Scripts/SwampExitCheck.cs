using UnityEngine;
using PixelCrushers.DialogueSystem;

public class SwampExitCheck : MonoBehaviour
{
    public string questName = "Save the people trapped on the Swamp!";
    public int requiredPeopleRescued = 5;
    public GameObject barrera;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Barca"))
        {
            Debug.Log("primer IF");
            int peopleRescued = DialogueLua.GetVariable("PeopleTrappedSwamp.numSaved").asInt;

            if (peopleRescued >= requiredPeopleRescued)
            {

                Debug.Log("Puedes pasar al siguiente área.");
                // Lógica para permitir el paso, como abrir una puerta o cargar una nueva escena
                barrera.SetActive(false);
            }
            else
            {
                DialogueManager.ShowAlert("There are still people trapped on swamp, go back and find them!");
            }
        }
    }
}
