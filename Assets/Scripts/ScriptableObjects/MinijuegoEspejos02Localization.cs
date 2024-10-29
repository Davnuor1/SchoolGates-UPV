using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinijuegoEspejos02Localization", menuName = "Localization/MinijuegoEspejos02Localization")]
public class MinijuegoEspejos02Localization : ScriptableObject
{
    public string textoPreguntaInicial;
    public string textoPreguntaRating;
    //public string textoPreguntaParte2;
    public string textoFeedbackLike;
    public string textoFeedbackDislikeFinal;
    //public string textoFeedbackDislikeIntermedio;
    public string textoDoYouAgree;

    [System.Serializable]
    public struct EspejoTextos
    {
        public string nombre;
        public string feedback01;
        public string feedback02;
    }

    public List<EspejoTextos> espejos;
}
