using System.Collections;
using UnityEngine;

public class MagicCrystal : MonoBehaviour
{
    public CrystalSpriteChanger crystalSpriteChanger;
    private bool areRedCrystalsActive = false; // Estado inicial: rojos desactivados, azules activados
    public float delay =0f; // Retardo 

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && WithinInteractionRange())
        {
            StartCoroutine(ToggleCrystalsWithDelay());
        }
    }

    private bool WithinInteractionRange()
    {
        return Vector3.Distance(transform.position, GameManager.instance.player.transform.position) < 2f;
    }

    private IEnumerator ToggleCrystalsWithDelay()
    {
        yield return new WaitForSeconds(delay); // Esperar por el retardo especificado antes de ejecutar la lógica de toggle

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
        }

        // Invierte el estado de los cristales rojos
        areRedCrystalsActive = !areRedCrystalsActive;
    }
}
