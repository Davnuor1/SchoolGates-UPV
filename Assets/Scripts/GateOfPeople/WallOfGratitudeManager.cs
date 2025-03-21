using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WallOfGratitudeManager : MonoBehaviour
{
    [Header("UI Panels")]
    public GameObject panelMain;
    public GameObject panelStoneView;
    public GameObject panelGratitudeWall;

    [Header("Upper Texts")]
    public TextMeshProUGUI textUpperMain;
    public TextMeshProUGUI textUpperStoneView;
    public TextMeshProUGUI textUpperGratitudeWall;

    [Header("Buttons")]
    public Button buttonFinish;
    public Button buttonAddToWall;
    public Button buttonBackToMain;
    public Button buttonNextStone;
    public Button buttonPreviousStone;
    public Button buttonBackToMainFromWall;

    [Header("Stone Display")]
    public TextMeshProUGUI textStoneTitle;
    public GameObject containerStoneDescription;

    [Header("Wall Content")]
    public GameObject containerGratitudeWall;
    public GameObject prefabGratitudeStone;

    [Header("Localization")]
    public WallOfGratitudeLocalization localizationData;

    [Header("Stone Buttons in Main Panel")] //  Lista de botones de piedras en Panel_Main
    public List<Button> stoneButtons;

    private List<GratitudeStone> allStones = new List<GratitudeStone>();
    private List<GratitudeStone> selectedStones = new List<GratitudeStone>();
    private int currentStoneIndex = 0;
    private int stonesViewed = 0;
    public GameObject portalSalida;
    private Image stoneImage; // Se obtiene automáticamente desde el Panel_StoneView

    private void Start()
    {
        LoadLocalization();
        LoadStones();
        AssignStoneTitles();

        //  Buscar la imagen de la piedra dentro del Panel_StoneView
        stoneImage = panelStoneView.GetComponentInChildren<Image>();
    }

    void LoadLocalization()
    {
        if (localizationData == null)
        {
            Debug.LogError("No localization data assigned!");
            return;
        }

        textUpperMain.text = localizationData.upperTextMain;
        textUpperStoneView.text = localizationData.upperTextStoneView;
        textUpperGratitudeWall.text = localizationData.upperTextGratitudeWall;

        buttonFinish.GetComponentInChildren<TextMeshProUGUI>().text = localizationData.finishButton;
        buttonAddToWall.GetComponentInChildren<TextMeshProUGUI>().text = localizationData.addToWallButton;
        buttonBackToMain.GetComponentInChildren<TextMeshProUGUI>().text = localizationData.backButton;
        buttonNextStone.GetComponentInChildren<TextMeshProUGUI>().text = localizationData.nextButton;
        buttonPreviousStone.GetComponentInChildren<TextMeshProUGUI>().text = localizationData.previousButton;
        buttonBackToMainFromWall.GetComponentInChildren<TextMeshProUGUI>().text = localizationData.backButton;
    }

    void LoadStones()
    {
        allStones.Clear();
        foreach (var stone in localizationData.stones)
        {
            allStones.Add(new GratitudeStone
            {
                title = stone.title,
                descriptions = new List<string>(stone.descriptions),
                icon = null // Ahora tomamos el icono desde el ScriptableObject
            });
        }
    }

    void AssignStoneTitles()
    {
        if (stoneButtons.Count != allStones.Count)
        {
            Debug.LogWarning("El número de botones en Panel_Main no coincide con el número de piedras en la localización.");
        }

        for (int i = 0; i < stoneButtons.Count && i < allStones.Count; i++)
        {
            TextMeshProUGUI buttonText = stoneButtons[i].GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
            {
                buttonText.text = allStones[i].title;
            }
            else
            {
                Debug.LogWarning($"El botón de la piedra {i} no tiene un TextMeshProUGUI asignado.");
            }

            int index = i; //  Capturar la variable local correctamente
            stoneButtons[i].onClick.AddListener(() => ShowStone(index)); //  Asignar la función ShowStone() dinámicamente
        }
    }

    public void ShowStone(int index)
    {
        Debug.Log($"Intentando mostrar la piedra {index}");

        if (index < 0 || index >= allStones.Count)
        {
            Debug.LogError($"Índice fuera de rango: {index}. Lista de piedras tiene {allStones.Count} elementos.");
            return;
        }

        currentStoneIndex = index;
        var stone = allStones[index];

        Debug.Log($"Mostrando piedra: {stone.title}");

        textStoneTitle.text = stone.title;

        if (stoneImage != null)
        {
            if (stone.icon != null)
            {
                stoneImage.sprite = stone.icon;
                stoneImage.enabled = true;
            }
            else
            {
                Debug.LogWarning($"La piedra {stone.title} no tiene icono asignado.");
                stoneImage.enabled = false;
            }
        }
        else
        {
            Debug.LogError("La referencia de stoneImage es NULL.");
        }

        foreach (Transform child in containerStoneDescription.transform)
            Destroy(child.gameObject);

        Debug.Log($"La piedra tiene {stone.descriptions.Count} frases.");

        foreach (string description in stone.descriptions)
        {
            GameObject newText = new GameObject("StoneDescription", typeof(TextMeshProUGUI));
            newText.transform.SetParent(containerStoneDescription.transform);
            TextMeshProUGUI tmp = newText.GetComponent<TextMeshProUGUI>();
            tmp.text = description;
            tmp.fontSize = 30;
            tmp.alignment = TextAlignmentOptions.Center;
        }

        panelMain.SetActive(false);
        panelStoneView.SetActive(true);

        stonesViewed++;
        CheckFinishCondition();
    }

    public void NextStone()
    {
        if (currentStoneIndex < allStones.Count - 1)
        {
            ShowStone(currentStoneIndex + 1);
        }
    }

    public void PreviousStone()
    {
        if (currentStoneIndex > 0)
        {
            ShowStone(currentStoneIndex - 1);
        }
    }

    public void AddStoneToWall()
    {
        if (!selectedStones.Contains(allStones[currentStoneIndex]))
        {
            selectedStones.Add(allStones[currentStoneIndex]);

            GameObject newStone = Instantiate(prefabGratitudeStone, containerGratitudeWall.transform);
            newStone.GetComponentInChildren<TextMeshProUGUI>().text = allStones[currentStoneIndex].title;
        }

        CheckFinishCondition();
    }

    void CheckFinishCondition()
    {
        if (stonesViewed >= allStones.Count && selectedStones.Count >= 3)
        {
            buttonFinish.interactable = true;
        }
    }

    public void ReturnToMain()
    {
        panelStoneView.SetActive(false);
        panelGratitudeWall.SetActive(false);
        panelMain.SetActive(true);
    }

    public void ShowGratitudeWall()
    {
        Debug.Log("Click a gratitude wall");
        panelMain.SetActive(false);
        panelStoneView.SetActive(false);
        panelGratitudeWall.SetActive(true);
    }

    public void FinishMinigame()
    {
        Debug.Log("Minijuego completado");
        portalSalida.SetActive(true);
        panelMain.SetActive(false);
    }
}
