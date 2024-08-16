using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;


public class MinijuegoEspejos02 : MonoBehaviour
{


    [SerializeField] public GameObject minijuegoEntero;
    [SerializeField] public GameObject botonLike;
    [SerializeField] public GameObject botonDislike;
    [SerializeField] public GameObject botonLike2;
    [SerializeField] public GameObject botonDislike2;
    [SerializeField] public GameObject RuedaRating;
    [SerializeField] public GameObject panelFeedback;
    [SerializeField] public GameObject panelTitulo;
    [SerializeField] public GameObject panelTitulo2;
    [SerializeField] TextMeshProUGUI textoEspejos;
    [SerializeField] TextMeshProUGUI textoPreguntas;
    [SerializeField] TextMeshProUGUI textoPreguntas2;
    [SerializeField] TextMeshProUGUI textoFeedback;
    [SerializeField] public GameObject botonSiguiente;
    List<string> espejos;
    
    List<string> espejosLike;
    
    public int idEspejo = 0;

    Dictionary<string, string> feedbacks01 = new Dictionary<string, string>()
    {
        {"I must do well and get the approval of everybody who matters to me or I will be a worthless person.", "Remember that not everyone is always going to like you and that's okay."},
        {"Other people must treat me kindly and fairly or else they are bad.", "Remember that people sometimes act unkind and unfair because, just like you, they have their own struggles and are not always able to behave friendly."},
        {"I must have an easy, enjoyable life or I cannot enjoy living at all.", "Remember that life is unpredictable and is filled with both challenges and enjoyment."},
        {"All the people who matter to me must love me and approve of me or it will be awful.", "Remember that those people who love and care about you, have their own opinions and not need to agree with or approve of everything you do.This does not mean that they don’t love you or love you less"},
        {"I must be a high achiever or I will be worthless.", "Remember that your personal worth comes from who you are, not just from what you accomplish or from what you have"},
        {"Nobody should ever behave badly and if they do I should condemn them.", "Remember that people are not perfect, and they can be influenced by unfortunate events, experiences, and emotions, which can lead to bad behavior."},
        {"I mustn’t be frustrated in getting what I want and if I am it will be terrible.", "Remember that it is not terrible to feel frustrated, it’s simply a signal that something is not going as planned."},
        {"When things are tough and I am under pressure I must be miserable and there is nothing I can do about this.", "Remember that in difficult moments it is normal to feel miserable, but there is always at least something you can do: decide how you take the setback."},
        {"When faced with the possibility of something frightening or dangerous happening to me I must obsess about it and make frantic efforts to avoid it.", "Remember that, while it‘s ok to be concerned about potential dangers or frightening situations, excessive worry and avoidance behaviors can make you feel even more anxious."},
        {"I can avoid my responsibilities and dealing with life’s difficulties and still be fulfilled.", "Remember that, while it's tempting to avoid responsibility and difficulties, doing so can prevent you from realizing your potential and enjoy life at its fullest."},
        {"My past is the most important part of my life and it will keep on dictating how I feel and what I do.", "Remember the past may limit your options, but it does not completely determine you. Instead of dwelling on the past and feeling trapped by it, focus on what you can control in the present."},
        {"Everybody and everything should be better than they are and, if they’re not, it’s awful.", "Remember that expecting everybody and everything to be perfect is an unattainable goal, because life is filled with imperfections, both in people and circumstances."},
        {"I can be as happy as is possible by doing as little as I can and by just enjoying myself.", "Remember that a happy life is not only about seeking pleasure, but also about accepting life's challenges."}

    };
    Dictionary<string, string> feedbacks02 = new Dictionary<string, string>()
    {
        {"I must do well and get the approval of everybody who matters to me or I will be a worthless person.", "Keep in mind that your personal worth should not depend on other people's approval."},
        {"Other people must treat me kindly and fairly or else they are bad.", "You may consider that bad behavior is not necessarily a reflection of malice."},
        {"I must have an easy, enjoyable life or I cannot enjoy living at all.", "Keep in mind that it's the very nature of life's ups and downs that make it a rich and meaningful experience and give you the opportunity to learn & grow."},
        {"All the people who matter to me must love me and approve of me or it will be awful.", "Consider that true connections are built on understanding, empathy, and the ability to work through disagreements."},
        {"I must be a high achiever or I will be worthless.", "Keep in mind that making mistakes and experiencing setbacks are part of the human experience."},
        {"Nobody should ever behave badly and if they do I should condemn them.", "You may consider consider adopting a more empathetic and constructive approach, trying to understand the reasons of their actions, instead of immediately condemning those who behave badly."},
        {"I mustn’t be frustrated in getting what I want and if I am it will be terrible.", "Keep in mind that frustration is a common emotional response to obstacles, delays, and unmet expectations, which can be a valuable source of motivation and growth and are part of everybody’s lives."},
        {"When things are tough and I am under pressure I must be miserable and there is nothing I can do about this.", "You may consider different things you can do to manage stress and improve your emotional state, rather than resigning yourself to misery."},
        {"When faced with the possibility of something frightening or dangerous happening to me I must obsess about it and make frantic efforts to avoid it.", "You may consider adopting a more rational and measured approach to addressing potential risks by evaluating the likelihood and severity of the threat objectively, and taking reasonable precautions to mitigate it."},
        {"I can avoid my responsibilities and dealing with life’s difficulties and still be fulfilled.", "Keep in mind that life comes with challenges, moral obligations, and responsibilities, and attempting to evade them will likely lead to greater stress and unhappiness in the long run."},
        {"My past is the most important part of my life and it will keep on dictating how I feel and what I do.", "You may consider that, despite your past, you have the power to make choices and decisions in the present moment that can influence your future."},
        {"Everybody and everything should be better than they are and, if they’re not, it’s awful.", "Keep in mind that the pursuit of improvement is a natural and ongoing process that should be balanced with an acceptance of reality."},
        {"I can be as happy as is possible by doing as little as I can and by just enjoying myself.", "You may consider that achieving happiness involves setting and pursuing meaningful goals, contributing to others, and embracing personal growth; all these aspects require effort, responsibilities and facing challenges."}

    };
    // Start is called before the first frame update
    void Start()
    {
        espejos = new List<string>();
        
        espejosLike = new List<string>();
        nombresEspejos();
        //textoPreguntas.text = "Is that you? Break the mirror if it is not. Or select if it is";
        textoPreguntas.text = "How much does this statement reflect your own beliefs?";
        textoEspejos.text = espejos[idEspejo];
    }
    public void darBotonLike()
    {
        botonLike.SetActive(false);
        botonDislike.SetActive(false);
        textoPreguntas.text = "Rate from 1-10 how much you identify with this mirror";
        RuedaRating.SetActive(true);
    }

