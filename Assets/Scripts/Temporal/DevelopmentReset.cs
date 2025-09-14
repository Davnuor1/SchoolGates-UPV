using UnityEngine;

public class DevelopmentReset : MonoBehaviour
{
    public GameObject verSoloEnTablet;
    private void Awake()
    {
        Debug.Log("DevelopmentReset Awake called");
        // Llama a la función ResetPortalStates al iniciar el juego
        ActivableObject.ResetPortalStates();
        CinematicTriggerCollider.ResetTriggerByID("intro");
        CinematicTriggerCollider.ResetTriggerByID("final");


    }

    private void Start()
    {
        // Desbloqueas los sets 0 y 1
        //GameManager.instance.bookOfCluesManager.UnlockPageSet(0);
        GameManager.instance.bookOfCluesManager.UnlockPageSet(1);
        //SkillTreeController.Instance.Unlock("ocho");
        //GameManager.instance.skillTreeController.Unlock("ocho");
        //Debug.Log("Desbloqueado casilla 8 skill tree");

    }
  
}
