using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestionCollection", menuName = "Quiz/QuestionCollection")]
public class QuestionCollection : ScriptableObject
{
    public List<Question> questions;
}
