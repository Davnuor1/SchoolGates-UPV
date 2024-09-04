using UnityEngine;
using System.Collections;
using System.Collections.Generic; // Asegúrate de incluir esto para usar List<>

public class MagicCrystal : MonoBehaviour
{
    public CrystalSpriteChanger crystalSpriteChanger; // Controlador de animaciones
    public float delay = 0f; // Retardo de 0 segundo

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
        

        // Verificar si los coliders de los cristales rojos están activados (emergidos) o desactivados (sumergidos)
        bool areRedCrystalsEmerged = IsColliderActive(crystalSpriteChanger.crystalsRojos);
        bool areBlueCrystalsEmerged = IsColliderActive(crystalSpriteChanger.crystalsAzules);

        if (areRedCrystalsEmerged)
        {
            // Si los cristales rojos están emergidos, sumergirlos y emerger los cristales azules
            
            crystalSpriteChanger.AnimarRojosSumerger();
            yield return new WaitForSeconds(delay); // Esperar por el retardo especificado antes de ejecutar la lógica de toggle
            crystalSpriteChanger.AnimarAzulesEmerger();
        }
        else if (areBlueCrystalsEmerged)
        {
            // Si los cristales azules están emergidos, sumergirlos y emerger los cristales rojos
            
            crystalSpriteChanger.AnimarAzulesSumerger();
            yield return new WaitForSeconds(delay); // Esperar por el retardo especificado antes de ejecutar la lógica de toggle
            crystalSpriteChanger.AnimarRojosEmerger();
        }
    }

    // Método para verificar si alguno de los cristales en la lista tiene su collider activado (emergido)
    private bool IsColliderActive(List<GameObject> crystals)
    {
        foreach (GameObject crystal in crystals)
        {
            BoxCollider2D collider = crystal.GetComponent<BoxCollider2D>();
            if (collider != null && collider.enabled)
            {
                return true; // Si al menos uno de los coliders está activado, consideramos que el grupo está emergido
            }
        }
        return false; // Si todos los coliders están desactivados, consideramos que están sumergidos
    }
}
