using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PreguntaUIController : MonoBehaviour
{
    public TextMeshProUGUI tituloEspejo;
    public TextMeshProUGUI preguntaTexto;

    public Button botonSi;
    public Button botonNo;
    public GameObject ruletaUnica; // Ruleta para la primera fase
    public GameObject ruletaIzquierda; // Ruleta izquierda para la segunda fase
    public GameObject ruletaDerecha; // Ruleta derecha para la segunda fase

    private Espejo espejoActual;
    private MinijuegoManager minijuegoManager;

    void Start()
    {
        minijuegoManager = FindObjectOfType<MinijuegoManager>();
        MostrarBotonesSiNo(true);
        ruletaUnica.SetActive(false);
        ruletaIzquierda.SetActive(false);
        ruletaDerecha.SetActive(false);
    }

    public void MostrarEspejo(Espejo espejo)
    {
        espejoActual = espejo;
        tituloEspejo.text = espejo.nombreEspejo;
        preguntaTexto.text = "Think on a nice or significant life… What do you really need? Select the mirror if you think it is important or break it if you don’t";

        MostrarBotonesSiNo(true);
        ruletaUnica.SetActive(false);
    }

    public void RecibirPuntuacion(int puntuacion)
    {
        Debug.Log("Puntuación recibida para " + espejoActual.nombreEspejo + ": " + puntuacion);
        espejoActual.puntuacion = puntuacion;
        ruletaUnica.SetActive(false);

        // Llamamos a SiguienteEspejo solo una vez aquí
        minijuegoManager.SiguienteEspejo();
    }

    public void ResponderSi()
    {
        MostrarBotonesSiNo(false);
        preguntaTexto.text = "Value the importance of this things to you ";
        ruletaUnica.SetActive(true);
    }

    public void ResponderNo()
    {
        // Llamamos a SiguienteEspejo solo cuando el usuario elige "No"
        minijuegoManager.SiguienteEspejo();
    }

    public void MostrarSegundaParte(Espejo espejo)
    {
        espejoActual = espejo;
        tituloEspejo.text = espejo.nombreEspejo;
        preguntaTexto.text = "So, you say these is important to you, but how much are you caring about this actually? How much attention and time are you really putting into these?";

        MostrarBotonesSiNo(false);
        ruletaUnica.SetActive(false);
        ruletaIzquierda.SetActive(true);
        ruletaDerecha.SetActive(true);
    }

    public void RecibirPuntuacionSegundaParte(int puntuacionIzquierda, int puntuacionDerecha)
    {
        if (puntuacionIzquierda != -1)
        {
            espejoActual.puntuacionIzquierda = puntuacionIzquierda;
            ruletaIzquierda.SetActive(false); // Ocultar la ruleta izquierda después de asignar la puntuación
        }

        if (puntuacionDerecha != -1)
        {
            espejoActual.puntuacionDerecha = puntuacionDerecha;
            ruletaDerecha.SetActive(false); // Ocultar la ruleta derecha después de asignar la puntuación
        }

        // Solo mostrar el panel de texto si ambas ruletas han sido utilizadas
        if (!ruletaIzquierda.activeSelf && !ruletaDerecha.activeSelf)
        {
            // Mostrar el panel de texto
            preguntaTexto.text = "What can you do now to take more or better care of your values?";
            FindObjectOfType<InputTextController>().MostrarPanelDeTexto();
        }
    }

    public void ManejarTextoEnviado()
    {
        // Después de que el usuario envía el texto, mostrar la pregunta de selección múltiple
        FindObjectOfType<SeleccionMultipleController>().MostrarSeleccionMultiple(espejoActual);
    }




    private void MostrarBotonesSiNo(bool mostrar)
    {
        botonSi.gameObject.SetActive(mostrar);
        botonNo.gameObject.SetActive(mostrar);
    }
}
