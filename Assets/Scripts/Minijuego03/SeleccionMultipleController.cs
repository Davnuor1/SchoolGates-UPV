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

    public void MostrarSeleccionMultiple(int idEspejo)
    {
        idEspejoActual = idEspejo;
        preguntaTexto.text = localizacion.textoPreguntaInicial;
        panelSeleccionMultiple.SetActive(true);

        // Configurar los botones basados en las respuestas del `ScriptableObject`
        List<string> respuestas = localizacion.espejos[idEspejoActual].respuestas;
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

    public void SeleccionarRespuesta(string respuesta, GameObject boton)
    {
        Image botonImage = boton.GetComponent<Image>();
        if (botonImage == null) return;

        if (respuestasSeleccionadas.Contains(respuesta))
        {
            respuestasSeleccionadas.Remove(respuesta);
            botonImage.color = Color.white;
        }
        else
        {
            respuestasSeleccionadas.Add(respuesta);
            botonImage.color = Color.green;
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
