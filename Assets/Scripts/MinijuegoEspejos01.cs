using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinijuegoEspejos01 : MonoBehaviour
{
    [SerializeField] public GameObject botonLike;
    [SerializeField] public GameObject botonDislike;
    [SerializeField] public GameObject RuedaRating;
    [SerializeField] TextMeshProUGUI textoEspejos;
    List<string> espejos;
    List<string> espejosLike;
    public int idEspejo=0;
    [SerializeField] public GameObject parte2;
    //[SerializeField] public GameObject botonCorrecto;
    [SerializeField] TextMeshProUGUI textoCorrecto;
    //[SerializeField] public GameObject botonIncorrecto;
    [SerializeField] TextMeshProUGUI textoIncorrecto;
    //[SerializeField] public GameObject panelFeedback;
    [SerializeField] TextMeshProUGUI textoFeedback;
    Dictionary<string, string> respuestasCorrectas = new Dictionary<string, string>()
    {
        {"Fearful/ anxious","I feel afraid of new things" },
        {"Optimistic","In difficult times I tend to hope for the best." },
        {"Impulsive","I don't usually think about the consequences of what I do or say." },
        {"Open to experience","I like trying new things." },
        {"Approval seeking","I usually do what my friends want me to do." },
        {"Independent","I like spending time with people, but I don't mind doing things on my own" },
        {"Underachiever","I have no clear goals in my life" },
        {"Determined","I am motivated by challenging tasks" },
        {"Insecure","I feel incompetent when faced with obstacles and difficulties." },
        {"Resourceful","I feel that my life has a clear meaning and direction" },
        {"Empathic","I try to treat people fairly" },
        {"Hostile","I tend to take revenge on those who have hurt me" },
        {"Faithful","I believe that we are all equal" },
        {"Materialistic","I only believe what I see" }
    };
    Dictionary<string, string> respuestasIncorrectas = new Dictionary<string, string>(){
        {"Fearful/ anxious","I get nervous when meeting new people" },
        {"Optimistic","Nothing bad can happen to me" },
        {"Impulsive","Sometimes I regret something I have said or done" },
        {"Open to experience","I like to do things my way" },
        {"Approval seeking","I don't feel good when someone gets mad at me" },
        {"Independent","If I am upset, I prefer to be alone." },
        {"Underachiever","Sometimes I doubt about myself and the best way to accomplish the task" },
        {"Determined","I gave up some things in my life" },
        {"Insecure","I feel insecure when I do things wrong" },
        {"Resourceful","I don’t always feel like I have a choice" },
        {"Empathic","sometimes I don't understand the people around me" },
        {"Hostile","I can get very angry sometimes" },
        {"Faithful","Sometimes I doubt that a better world is possible" },
        {"Materialistic","I like receiving presents." }
    };
    void Start()
    {
        espejos = new List<string>();
        espejosLike = new List<string>();
        nombresEspejos();
        textoEspejos.text = espejos[idEspejo];
    }

    
    public void darBotonLike()
    {
        botonLike.SetActive(false);
        botonDislike.SetActive(false);
        RuedaRating.SetActive(true);
    }

    public void darBotonDislike()
    {
        idEspejo += 1;
        //romper espejo
        if (idEspejo > (espejos.Count-1))
        {
            idEspejo = 0;
            parte2Minijuego();
        }
        else
        {
            textoEspejos.text = espejos[idEspejo];
        }
    }
    public void nombresEspejos()
    {
        espejos.Add("Fearful/ anxious");
        espejos.Add("Optimistic");
        espejos.Add("Impulsive");
        espejos.Add("Open to experience");
        espejos.Add("Approval seeking");
        espejos.Add("Independent");
        espejos.Add("Underachiever");
        espejos.Add("Determined");
        espejos.Add("Insecure");
        espejos.Add("Resourceful");
        espejos.Add("Empathic");
        espejos.Add("Hostile");
        espejos.Add("Faithful");
        espejos.Add("Materialistic");

    }
    public void darRating()
    {
        //guardar puntuacion
        espejosLike.Add(espejos[idEspejo]);
        idEspejo += 1;
        Debug.Log("hola rating");
        if (idEspejo > (espejos.Count-1))
        {
            Debug.Log("hola rating en el if");
            idEspejo = 0;
            parte2Minijuego();
        }
        else
        {
            Debug.Log("hola rating en el else");
            botonLike.SetActive(true);
            botonDislike.SetActive(true);
            RuedaRating.SetActive(false);
            textoEspejos.text = espejos[idEspejo];
        }
        
        
    }
    public void parte2Minijuego()
    {
        botonLike.SetActive(false);
        botonDislike.SetActive(false);
        RuedaRating.SetActive(false);
        parte2.SetActive(true);
        textoEspejos.text = "Which is the real proof of beeing " + espejosLike[idEspejo];
        textoCorrecto.text= respuestasCorrectas[espejosLike[idEspejo]];
        textoIncorrecto.text= respuestasIncorrectas[espejosLike[idEspejo]];
    }
    public void pulsarCorrecto()
    {
        textoFeedback.text = "Aqui feedback correcto";
        idEspejo += 1;
        textoEspejos.text = "Which is the real proof of beeing " + espejosLike[idEspejo];
        textoCorrecto.text = respuestasCorrectas[espejosLike[idEspejo]];
        textoIncorrecto.text = respuestasIncorrectas[espejosLike[idEspejo]];
    }
    public void pulsarIncorrecto()
    {
        textoFeedback.text = "Aqui feedback incorrecto";
        idEspejo += 1;
        textoEspejos.text = "Which is the real proof of beeing " + espejosLike[idEspejo];
        textoCorrecto.text = respuestasCorrectas[espejosLike[idEspejo]];
        textoIncorrecto.text = respuestasIncorrectas[espejosLike[idEspejo]];
    }

}
