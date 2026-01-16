using UnityEngine;

[CreateAssetMenu(fileName = "GateOfEmotionsLocalization", menuName = "Localization/GateOfEmotionsLocalization")]
public class GateOfEmotionsLocalization : ScriptableObject
{
    [Header("Parte 1 - Introducción")]
    public string introductionText;
    public string selectTwoEmotionsText;

    [Header("Emociones")]
    public string anger;
    public string fear;
    public string joy;
    public string sadness;

    [Header("Parte 1 - Preguntas en el cielo")]
    public string question1; // "How pleasant or unpleasant do you feel?"
    public string question2; // "How high or low is your energy?"
    public string question3; // "What would you call this feeling?"

    public string pleasant;
    public string unpleasant;
    public string highEnergy;
    public string lowEnergy;

    [Header("Parte 1 - Feedback")]
    public string feedbackIncorrect;
    public string feedbackCorrecto01;
    public string feedbackCorrecto02;
    public string feedbackAnger;
    public string feedbackFear;
    public string feedbackJoy;
    public string feedbackSadness;


    [Header("Parte 2 - Pregunta inicial")]
    public string questionPart2; // "Which emotion is he/she feeling?"
    public string feedbackIncorrectPart2; // "That’s not correct, try again..."

    [Header("Parte 2 - Respuestas")]
    public string respuestaJugador1; // "That's it, they're feeling [emotion]..."
    public string respuestaNPC1; // "You're right, I've been feeling..."

    [Header("Parte 2 - Opciones de respuesta del jugador")]
    public string[] botonesRespuestaJugador2 = new string[3]; // Ej: "You should talk about it", "You’re not alone", etc.

    [Header("Parte 2 - Feedback final (según emoción del NPC)")]
    public string feedbackAnger_Part2;
    public string feedbackFear_Part2;
    public string feedbackJoy_Part2;
    public string feedbackSadness_Part2;
}

