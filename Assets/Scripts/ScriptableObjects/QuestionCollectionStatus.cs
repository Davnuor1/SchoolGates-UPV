using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "QuestionCollectionStatus", menuName = "Quiz/QuestionCollectionStatus")]
public class QuestionCollectionStatus : ScriptableObject
{
    public List<string> completedCollections;

    public void MarkCollectionAsCompleted(string collectionName)
    {
        if (!completedCollections.Contains(collectionName))
        {
            completedCollections.Add(collectionName);
        }
    }

    public bool IsCollectionCompleted(string collectionName)
    {
        return completedCollections.Contains(collectionName);
    }
}
