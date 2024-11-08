using System.Collections;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class NPCLaberinto : MonoBehaviour
{
    [SerializeField] private GameObject emoticono; // Referencia al objeto emoticono específico de cada NPC

    public bool HayFrutaSiguiendo()
    {
        // Encuentra todas las frutas en la escena
        FrutaLaberintoController[] frutas = FindObjectsOfType<FrutaLaberintoController>();

        // Busca la fruta que está siguiendo al jugador
        foreach (FrutaLaberintoController fruta in frutas)
        {
            if (fruta.isFollowing)
            {
                return true; // Retorna verdadero si hay al menos una fruta siguiendo al jugador
            }
        }
        return false; // Retorna falso si no se encontró ninguna fruta siguiendo al jugador
    }

    // Método público que puede ser llamado desde FrutaManager para entregar la fruta a este NPC específico
    public bool EntregarFruta()
    {
        // Encuentra todas las frutas en la escena
        FrutaLaberintoController[] frutas = FindObjectsOfType<FrutaLaberintoController>();
        foreach (FrutaLaberintoController fruta in frutas)
        {
            if (fruta.isFollowing)
            {
                fruta.DeactivateFruit();
                emoticono.transform.position = transform.position + new Vector3(0, 1, 0);
                emoticono.SetActive(true);

                StartCoroutine(DeactivateEmoticonAfterDelay());
                return true; // Indica que se desactivó una fruta
            }
        }
        return false; // No se encontró ninguna fruta siguiendo al jugador para desactivar
    }

    public IEnumerator DeactivateEmoticonAfterDelay()
    {
        // Esperar 3 segundos
        yield return new WaitForSeconds(3f);

        // Desactivar el emoticono del NPC específico
        emoticono.SetActive(false);
    }
}
