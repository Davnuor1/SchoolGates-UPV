using System.Collections.Generic;
using UnityEngine;

public class MinijuegoManager : MonoBehaviour
{
    public List<Espejo> todosLosEspejos; // Lista completa de espejos para la primera fase
    private List<Espejo> espejosSegundaFase; // Lista filtrada de espejos para la segunda fase

    public int indiceEspejoActual = 0;
    private bool enSegundaFase = false;
    private PlayerMovement playerMovement;
    //public FadeInObject escaleras3;
    public GameObject portalSalida;

    public PreguntaUIController preguntaUIController;

    void Start()
    {
        // Iniciar la primera fase mostrando el primer espejo
        indiceEspejoActual = 0;
        enSegundaFase = false;
        preguntaUIController.MostrarEspejo(todosLosEspejos[indiceEspejoActual]);
    }

    public void SiguienteEspejo()
    {
        if (!enSegundaFase)
        {
            //Debug.Log("Indice espejo:"+indiceEspejoActual);
            indiceEspejoActual++;
            //Debug.Log("Indice espejo postsuma:" + indiceEspejoActual);

            if (indiceEspejoActual < todosLosEspejos.Count)
            {
                preguntaUIController.MostrarEspejo(todosLosEspejos[indiceEspejoActual]);
            }
            else
            {
                // Preparar la segunda fase
                PrepararSegundaFase();
            }
        }
        else
        {
            indiceEspejoActual++;

            if (indiceEspejoActual < espejosSegundaFase.Count)
            {
                preguntaUIController.MostrarSegundaParte(espejosSegundaFase[indiceEspejoActual]);
            }
            else
            {
                // Finalizar el minijuego
                
                TerminarMinijuego();
            }
        }
    }

    void PrepararSegundaFase()
    {
        // Filtrar los espejos que tienen puntuación asignada
        espejosSegundaFase = todosLosEspejos.FindAll(espejo => espejo.puntuacion > 0);

        if (espejosSegundaFase.Count == 0)
        {
            // Si no hay espejos con puntuación, finalizar el minijuego
            TerminarMinijuego();
            return;
        }

        // Reiniciar el índice y cambiar el estado a segunda fase
        indiceEspejoActual = 0;
        enSegundaFase = true;

        // Mostrar el primer espejo de la segunda fase
        preguntaUIController.MostrarSegundaParte(espejosSegundaFase[indiceEspejoActual]);
    }

    void TerminarMinijuego()
    {
        // Implementar la lógica para finalizar el minijuego
        Debug.Log("Minijuego terminado.");
        //escaleras3.FadeIn();
        this.gameObject.SetActive(false);
        playerMovement = GameManager.instance.player.GetComponent<PlayerMovement>();
        playerMovement.enabled = true;
        portalSalida.SetActive(true);
        // Puedes cargar una nueva escena, mostrar una pantalla de resumen, etc.
    }
}
