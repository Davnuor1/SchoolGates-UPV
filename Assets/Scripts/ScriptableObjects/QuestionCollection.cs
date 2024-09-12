using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestionCollection", menuName = "Quiz/QuestionCollection")]
public class QuestionCollection : ScriptableObject
{
    public string collectionName;
    public List<Question> questions;
    public Sprite collectionIcon; // Icono asociado a la colección
}
