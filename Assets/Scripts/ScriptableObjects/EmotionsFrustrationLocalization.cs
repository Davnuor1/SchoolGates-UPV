using UnityEngine;

[CreateAssetMenu(fileName = "EmotionsFrustrationLocalization", menuName = "Localization/EmotionsFrustration")]
public class EmotionsFrustrationLocalization : ScriptableObject
{
    [TextArea] public string introText;
    public string nextButtonText;

    [TextArea] public string meditationDialogue1;
    [TextArea] public string meditationDialogue2;
    [TextArea] public string meditationDialogue3;
    [TextArea] public string meditationInstruction1; // "Close your eyes…"
    [TextArea] public string meditationInstruction2; // "Now, breathe in and out"
    [TextArea] public string meditationInstruction3; // Texto mientras se reproduce respiración
    [TextArea] public string meditationFinalText1;
    [TextArea] public string meditationFinalText2;
    [TextArea] public string meditationFinalText3;
    public string meditationNextButtonText;
}
