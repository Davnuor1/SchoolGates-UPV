using UnityEngine;
using UnityEngine.UI;

public class QuizSelector : MonoBehaviour
{
    public QuizManager quizManager;
    //public QuestionCollection[] questionCollections;
    public QuestionCollectionStatus questionCollectionStatus; // Referencia al ScriptableObject
    public Button[] collectionButtons;
    public GameObject[] completionVisuals;
    [Header("Traducciones")]
    public QuestionCollection[] questionCollectionsES;
    public QuestionCollection[] questionCollectionsIT;
    public QuestionCollection[] questionCollectionsDE;
    public QuestionCollection[] questionCollectionsEN;
    public QuestionCollection[] questionCollectionsFI;
    public QuestionCollection[] questionCollections;
    
    private string codeLanguage;

    void Start()
    {
        defineLanguage();
        UpdateCollectionButtons();
    }
    public void defineLanguage()
    {
        codeLanguage = LocalizationManager.Instance.CurrentLanguage;
        if (codeLanguage == "es") { questionCollections = questionCollectionsES; }
        else if (codeLanguage == "it") { questionCollections = questionCollectionsIT; }
        else if (codeLanguage == "de") { questionCollections = questionCollectionsDE; }
        else if (codeLanguage == "en") { questionCollections = questionCollectionsEN; }
        else if (codeLanguage == "fi") { questionCollections = questionCollectionsFI; }
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
