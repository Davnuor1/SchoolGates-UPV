using System.Collections.Generic;
using UnityEngine;

public class MinijuegoManager : MonoBehaviour
{
    [SerializeField] private MinijuegoEspejos03Localization localizacion;
    public List<Espejo> todosLosEspejos;
    private List<Espejo> espejosSegundaFase;
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

        // Inicializar la lista de puntuaciones de acuerdo al número de espejos
        puntuacionesEspejos = new List<int>(new int[todosLosEspejos.Count]);

        // Mostrar el primer espejo
        preguntaUIController.MostrarEspejo(indiceEspejoActual);
    }

    public void SiguienteEspejo()
    {
        if (!enSegundaFase)
        {
            indiceEspejoActual++;

            if (indiceEspejoActual < todosLosEspejos.Count)
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
                preguntaUIController.MostrarSegundaParte(indiceEspejoActual);
            }
            else
            {
                TerminarMinijuego();
            }
        }
    }

    void PrepararSegundaFase()
    {
        // Filtrar los espejos que tienen puntuación asignada
        espejosSegundaFase = new List<Espejo>();

        for (int i = 0; i < todosLosEspejos.Count; i++)
        {
            if (puntuacionesEspejos[i] > 0) // Agregar a segunda fase si el espejo tiene puntuación
            {
                espejosSegundaFase.Add(todosLosEspejos[i]);
            }
        }

        if (espejosSegundaFase.Count == 0)
        {
            TerminarMinijuego();
            return;
        }

        indiceEspejoActual = 0;
        enSegundaFase = true;

        preguntaUIController.MostrarSegundaParte(indiceEspejoActual);
    }

    public void AsignarPuntuacionEspejoActual(int puntuacion)
    {
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
