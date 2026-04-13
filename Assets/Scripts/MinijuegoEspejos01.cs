using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinijuegoEspejos01 : MonoBehaviour
{
    //[SerializeField] private MinijuegoEspejos01Localization localizacion;
    
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

    [SerializeField] private GeometricShapesGenerator geometricShapesGenerator; // Generador de figuras geométricas
    [Header("Traducciones")]
    [SerializeField] private MinijuegoEspejos01Localization localizacionES;
    [SerializeField] private MinijuegoEspejos01Localization localizacionIT;
    [SerializeField] private MinijuegoEspejos01Localization localizacionDE;
    [SerializeField] private MinijuegoEspejos01Localization localizacionEN;
    [SerializeField] private MinijuegoEspejos01Localization localizacionFI;
    [SerializeField] private MinijuegoEspejos01Localization localizacionFR;
    private MinijuegoEspejos01Localization localizacion;
    private string codeLanguage;
    private List<(string nombre, int id, int rating)> espejosLike = new List<(string nombre, int id, int rating)>();
    private int idEspejo = 0;
    private PlayerMovement playerMovement;
    [SerializeField] private FadeInObject escaleras;

    private void Start()
    {
        defineLanguage();
        textoPregunta.text = localizacion.textoPreguntaInicial;
        textoEspejos.text = localizacion.espejos[idEspejo].nombre;
    }
    public void defineLanguage()
    {
        codeLanguage = LocalizationManager.Instance.CurrentLanguage;
        if(codeLanguage=="es") { localizacion = localizacionES; }
        else if (codeLanguage == "it") { localizacion = localizacionIT; }
        else if (codeLanguage == "de") { localizacion = localizacionDE; }
        else if (codeLanguage == "en") { localizacion = localizacionEN; }
        else if (codeLanguage == "fi") { localizacion = localizacionFI; }
        else if (codeLanguage == "fr") { localizacion = localizacionFR; }
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

    public void darRating(int rating)
    {
        espejosLike.Add((localizacion.espejos[idEspejo].nombre, idEspejo, rating));
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

        var (nombre, id, _) = espejosLike[idEspejo];
        textoEspejos.text = localizacion.textoPreEspejoParte2 +" "+ nombre+" " + localizacion.textoPostEspejoParte2;
        textoCorrecto.text = localizacion.espejos[id].respuestaCorrecta;
        textoIncorrecto.text = localizacion.espejos[id].respuestaIncorrecta;
    }

    public void pulsarCorrecto()
    {
        var (nombre, _, _) = espejosLike[idEspejo];
        textoFeedback.text = localizacion.textoFeedbackCorrecto + " " + nombre + " " + localizacion.textoFeedbackCorrectoDetras;
        botonSiguiente.SetActive(true);
    }

    public void pulsarIncorrecto()
    {
        var (nombre, _, _) = espejosLike[idEspejo];
        textoFeedback.text = localizacion.textoFeedbackIncorrectoParte1 + " " + nombre + " " + localizacion.textoFeedbackIncorrectoParte2;
        botonSiguiente.SetActive(true);
    }

    public void pulsarSiguiente()
    {
        idEspejo++;
        if (idEspejo >= espejosLike.Count)
        {
            EndMinijuego();
        }
        else
        {
            botonSiguiente.SetActive(false);
            textoFeedback.text = "";

            var (nombre, id, _) = espejosLike[idEspejo];
            textoEspejos.text = localizacion.textoPreEspejoParte2 + " " + nombre + " " + localizacion.textoPostEspejoParte2;
            textoCorrecto.text = localizacion.espejos[id].respuestaCorrecta;
            textoIncorrecto.text = localizacion.espejos[id].respuestaIncorrecta;
        }
    }

    private void EndMinijuego()
    {
        minijuegoEntero.SetActive(false);
        playerMovement = GameManager.instance.player.GetComponent<PlayerMovement>();
        playerMovement.enabled = true;
        if (DeviceDetector.isTouchDevice && GameManager.instance.tabletUI != null)
        {
            GameManager.instance.tabletUI.SetActive(true);
        }
        escaleras.FadeIn();

        // Preparar los datos para el generador de figuras geométricas
        List<(string nombre, int rating, Sprite figura)> likedMirrorsData = new List<(string, int, Sprite)>();
        foreach (var (nombre, id, rating) in espejosLike)
        {
            var espejoData = localizacion.espejos[id];
            likedMirrorsData.Add((nombre, rating, espejoData.figuraGeometrica));
        }

        geometricShapesGenerator.GenerateShapes(likedMirrorsData);
    }
}
