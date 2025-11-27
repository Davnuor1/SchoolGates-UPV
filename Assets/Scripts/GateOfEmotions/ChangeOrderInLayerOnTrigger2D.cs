using UnityEngine;

public class ChangeOrderInLayerOnTrigger2D : MonoBehaviour
{
    [Header("Objetos cuyo Order in Layer quieres cambiar")]
    public SpriteRenderer[] objectsToChange;

    [Header("Nuevo Order in Layer al entrar en el trigger")]
    public int newOrderInLayer = 13;

    [Header("Opcional")]
    [Tooltip("Si está activo, al salir del trigger se restauran los valores originales.")]
    public bool revertOnExit = true;

    // Guardamos los valores originales para poder restaurarlos
    private int[] originalOrders;

    private void Start()
    {
        if (objectsToChange == null || objectsToChange.Length == 0)
            return;

        originalOrders = new int[objectsToChange.Length];

        for (int i = 0; i < objectsToChange.Length; i++)
        {
            if (objectsToChange[i] != null)
            {
                originalOrders[i] = objectsToChange[i].sortingOrder;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Aquí puedes filtrar por tag si quieres, por ejemplo:
        // if (!other.CompareTag("Player")) return;

        if (objectsToChange == null || objectsToChange.Length == 0)
            return;

        for (int i = 0; i < objectsToChange.Length; i++)
        {
            if (objectsToChange[i] != null)
            {
                objectsToChange[i].sortingOrder = newOrderInLayer;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!revertOnExit)
            return;

        // if (!other.CompareTag("Player")) return;

        if (objectsToChange == null || objectsToChange.Length == 0 || originalOrders == null)
            return;

        for (int i = 0; i < objectsToChange.Length; i++)
        {
            if (objectsToChange[i] != null)
            {
                objectsToChange[i].sortingOrder = originalOrders[i];
            }
        }
    }
}
