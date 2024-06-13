using System.Collections;
using UnityEngine;

public class NPCLaberinto : MonoBehaviour
{
    
    public GameObject emoticono; // Referencia al objeto emoticono

    private void OnMouseDown()
    {
        // Encuentra todas las frutas en la escena
        FrutaLaberintoController[] frutas = FindObjectsOfType<FrutaLaberintoController>();

        // Busca la fruta que está siguiendo al jugador
        foreach (FrutaLaberintoController fruta in frutas)
        {
            if (fruta.isFollowing)
            {
                // Desactivar la fruta
                fruta.DeactivateFruit();

                // Activar el emoticono en la posición de la caja
                emoticono.transform.position = transform.position + new Vector3(0, 1, 0); // Ajusta la posición según sea necesario
                emoticono.SetActive(true);
                // Iniciar la corrutina para desactivar el emoticono después de 2 segundos
                StartCoroutine(DeactivateEmoticonAfterDelay());

                break;
            }
        }
    }

    private IEnumerator DeactivateEmoticonAfterDelay()
    {
        // Esperar 2 segundos
        yield return new WaitForSeconds(2f);

        // Desactivar el emoticono
        emoticono.SetActive(false);
    }
}
