using UnityEngine;

public class TriggerMinijuego : MonoBehaviour
{
   
    private PlayerMovement playerMovement;
    private Animator playerAnimator;
    public MinijuegoEspejosExteriorManager minijuegoEspejosExteriorManager;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        //FindObjectOfType<MinijuegoEspejosExteriorManager>().StartMinijuego();
        minijuegoEspejosExteriorManager.StartMinijuego();
        
        //this.gameObject.SetActive(false);
        gameObject.SetActive(false);
    }
}
