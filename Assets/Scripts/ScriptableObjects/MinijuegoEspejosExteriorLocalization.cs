using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MinijuegoEspejosExteriorLocalization", menuName = "Localization/MinijuegoEspejosExteriorLocalization")]
public class MinijuegoEspejosExteriorLocalization : ScriptableObject
{
    [Header("Global Texts")]
    public string topMessageInitial = "Rumiator is talking to you, what do you want to do?";
    public string topMessageRespondFormat = "How do you respond to the demon of {0}?"; // {0} se reemplazará por el título del espejo
    public string topMessageSecondRound = "What else can you tell them?";

    public string buttonRepeatText = "Repeat";
    public string buttonRespondText = "Respond";

    [Header("Mirror Data")]
    public List<MirrorData> espejos;

    [System.Serializable]
    public class MirrorData
    {
        public string mirrorTitle;    // Título del espejo
        public string mirrorText;     // Texto que aparecerá dentro del espejo (con efecto typewriter)
        public string[] responses;    // 3 textos para las respuestas
    }
}