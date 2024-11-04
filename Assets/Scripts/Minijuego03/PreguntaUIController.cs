using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PreguntaUIController : MonoBehaviour
{
    [SerializeField] private MinijuegoEspejos03Localization localizacion;

    public TextMeshProUGUI tituloEspejo;
    public TextMeshProUGUI preguntaTexto;

    public Button botonSi;
    public Button botonNo;
    public GameObject ruletaUnica;
    public GameObject ruletaIzquierda;
    public GameObject ruletaDerecha;

    private MinijuegoManager minijuegoManager;

    void Start()
    {
        minijuegoManager = FindObjectOfType<MinijuegoManager>();
        MostrarBotonesSiNo(true);
        ruletaUnica.SetActive(false);
        ruletaIzquierda.SetActive(false);
        ruletaDerecha.SetActive(false);
    }

    public void MostrarEspejo(int idEspejo)
    {
        tituloEspejo.text = localizacion.espejos[idEspejo].nombre;
        preguntaTexto.text = localizacion.textoInstruccionesPrimeraFase;

        MostrarBotonesSiNo(true);
        ruletaUnica.SetActive(false);
    }

    public void RecibirPuntuacion(int puntuacion)
    {
        Debug.Log("Puntuación recibida para " + localizacion.espejos[minijuegoManager.indiceEspejoActual].nombre + ": " + puntuacion);

        // Llamar al método de MinijuegoManager para asignar la puntuación
        minijuegoManager.AsignarPuntuacionEspejoActual(puntuacion);

        ruletaUnica.SetActive(false);

        // Pasar al siguiente espejo
        minijuegoManager.SiguienteEspejo();
    }

    public void ResponderSi()
    {
        MostrarBotonesSiNo(false);
        preguntaTexto.text = localizacion.textoValoracion;
        ruletaUnica.SetActive(true);
    }

    public void ResponderNo()
    {
        minijuegoManager.SiguienteEspejo();
    }

    public void MostrarSegundaParte(MinijuegoEspejos03Localization.EspejoData espejo)
    {
        tituloEspejo.text = espejo.nombre;
        preguntaTexto.text = localizacion.textoInstruccionesSegundaFase;

        MostrarBotonesSiNo(false);
        ruletaUnica.SetActive(false);
        ruletaIzquierda.SetActive(true);
        ruletaDerecha.SetActive(true);
    }

    public void RecibirPuntuacionSegundaParte(int puntuacionIzquierda, int puntuacionDerecha)
    {
        if (puntuacionIzquierda != -1)
        {
            ruletaIzquierda.SetActive(false);
        }

        if (puntuacionDerecha != -1)
        {
            ruletaDerecha.SetActive(false);
        }

        if (!ruletaIzquierda.activeSelf && !ruletaDerecha.activeSelf)
        {
            preguntaTexto.text = localizacion.textoCuidadoValores;
            FindObjectOfType<InputTextController>().MostrarPanelDeTexto();
        }
    }

    public void ManejarTextoEnviado()
    {
        preguntaTexto.text = localizacion.textoAccionFinal;
        FindObjectOfType<SeleccionMultipleController>().MostrarSeleccionMultiple(minijuegoManager.indiceEspejoActual);
    }

    private void MostrarBotonesSiNo(bool mostrar)
    {
        botonSi.gameObject.SetActive(mostrar);
        botonNo.gameObject.SetActive(mostrar);
    }
}
