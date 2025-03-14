using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WallOfGratitudeLocalization", menuName = "Localization/WallOfGratitude")]
public class WallOfGratitudeLocalization : ScriptableObject
{
    [Header("General UI Texts")]
    public string upperTextMain;
    public string upperTextStoneView;
    public string upperTextGratitudeWall;
    public string finishButton;
    public string addToWallButton;
    public string backButton;
    public string nextButton;
    public string previousButton;
    public string gratitudeWallTitle;

    [Header("Gratitude Stones")]
    public List<StoneData> stones;

    [System.Serializable]
    public class StoneData
    {
        public string title;  // Título de la piedra
        [TextArea(2, 4)] public List<string> descriptions;  // Frases dentro de la piedra
    }
}
