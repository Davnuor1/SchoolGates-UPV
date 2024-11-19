using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SeleccionMultipleController : MonoBehaviour
{
    [SerializeField] private MinijuegoEspejos03Localization localizacion;
    public TextMeshProUGUI preguntaTexto;
    public GameObject panelSeleccionMultiple;
    public List<Button> botonesPlantilla;

    private List<string> respuestasSeleccionadas = new List<string>();
    private int idEspejoActual;
    private MinijuegoManager minijuegoManager;

    private void Start()
    {
        minijuegoManager = FindObjectOfType<MinijuegoManager>();
    }

    public void MostrarSeleccionMultiple(int idEspejo)
    {
        idEspejoActual = idEspejo;
        preguntaTexto.text = localizacion.textoPreguntaInicial;
        panelSeleccionMultiple.SetActive(true);

        // Reiniciar los botones antes de mostrar las nuevas respuestas
        ResetearBotones();

        // Configurar los botones basados en las respuestas del `ScriptableObject`
        List<string> respuestas = localizacion.espejos[minijuegoManager.espejosSegundaFase[idEspejo]].respuestas;
        for (int i = 0; i < botonesPlantilla.Count; i++)
        {
            if (i < respuestas.Count)
            {
                botonesPlantilla[i].gameObject.SetActive(true);
                TMP_Text textoBoton = botonesPlantilla[i].GetComponentInChildren<TMP_Text>();
                textoBoton.text = respuestas[i];

                int indiceLocal = i;
                botonesPlantilla[i].onClick.RemoveAllListeners();
                botonesPlantilla[i].onClick.AddListener(() => SeleccionarRespuesta(respuestas[indiceLocal], botonesPlantilla[indiceLocal].gameObject));
            }
            else
            {
                botonesPlantilla[i].gameObject.SetActive(false);
            }
        }
    }

    private void ResetearBotones()
    {
        respuestasSeleccionadas.Clear(); // Limpiar la lista de respuestas seleccionadas

        foreach (Button boton in botonesPlantilla)
        {
            boton.GetComponent<Image>().color = Color.white; // Restablecer el color a blanco
        }
    }

    public void SeleccionarRespuesta(string respuesta, GameObject boton)
    {
        Image botonImage = boton.GetComponent<Image>();
        if (botonImage == null) return;

        if (respuestasSeleccionadas.Contains(respuesta))
        {
            respuestasSeleccionadas.Remove(respuesta);
            botonImage.color = Color.white; // Cambiar el color del botón a blanco si se deselecciona
        }
        else
        {
            respuestasSeleccionadas.Add(respuesta);
            botonImage.color = Color.green; // Cambiar el color del botón a verde si se selecciona
        }
    }

    public void ConfirmarRespuestas()
    {
        if (respuestasSeleccionadas.Count > 0)
        {
            Debug.Log("Respuestas seleccionadas: " + string.Join(", ", respuestasSeleccionadas));
            panelSeleccionMultiple.SetActive(false);
            FindObjectOfType<MinijuegoManager>().SiguienteEspejo();
        }
        else
        {
            Debug.Log("No se seleccionó ninguna respuesta.");
        }
    }
}
