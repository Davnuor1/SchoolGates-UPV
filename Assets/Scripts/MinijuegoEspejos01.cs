using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinijuegoEspejos01 : MonoBehaviour
{
    [SerializeField] private MinijuegoEspejos01Localization localizacion;
    [SerializeField] private GameObject minijuegoEntero;
    [SerializeField] private GameObject botonLike;
    [SerializeField] private GameObject botonDislike;
    [SerializeField] private GameObject RuedaRating;
    [SerializeField] private TextMeshProUGUI textoEspejos;
    [SerializeField] private TextMeshProUGUI textoPregunta;
    [SerializeField] private GameObject parte2;
    [SerializeField] private TextMeshProUGUI textoCorrecto;
    [SerializeField] private TextMeshProUGUI textoIncorrecto;
    [SerializeField] private TextMeshProUGUI textoFeedback;
    [SerializeField] private GameObject botonSiguiente;

    private List<(string nombre, int id)> espejosLike = new List<(string nombre, int id)>();
    private int idEspejo = 0;
    private PlayerMovement playerMovement;
    [SerializeField] private FadeInObject escaleras;

    private void Start()
    {
        textoPregunta.text = localizacion.textoPreguntaInicial;
        textoEspejos.text = localizacion.espejos[idEspejo].nombre;
    }

    public void darBotonLike()
    {
        botonLike.SetActive(false);
        botonDislike.SetActive(false);
        RuedaRating.SetActive(true);
        textoPregunta.text = localizacion.textoPreguntaRating;
    }

    public void darBotonDislike()
    {
        idEspejo++;
        if (idEspejo >= localizacion.espejos.Count)
        {
            idEspejo = 0;
            parte2Minijuego();
        }
        else
        {
            textoEspejos.text = localizacion.espejos[idEspejo].nombre;
        }
    }

    public void darRating()
    {
        espejosLike.Add((localizacion.espejos[idEspejo].nombre, idEspejo));
        idEspejo++;

        if (idEspejo >= localizacion.espejos.Count)
        {
            idEspejo = 0;
            parte2Minijuego();
        }
        else
        {
            botonLike.SetActive(true);
            botonDislike.SetActive(true);
            RuedaRating.SetActive(false);
            textoEspejos.text = localizacion.espejos[idEspejo].nombre;
            textoPregunta.text = localizacion.textoPreguntaInicial;
        }
    }

    private void parte2Minijuego()
    {
        botonLike.SetActive(false);
        botonDislike.SetActive(false);
        RuedaRating.SetActive(false);
        parte2.SetActive(true);
        textoPregunta.text = localizacion.textoPreguntaParte2;

        var (nombre, id) = espejosLike[idEspejo];
        textoEspejos.text = localizacion.textoPreEspejoParte2 + nombre + localizacion.textoPostEspejoParte2;
        textoCorrecto.text = localizacion.espejos[id].respuestaCorrecta;
        textoIncorrecto.text = localizacion.espejos[id].respuestaIncorrecta;
    }

    public void pulsarCorrecto()
    {
        var (nombre, _) = espejosLike[idEspejo];
        textoFeedback.text = localizacion.textoFeedbackCorrecto + nombre + localizacion.textoFeedbackCorrectoDetras;
        botonSiguiente.SetActive(true);
    }

    public void pulsarIncorrecto()
    {
        var (nombre, _) = espejosLike[idEspejo];
        textoFeedback.text = localizacion.textoFeedbackIncorrectoParte1 + nombre + localizacion.textoFeedbackIncorrectoParte2;
        botonSiguiente.SetActive(true);
    }

    public void pulsarSiguiente()
    {
        idEspejo++;
        if (idEspejo >= espejosLike.Count)
        {
            minijuegoEntero.SetActive(false);
            playerMovement = GameManager.instance.player.GetComponent<PlayerMovement>();
            playerMovement.enabled = true;
            escaleras.FadeIn();
        }
        else
        {
            botonSiguiente.SetActive(false);
            textoFeedback.text = "";

            var (nombre, id) = espejosLike[idEspejo];
            textoEspejos.text = localizacion.textoPreEspejoParte2 + nombre + localizacion.textoPostEspejoParte2;
            textoCorrecto.text = localizacion.espejos[id].respuestaCorrecta;
            textoIncorrecto.text = localizacion.espejos[id].respuestaIncorrecta;
        }
    }
}
