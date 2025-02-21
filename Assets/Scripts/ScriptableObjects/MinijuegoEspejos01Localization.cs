using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinijuegoEspejos01Localization", menuName = "Localization/MinijuegoEspejos01Localization")]
public class MinijuegoEspejos01Localization : ScriptableObject
{
    public string textoPreguntaInicial;
    public string textoPreguntaRating;
    public string textoPreguntaParte2;
    public string textoPreEspejoParte2;
    public string textoPostEspejoParte2;

    public string textoFeedbackCorrecto;
    public string textoFeedbackCorrectoDetras;
    public string textoFeedbackIncorrectoParte1;
    public string textoFeedbackIncorrectoParte2;

    [System.Serializable]
    public struct EspejoTextos
    {
        public string nombre;
        public string respuestaCorrecta;
        public string respuestaIncorrecta;
        public Sprite figuraGeometrica; // Imagen de la figura geométrica
    }

    public List<EspejoTextos> espejos;
}
