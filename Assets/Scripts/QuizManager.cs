using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class QuizManager : MonoBehaviour
{
    public TextMeshProUGUI questionText;
    public TextMeshProUGUI[] answerTexts;
    public GameObject quizPanel;
    public GameObject imagenCorrecto;
    public GameObject imagenIncorrecto;
    public GameObject panelPostMinijuego;
    public bool showPostMinijuegoPanel;
    public int maxCorrectAnswers = 1;
    public Image collectionIconImage; // Imagen para mostrar el icono de la colección
    public TextMeshProUGUI nombreCollection;
    public QuestionCollectionStatus questionCollectionStatus; // Referencia al ScriptableObject
    public QuizSelector quizSelector; // Referencia al QuizSelector

    public GameObject rewardPanel; // Panel que se activará al completar las colecciones necesarias
    public int requiredCollectionsCompleted = 5; // Número de colecciones requeridas para mostrar el panel

    private QuestionCollection questionCollection;
    private List<Question> questions;
    private Question currentQuestion;
    private int currentQuestionIndex;
    private int correctAnswersCount;

    public void SetQuestionCollection(QuestionCollection collection)
    {
        questionCollection = collection;
        if (collectionIconImage != null && questionCollection.collectionIcon != null)
        {
            collectionIconImage.sprite = questionCollection.collectionIcon;
            collectionIconImage.gameObject.SetActive(true); // Mostrar la imagen si no estaba activa
            nombreCollection.text = questionCollection.collectionName;
            nombreCollection.gameObject.SetActive(true);
        }
    }

    public void StartQuiz()
    {
        if (questionCollection == null)
        {
            Debug.LogError("Question collection is not set!");
            return;
        }

        questions = new List<Question>(questionCollection.questions);
        quizPanel.SetActive(true);
        imagenCorrecto.SetActive(false);
        imagenIncorrecto.SetActive(false);
        if (panelPostMinijuego != null)
        {
            panelPostMinijuego.SetActive(false);
        }
        if (rewardPanel != null)
        {
            rewardPanel.SetActive(false); // Asegurarse de que el panel de recompensa esté oculto al inicio
        }
        correctAnswersCount = 0;
        GenerateRandomQuestion();
    }

    void GenerateRandomQuestion()
    {
        if (questions.Count > 0)
        {
            currentQuestionIndex = Random.Range(0, questions.Count);
            currentQuestion = questions[currentQuestionIndex];

            questionText.text = currentQuestion.questionText;
            for (int i = 0; i < answerTexts.Length; i++)
            {
                if (i < currentQuestion.answers.Length)
                {
                    Debug.Log("Assigning answer " + i + ": " + currentQuestion.answers[i]);
                    answerTexts[i].text = currentQuestion.answers[i];
                }
                else
                {
                    Debug.LogWarning("Answer index out of range: " + i);
                }
            }
        }
        else
        {
            Debug.Log("No more questions available.");
        }
    }

    public void AnswerButtonClicked(int index)
    {
        if (index == currentQuestion.correctAnswerIndex)
        {
            ShowCorrectImage();
            correctAnswersCount++;
        }
        else
        {
            ShowIncorrectImage();
        }

        quizPanel.SetActive(false);
    }

    void ShowCorrectImage()
    {
        imagenCorrecto.SetActive(true);
        imagenIncorrecto.SetActive(false);
    }

    void ShowIncorrectImage()
    {
        imagenIncorrecto.SetActive(true);
        imagenCorrecto.SetActive(false);
    }

    public void NextQuestionButtonClicked()
    {
        if (correctAnswersCount >= maxCorrectAnswers)
        {
            EndQuiz();
        }
        else
        {
            questions.RemoveAt(currentQuestionIndex);

            if (questions.Count > 0)
            {
                quizPanel.SetActive(true);
                imagenCorrecto.SetActive(false);
                imagenIncorrecto.SetActive(false);
                GenerateRandomQuestion();
            }
            else
            {
                Debug.Log("All questions answered.");
                EndQuiz();
            }
        }
    }

    void EndQuiz()
    {
        Debug.Log("Quiz Finished");
        if (showPostMinijuegoPanel && panelPostMinijuego != null)
        {
            imagenCorrecto.SetActive(false);
            imagenIncorrecto.SetActive(false);
            panelPostMinijuego.SetActive(true);
        }

        // Marcar la colección como completada
        if (questionCollectionStatus != null && questionCollection != null)
        {
            questionCollectionStatus.MarkCollectionAsCompleted(questionCollection.name);
        }

        // Notificar al QuizSelector
        if (quizSelector != null)
        {
            quizSelector.UpdateCollectionButtons();
        }

        // Comprobar si el usuario ha completado suficientes colecciones para activar el panel de recompensa
        if (rewardPanel != null && questionCollectionStatus.completedCollections.Count >= requiredCollectionsCompleted)
        {
            rewardPanel.SetActive(true);
        }
    }
}
