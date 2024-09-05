using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrdenarAsignaturas : MonoBehaviour
{
    public GameObject[] collectionIcons; // Los iconos de las colecciones completadas
    public GameObject[] slots; // Los "huecos" numerados del 1 al 5

    public GameObject minigamePanel; // Panel general del minijuego (asignado desde el Inspector)
    public GameObject portal; // GameObject "portal" que se activará (asignado desde el Inspector)

    private List<GameObject> availableCollections = new List<GameObject>(); // Lista de colecciones disponibles para ordenar
    private int nextSlotIndex = 0; // El índice del siguiente hueco disponible
    private PlayerMovement playerMovement;

    void Start()
    {
        //player = GameManager.instance.player;
        // Inicializar la lista de colecciones disponibles con los iconos visibles
        foreach (var icon in collectionIcons)
        {
            if (icon != null)
            {
                availableCollections.Add(icon);
            }
        }
    }

    public void OnCollectionIconClicked(GameObject clickedIcon)
    {
        if (nextSlotIndex < slots.Length && availableCollections.Contains(clickedIcon))
        {
            // Mover el icono al siguiente hueco disponible
            clickedIcon.transform.SetParent(slots[nextSlotIndex].transform, false);
            clickedIcon.transform.position = slots[nextSlotIndex].transform.position;

            // Actualizar el siguiente hueco disponible
            nextSlotIndex++;

            // Remover la colección de las disponibles
            availableCollections.Remove(clickedIcon);

            // Verificar si todos los slots están llenos para cerrar el panel y activar el portal
            if (nextSlotIndex >= slots.Length)
            {
                CloseRewardPanel();
            }
        }
    }

    public void ResetSlots()
    {
        // Reiniciar los slots y devolver los iconos a su posición original
        foreach (var icon in collectionIcons)
        {
            if (icon != null)
            {
                icon.transform.SetParent(this.transform, false);
                availableCollections.Add(icon);
            }
        }

        // Reiniciar el índice del siguiente hueco disponible
        nextSlotIndex = 0;

        // Vaciar todos los slots
        foreach (var slot in slots)
        {
            if (slot.transform.childCount > 0)
            {
                var child = slot.transform.GetChild(0);
                child.SetParent(this.transform, false);
            }
        }
    }

    private void CloseRewardPanel()
    {
        // Cerrar el panel de recompensa, el panel del minijuego y activar el portal
        this.gameObject.SetActive(false);

        if (minigamePanel != null)
        {
            minigamePanel.SetActive(false);
        }

        if (portal != null)
        {
            portal.SetActive(true);
        }
        
        playerMovement=GameManager.instance.player.GetComponent<PlayerMovement>();
        playerMovement.enabled = true;
    }
}
