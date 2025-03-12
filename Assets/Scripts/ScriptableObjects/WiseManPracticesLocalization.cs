using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "WiseManPracticesLocalization", menuName = "Localization/WiseManPractices")]
public class WiseManPracticesLocalization : ScriptableObject
{
    public enum VignettePlacement
    {
        BeforeConflict1,
        AfterConflict1,
        BeforeConflict2,
        AfterConflict2,
        BeforeConflict3,
        AfterConflict3,
        Conclusion
    }

    [System.Serializable]
    public class Vignette
    {
        public Sprite backgroundImage;
        [TextArea] public string text;
        public VignettePlacement placement;
    }

    [System.Serializable]
    public class ResponseOption
    {
        public string responseText;
        [TextArea] public string responseFeedback;
    }

    [System.Serializable]
    public class Dialogue
    {
        public string characterName;
        public Sprite portrait;
        [TextArea] public string text;
        public bool requiresResponse;
        public List<ResponseOption> responseOptions;
    }

    [System.Serializable]
    public class Concept
    {
        public string term; // Nombre de la palabra clave
        [TextArea] public string description; // Explicación de la palabra
    }

    [System.Serializable]
    public class Conflict
    {
        public List<Dialogue> dialogues;
        public bool requiresAllFeedback;
        public bool isBranchingConflict; // Indica si este conflicto tiene ramas narrativas
    }

    public List<Vignette> vignettes;
    public List<Conflict> conflicts;

    public List<Concept> conflict1Concepts; // Lista de conceptos para el Conflicto 1
    public List<Concept> conflict3Concepts; // Lista de conceptos para el Conflicto 3
}
