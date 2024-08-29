using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeleccionMultipleController : MonoBehaviour
{
    public TextMeshProUGUI preguntaTexto;
    public GameObject panelSeleccionMultiple; // El panel que contiene las respuestas
    public List<Button> botonesPlantilla; // Lista de los 6 botones plantilla preposicionados

    private List<string> respuestasSeleccionadas = new List<string>();
    private Espejo espejoActual;

    public void MostrarSeleccionMultiple(Espejo espejo)
    {
        espejoActual = espejo;
        preguntaTexto.text = "Which kind of people do you know who care about this as much as you do?";
        panelSeleccionMultiple.SetActive(true);

        // Desactivar todos los botones antes de comenzar y resetear el color a blanco
        foreach (Button boton in botonesPlantilla)
        {
            boton.gameObject.SetActive(false);  // Asegúrate de que los botones no visibles estén desactivados
            boton.GetComponent<Image>().color = Color.white; // Restablecer el color a blanco
        }

        // Activar y configurar solo los botones necesarios
        for (int i = 0; i < espejo.respuestasSeleccionMultiple.Count; i++)
        {
            botonesPlantilla[i].gameObject.SetActive(true);
            TMP_Text textoBoton = botonesPlantilla[i].GetComponentInChildren<TMP_Text>();
            textoBoton.text = espejo.respuestasSeleccionMultiple[i];

            // Captura correctamente el índice en una variable local
            int indiceLocal = i;
            botonesPlantilla[i].onClick.RemoveAllListeners();  // Remover cualquier listener previo
            botonesPlantilla[i].onClick.AddListener(() => SeleccionarRespuesta(espejo.respuestasSeleccionMultiple[indiceLocal], botonesPlantilla[indiceLocal].gameObject));
        }
    }


    public void SeleccionarRespuesta(string respuesta, GameObject boton)
    {
        Image botonImage = boton.GetComponent<Image>();

        if (botonImage == null)
        {
            Debug.LogError("El botón no tiene un componente Image asignado.");
            return;
        }

        Debug.Log($"Botón pulsado: {boton.name}, Respuesta: {respuesta}");

        if (respuestasSeleccionadas.Contains(respuesta))
        {
            respuestasSeleccionadas.Remove(respuesta);
            botonImage.color = Color.white; // Cambia el color del botón a blanco si se deselecciona
            Debug.Log($"El botón {boton.name} ha sido deseleccionado (blanco).");
        }
        else
        {
            respuestasSeleccionadas.Add(respuesta);
            botonImage.color = Color.green; // Cambia el color del botón a verde si se selecciona
            Debug.Log($"El botón {boton.name} ha sido seleccionado (verde).");
        }
    }

    public void ConfirmarRespuestas()
    {
        if (respuestasSeleccionadas.Count > 0)
        {
            Debug.Log("Respuestas seleccionadas: " + string.Join(", ", respuestasSeleccionadas));
            panelSeleccionMultiple.SetActive(false);

            // Proceder al siguiente espejo
            FindObjectOfType<MinijuegoManager>().SiguienteEspejo();
        }
        else
        {
            Debug.Log("No se seleccionó ninguna respuesta.");
        }
    }
}
