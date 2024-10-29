using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinijuegoEspejos02 : MonoBehaviour
{
    [SerializeField] private MinijuegoEspejos02Localization localizacion;
    [SerializeField] private GameObject minijuegoEntero;
    [SerializeField] private GameObject botonLike;
    [SerializeField] private GameObject botonDislike;
    [SerializeField] private GameObject botonLike2;
    [SerializeField] private GameObject botonDislike2;
    [SerializeField] private GameObject RuedaRating;
    [SerializeField] private GameObject panelFeedback;
    [SerializeField] private GameObject panelTitulo;
    [SerializeField] private GameObject panelTitulo2;
    [SerializeField] private TextMeshProUGUI textoEspejos;
    [SerializeField] private TextMeshProUGUI textoPreguntas;
    [SerializeField] private TextMeshProUGUI textoPreguntas2;
    [SerializeField] private TextMeshProUGUI textoFeedback;
    [SerializeField] private GameObject botonSiguiente;
    private PlayerMovement playerMovement;
    private List<string> espejosLike = new List<string>();
    private int idEspejo = 0;
    [SerializeField] private FadeInObject escaleras2;

    private void Start()
    {
        textoPreguntas.text = localizacion.textoPreguntaInicial;
        textoEspejos.text = localizacion.espejos[idEspejo].nombre;
    }

    public void darBotonLike()
    {
        botonLike.SetActive(false);
        botonDislike.SetActive(false);
        textoPreguntas.text = localizacion.textoPreguntaRating;
        RuedaRating.SetActive(true);
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

    public void darRating(int puntuacion)
    {
        espejosLike.Add(localizacion.espejos[idEspejo].nombre);
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
            textoPreguntas.text = localizacion.textoPreguntaInicial;
            textoEspejos.text = localizacion.espejos[idEspejo].nombre;
        }
    }

    private void parte2Minijuego()
    {
        botonLike.SetActive(false);
        botonDislike.SetActive(false);
        RuedaRating.SetActive(false);
        panelTitulo.SetActive(false);
        botonLike2.SetActive(true);
        botonDislike2.SetActive(true);
        panelFeedback.SetActive(true);
        panelTitulo2.SetActive(true);
        textoEspejos.text = espejosLike[idEspejo];
        textoPreguntas2.text = localizacion.espejos[idEspejo].feedback01 + localizacion.textoDoYouAgree;
    }

    public void darBotonLike2()
    {
        textoFeedback.text = localizacion.textoFeedbackLike;
        botonSiguiente.SetActive(true);
    }

    public void darBotonDislike2()
    {
        if (textoPreguntas2.text == localizacion.espejos[idEspejo].feedback02 +  localizacion.textoDoYouAgree)
        {
            textoFeedback.text = localizacion.textoFeedbackDislikeFinal;
            botonSiguiente.SetActive(true);
        }
        else
        {
            textoPreguntas2.text = localizacion.espejos[idEspejo].feedback02 + localizacion.textoDoYouAgree;
        }
    }

    public void pulsarSiguiente()
    {
        idEspejo++;
        if (idEspejo >= espejosLike.Count)
        {
            escaleras2.FadeIn();
            playerMovement = GameManager.instance.player.GetComponent<PlayerMovement>();
            playerMovement.enabled = true;
            minijuegoEntero.SetActive(false);
        }
        else
        {
            botonSiguiente.SetActive(false);
            textoFeedback.text = "";
            textoEspejos.text = espejosLike[idEspejo];
            textoPreguntas2.text = localizacion.espejos[idEspejo].feedback01 + localizacion.textoDoYouAgree;
        }
    }
}
