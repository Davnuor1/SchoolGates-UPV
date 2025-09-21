using UnityEngine;

[CreateAssetMenu(fileName = "CardMinigameLocalization", menuName = "Localization/CardMinigame")]
public class CardMinigameLocalization : ScriptableObject
{
    [Header("UI")]
    public string instructionText;            // texto superior con instrucciones
    public string btnConfirmLabel;            // texto del botón Confirmar
    public string btnCancelLabel;             // opcional

    [Header("Nombres de cartas")]
    public string nameYourself;               // Yourself
    public string nameMonster;                // Monster
    public string namePeople;                 // People
    public string nameLove;                   // Love

    [Header("Textos de finales (para el recuadro dinámico)")]
    public string final1Text;                 // Monster o Monster+Love
    public string final2Text;                 // People o People+Love
    public string final3Text;                 // Love+Yourself+People
    public string final4Text;                 // Love+People+Monster
    public string final5Text;                 // Love+People+Self (mismo set que final3; ver prioridad)
}
