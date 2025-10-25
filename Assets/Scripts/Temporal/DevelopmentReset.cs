using UnityEngine;

public class DevelopmentReset : MonoBehaviour
{
    public GameObject verSoloEnTablet;
    private void Awake()
    {
        Debug.Log("DevelopmentReset Awake called");

        // Reseteos que ya tenías
        ActivableObject.ResetPortalStates();
        CinematicTriggerCollider.ResetTriggerByID("intro");
        CinematicTriggerCollider.ResetTriggerByID("final");

        // Reset de las cinemáticas por ID (coincidir con 'cinematicId' en cada CinematicManager)
        // Si en el inspector dejaste includeLanguageInId = true:
        CinematicManager.ResetCinematicById("intro", includeLanguage: true, languageIfNeeded: "es");
        CinematicManager.ResetCinematicById("final", includeLanguage: true, languageIfNeeded: "es");

        // Si en ese CinematicManager usaste clave automática (sin cinematicId), puedes resetear así:
        // CinematicManager.ResetCinematicAuto(Application.productName, "NombreDeLaEscena", "es", "NombreDelAssetCinematicData");
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
