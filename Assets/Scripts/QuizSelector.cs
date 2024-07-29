using UnityEngine;
using UnityEngine.UI;

public class QuizSelector : MonoBehaviour
{
    public QuizManager quizManager;
    public QuestionCollection[] questionCollections;
    public QuestionCollectionStatus questionCollectionStatus; // Referencia al ScriptableObject
    public Button[] collectionButtons;
    public GameObject[] completionVisuals;

    void Start()
    {
        UpdateCollectionButtons();
    }

    public void SelectQuiz(int index)
    {
        if (index >= 0 && index < questionCollections.Length)
        {
            quizManager.SetQuestionCollection(questionCollections[index]);
            quizManager.StartQuiz();
        }
        else
        {
            Debug.LogError("Quiz index out of range!");
        }
    }

    public void UpdateCollectionButtons()
    {
        for (int i = 0; i < questionCollections.Length; i++)
        {
            bool isCompleted = questionCollectionStatus.IsCollectionCompleted(questionCollections[i].name);
            collectionButtons[i].interactable = !isCompleted;
            completionVisuals[i].SetActive(isCompleted);
        }
    }
}
