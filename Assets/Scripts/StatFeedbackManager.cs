using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class StatFeedbackManager : MonoBehaviour
{
    public GameObject statIconTemplate; // Plantilla del icono
    public Transform feedbackPanel; // Panel donde se mostrarán los feedbacks

    public Sprite energyIcon;
    public Sprite karmaIcon;
    public Sprite spiritualityIcon;
    public Sprite experienceIcon;
    public Sprite arrowUp;
    public Sprite arrowDown;

    private void Start()
    {
        // Asegúrate de que la plantilla y el panel estén configurados
        if (statIconTemplate == null || feedbackPanel == null)
        {
            Debug.LogError("StatFeedbackManager: Faltan referencias al icono de plantilla o al panel.");
        }
    }

    public void ShowFeedback(string statName, double amount)
    {
        if (statIconTemplate == null)
        {
            Debug.LogError("StatFeedbackManager: El icono de plantilla no está configurado.");
            return;
        }

        // Instanciar un nuevo icono basado en la plantilla
        GameObject newFeedback = Instantiate(statIconTemplate, feedbackPanel);
        newFeedback.SetActive(true);

        // Configurar el icono y la flecha
        Image iconImage = newFeedback.GetComponentInChildren<Image>();
        Image arrowImage = newFeedback.transform.Find("ArrowIndicator")?.GetComponent<Image>();

        if (iconImage == null || arrowImage == null)
        {
            Debug.Log("StatFeedbackManager: No se encontraron los componentes de imagen.");
            return;
        }

        // Asignar el icono correspondiente según la estadística
        switch (statName)
        {
            case "Energy":
                iconImage.sprite = energyIcon;
                Debug.Log("Ponemos el icono energia");
                break;
            case "Karma":
                iconImage.sprite = karmaIcon;
                break;
            case "Spirituality":
                iconImage.sprite = spiritualityIcon;
                break;
            case "Experience":
                iconImage.sprite = experienceIcon;
                break;
            default:
                Debug.LogWarning("StatFeedbackManager: Estadística desconocida: " + statName);
                break;
        }

        // Asignar la flecha (arriba si el valor es positivo, abajo si es negativo)
        arrowImage.sprite = amount > 0 ? arrowUp : arrowDown;

        // Iniciar una corrutina para eliminar el feedback después de un tiempo
        StartCoroutine(RemoveFeedbackAfterTime(newFeedback, 2.0f)); // 2 segundos
    }

    private IEnumerator RemoveFeedbackAfterTime(GameObject feedback, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(feedback);
    }
}
