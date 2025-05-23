using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OrdenarAsignaturas : MonoBehaviour
{
    public GameObject[] collectionIcons; // Los iconos de las colecciones completadas
    public GameObject[] slots; // Los "huecos" numerados del 1 al 5

    public GameObject minigamePanel; // Panel general del minijuego (asignado desde el Inspector)
    public GameObject portal; // GameObject "portal" que se activará (asignado desde el Inspector)
    public GameObject[] GuardiasDesactivar;

    private List<GameObject> availableCollections = new List<GameObject>(); // Lista de colecciones disponibles para ordenar
    private Dictionary<GameObject, Transform> originalParents = new Dictionary<GameObject, Transform>(); // Para guardar el padre original de cada icono
    private int nextSlotIndex = 0; // El índice del siguiente hueco disponible
    private PlayerMovement playerMovement;

    void Start()
    {
        // Inicializar la lista de colecciones disponibles con los iconos visibles y guardar sus padres originales
        foreach (var icon in collectionIcons)
        {
            if (icon != null)
            {
                availableCollections.Add(icon);
                originalParents[icon] = icon.transform.parent; // Guardamos el padre original del icono
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
                icon.transform.SetParent(originalParents[icon], false); // Restaurar el padre original
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
                child.SetParent(originalParents[child.gameObject], false); // Restaurar el padre del icono
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
            //portal.SetActive(true);
            Debug.Log("Aqui activabamos portal");
            RetirarGuardias();
        }

        playerMovement = GameManager.instance.player.GetComponent<PlayerMovement>();
        playerMovement.enabled = true;
        if (DeviceDetector.isTouchDevice && GameManager.instance.tabletUI != null)
        {
            GameManager.instance.tabletUI.SetActive(true);
        }
    }
    private void RetirarGuardias()
    {
        foreach (var guardia in GuardiasDesactivar)
        {
            guardia.gameObject.SetActive(false);
        }
    }
}
