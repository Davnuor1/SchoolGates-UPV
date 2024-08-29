using UnityEngine;

public class MagicCrystal : MonoBehaviour
{
    
    public CrystalSpriteChanger crystalSpriteChanger;
    private bool areRedCrystalsActive = false; // Estado inicial: rojos desactivados, azules activados


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && WithinInteractionRange())
        {
            ToggleCrystals();
        }
    }

    private bool WithinInteractionRange()
    {
        // Asume que tienes una referencia al jugador, aquí solo necesitas
        // verificar si está lo suficientemente cerca para interactuar.
        return Vector3.Distance(transform.position, GameManager.instance.player.transform.position) < 2f;
    }

    private void ToggleCrystals()
    {
        if (areRedCrystalsActive)
        {
            // Si los cristales rojos están activos, desactívalos y activa los azules
            
            crystalSpriteChanger.AnimarAzulesEmerger();
            crystalSpriteChanger.AnimarRojosSumerger();
            
            
        }
        else
        {
            // Si los cristales rojos están inactivos, actívalos y desactiva los azules
            crystalSpriteChanger.AnimarRojosEmerger();
            crystalSpriteChanger.AnimarAzulesSumerger();
            ;
            
        }
        // Invierte el estado de los cristales rojos
        areRedCrystalsActive = !areRedCrystalsActive;
    }
}
