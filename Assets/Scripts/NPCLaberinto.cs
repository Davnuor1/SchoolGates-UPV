using System.Collections;
using UnityEngine;
using PixelCrushers.DialogueSystem; // Asegúrate de incluir este namespace

public class NPCLaberinto : MonoBehaviour
{
    [SerializeField] private  GameObject emoticono; // Referencia al objeto emoticono
    
    void Start()
    {
        // Registrar el método EntregarFruta para que sea accesible desde Lua
        Lua.RegisterFunction("EntregarFruta", this, this.GetType().GetMethod("EntregarFruta"));

        // Registrar el método DeactivateEmoticonAfterDelay si también necesitas llamarlo directamente (poco común)
        Lua.RegisterFunction("DeactivateEmoticonAfterDelay", this, this.GetType().GetMethod("DeactivateEmoticonAfterDelay"));

        Lua.RegisterFunction("HayFrutaSiguiendo", this, this.GetType().GetMethod("HayFrutaSiguiendo"));
    }

    private void OnMouseDown()
    {
        // Iniciar una conversación
        DialogueManager.StartConversation("NPCFruta", transform);
    }

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


    // Método público que puede ser llamado desde el sistema de diálogo
    public void EntregarFruta()
    {
        // Encuentra todas las frutas en la escena
        FrutaLaberintoController[] frutas = FindObjectsOfType<FrutaLaberintoController>();
        foreach (FrutaLaberintoController fruta in frutas)
        {
            if (fruta.isFollowing)
            {
                fruta.DeactivateFruit();
                GatePeopleController.Instance.IncrementarFrutasDesactivadas(); // Llamada al GameController

                emoticono.transform.position = transform.position + new Vector3(0, 1, 0);
                emoticono.SetActive(true);

                StartCoroutine(DeactivateEmoticonAfterDelay());
                break;
            }
        }
    }

    public IEnumerator DeactivateEmoticonAfterDelay()
    {
        // Esperar 2 segundos
        yield return new WaitForSeconds(3f);

        // Desactivar el emoticono
        emoticono.SetActive(false);

    }
}
