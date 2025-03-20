using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCinematic", menuName = "Cinematic/Cinematic Data")]
public class CinematicData : ScriptableObject
{
    [System.Serializable]
    public class Vignette
    {
        public Sprite image; // Imagen de la viñeta
        public string text;  // Texto asociado a la viñeta
    }

    public List<Vignette> vignettes = new List<Vignette>(); // Lista de viñetas
    public string nextButtonText; // Texto del botón "Next"
}
