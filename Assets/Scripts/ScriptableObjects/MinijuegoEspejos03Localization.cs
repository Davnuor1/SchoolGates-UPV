using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinijuegoEspejos03Localization", menuName = "Localization/MinijuegoEspejos03Localization")]
public class MinijuegoEspejos03Localization : ScriptableObject
{
    public string textoPreguntaInicial;
    public string textoInstruccionesPrimeraFase;
    public string textoInstruccionesSegundaFase;
    public string textoValoracion;
    public string textoCuidadoValores;
    public string textoAccionFinal;

    [System.Serializable]
    public struct EspejoData
    {
        public string nombre;
        public List<string> respuestas;
    }

    public List<EspejoData> espejos;
}