    public void darBotonDislike()
    {
        idEspejo += 1;
        //romper espejo
        if (idEspejo > (espejos.Count - 1))
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
        espejos.Add("I must do well and get the approval of everybody who matters to me or I will be a worthless person.");
        espejos.Add("Other people must treat me kindly and fairly or else they are bad.");
        espejos.Add("I must have an easy, enjoyable life or I cannot enjoy living at all.");
        espejos.Add("All the people who matter to me must love me and approve of me or it will be awful.");
        espejos.Add("I must be a high achiever or I will be worthless.");
        espejos.Add("Nobody should ever behave badly and if they do I should condemn them.");
        espejos.Add("I mustn’t be frustrated in getting what I want and if I am it will be terrible.");
        espejos.Add("When things are tough and I am under pressure I must be miserable and there is nothing I can do about this.");
        espejos.Add("When faced with the possibility of something frightening or dangerous happening to me I must obsess about it and make frantic efforts to avoid it.");
        espejos.Add("I can avoid my responsibilities and dealing with life’s difficulties and still be fulfilled.");
        espejos.Add("My past is the most important part of my life and it will keep on dictating how I feel and what I do.");
        espejos.Add("Everybody and everything should be better than they are and, if they’re not, it’s awful.");
        espejos.Add("I can be as happy as is possible by doing as little as I can and by just enjoying myself.");

    }
    public void feedback01()
    {
        espejos.Add("I must do well and get the approval of everybody who matters to me or I will be a worthless person.");
        espejos.Add("Other people must treat me kindly and fairly or else they are bad.");
        espejos.Add("I must have an easy, enjoyable life or I cannot enjoy living at all.");
        espejos.Add("All the people who matter to me must love me and approve of me or it will be awful.");
        espejos.Add("I must be a high achiever or I will be worthless.");
        espejos.Add("Nobody should ever behave badly and if they do I should condemn them.");
        espejos.Add("I mustn’t be frustrated in getting what I want and if I am it will be terrible.");
        espejos.Add("When things are tough and I am under pressure I must be miserable and there is nothing I can do about this.");
        espejos.Add("When faced with the possibility of something frightening or dangerous happening to me I must obsess about it and make frantic efforts to avoid it.");
        espejos.Add("I can avoid my responsibilities and dealing with life’s difficulties and still be fulfilled.");
        espejos.Add("My past is the most important part of my life and it will keep on dictating how I feel and what I do.");
        espejos.Add("Everybody and everything should be better than they are and, if they’re not, it’s awful.");
        espejos.Add("I can be as happy as is possible by doing as little as I can and by just enjoying myself.");

    }
    public void darRating(int puntuacion)
    {
        //guardar puntuacion
        espejosLike.Add(espejos[idEspejo]);
        idEspejo += 1;

        if (idEspejo > (espejos.Count - 1))
        {
            idEspejo = 0;
            parte2Minijuego();
        }
        else
        {
            botonLike.SetActive(true);
            botonDislike.SetActive(true);
            RuedaRating.SetActive(false);
            textoPreguntas.text = "How much does this statement reflect your own beliefs?";
            textoEspejos.text = espejos[idEspejo];
        }
        
    }
    public void parte2Minijuego()
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
        textoPreguntas2.text = feedbacks01[espejosLike[idEspejo]]+" Do you agree?";
    }
    public void darBotonLike2()
    {
        textoFeedback.text = "Great! Let's keep looking for the real you…";
        //se rompe espejo
        
        botonSiguiente.SetActive(true);
    }
    public void darBotonDislike2()
    {
        if(textoPreguntas2.text == feedbacks02[espejosLike[idEspejo]] + " Do you agree now?")
        {
            textoFeedback.text = "Ok… just keep this in mind and keep looking on for the real you…";
            //espejo se oscurece
            botonSiguiente.SetActive(true);
        }
        else
        {
            textoPreguntas2.text = feedbacks02[espejosLike[idEspejo]] + " Do you agree now?";
        }
        
    }
    public void pulsarSiguiente()
    {
        idEspejo += 1;
        if (idEspejo > (espejosLike.Count - 1))
        {
            minijuegoEntero.SetActive(false);
        }
        else
        {
            botonSiguiente.SetActive(false);
            textoFeedback.text = "";
            textoEspejos.text = espejosLike[idEspejo];
            textoPreguntas2.text = feedbacks01[espejosLike[idEspejo]] + " Do you agree?";

        }
    }
}
