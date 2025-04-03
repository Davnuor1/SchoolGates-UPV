using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "GateOfEmotionsLocalization", menuName = "Localization/GateOfEmotionsLocalization")]
public class GateOfEmotionsLocalization : ScriptableObject
{
    [Header("Textos Iniciales")]
    public string introductionText;
    public string selectTwoEmotionsText;

    [Header("Nombre de emociones")]
    public string anger;
    public string fear;
    public string joy;
    public string sadness;

    [Header("Cielo - Preguntas")]
    public string question1;
    public string question2;
    public string question3;

    [Header("Opciones para preguntas")]
    public string pleasant;
    public string unpleasant;
    public string highEnergy;
    public string lowEnergy;

    [Header("Feedback Correcto")]
    public string feedbackAnger;
    public string feedbackFear;
    public string feedbackJoy;
    public string feedbackSadness;

    [Header("Feedback Incorrecto")]
    public string feedbackIncorrect;
}
