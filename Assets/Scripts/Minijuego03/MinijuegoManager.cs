using System.Collections.Generic;
using UnityEngine;

public class MinijuegoManager : MonoBehaviour
{
    [SerializeField] private MinijuegoEspejos03Localization localizacion;
    private List<int> puntuacionesEspejos; // Lista para almacenar las puntuaciones de cada espejo

    public int indiceEspejoActual = 0;
    private bool enSegundaFase = false;
    private PlayerMovement playerMovement;
    public GameObject portalSalida;

    public PreguntaUIController preguntaUIController;

    void Start()
    {
        indiceEspejoActual = 0;
        enSegundaFase = false;

        // Inicializar la lista de puntuaciones de acuerdo al número de espejos en la localización
        puntuacionesEspejos = new List<int>(new int[localizacion.espejos.Count]);

        // Mostrar el primer espejo
        preguntaUIController.MostrarEspejo(indiceEspejoActual);
    }

    public void SiguienteEspejo()
    {
        if (!enSegundaFase)
        {
            indiceEspejoActual++;

            if (indiceEspejoActual < localizacion.espejos.Count)
            {
                preguntaUIController.MostrarEspejo(indiceEspejoActual);
            }
            else
            {
                PrepararSegundaFase();
            }
        }
        else
        {
            indiceEspejoActual++;

            if (indiceEspejoActual < espejosSegundaFase.Count)
            {
                Debug.Log($"Mostrando espejo en la segunda fase: {localizacion.espejos[espejosSegundaFase[indiceEspejoActual]].nombre}");
                preguntaUIController.MostrarSegundaParte(localizacion.espejos[espejosSegundaFase[indiceEspejoActual]]);
            }
            else
            {
                TerminarMinijuego();
            }
        }
    }

    private List<int> espejosSegundaFase;

    void PrepararSegundaFase()
    {
        espejosSegundaFase = new List<int>();

        for (int i = 0; i < puntuacionesEspejos.Count; i++)
        {
            Debug.Log($"Evaluando espejo {i} con puntuación: {puntuacionesEspejos[i]}");
            if (puntuacionesEspejos[i] > 0)
            {
                espejosSegundaFase.Add(i);
                Debug.Log($"Espejo {localizacion.espejos[i].nombre} añadido para la segunda fase, recuento actual: {espejosSegundaFase.Count}");
            }
        }

        if (espejosSegundaFase.Count == 0)
        {
            Debug.Log("No hay espejos con puntuación para la segunda fase. Terminando el minijuego.");
            TerminarMinijuego();
            return;
        }

        indiceEspejoActual = 0;
        enSegundaFase = true;

        Debug.Log($"Comenzando segunda fase con {espejosSegundaFase.Count} espejos.");
        preguntaUIController.MostrarSegundaParte(localizacion.espejos[espejosSegundaFase[indiceEspejoActual]]);
    }

    public void AsignarPuntuacionEspejoActual(int puntuacion)
    {
        Debug.Log($"Asignando puntuación {puntuacion} al índice {indiceEspejoActual}");
        puntuacionesEspejos[indiceEspejoActual] = puntuacion;
    }

    void TerminarMinijuego()
    {
        Debug.Log("Minijuego terminado.");
        this.gameObject.SetActive(false);
        playerMovement = GameManager.instance.player.GetComponent<PlayerMovement>();
        playerMovement.enabled = true;
        portalSalida.SetActive(true);
    }
}
