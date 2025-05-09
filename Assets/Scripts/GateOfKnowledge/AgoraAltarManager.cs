using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AgoraAltarManager : MonoBehaviour
{
    public static AgoraAltarManager Instance;

    [Header("Referencias")]
    public AgoraCH2Localization localization;
    
    public GameObject altarPanel;
    public GameObject cluePanel;
    public GameObject fusionErrorPanel;
    public GameObject fusionSuccessPanel;
    public GameObject cofreDoradoFinal;
    public GameObject objectOptionsPanel;
    public Image selectedImage;
    public TextMeshProUGUI clueText;
    public Button closeClueButton;
    [Header("Canvas Root")]
    public GameObject canvasRoot; //  AgoraChestCanvas
    public Button closeErrorButton;
    public Button closeSuccessButton;

    [Header("UI dinámicos")]
    public List<Button> objectButtons; // Botones con imagen del objeto
    public List<Button> clueButtons;   // Botones para ver pista

    private AgoraCH2Localization.ChestEntry[] chests;
    private Dictionary<string, bool> objectUsed = new();
    private Altar currentAltar;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        chests = localization.chests;
        foreach (var c in chests) objectUsed[c.chestID] = false;

        altarPanel.SetActive(false);
        canvasRoot.SetActive(false);
        cluePanel.SetActive(false);
        fusionErrorPanel.SetActive(false);
        fusionSuccessPanel.SetActive(false);
        cofreDoradoFinal.SetActive(false);

        closeClueButton.onClick.AddListener(() =>
        {
            cluePanel.SetActive(false);
            objectOptionsPanel.SetActive(true);
            selectedImage.gameObject.SetActive(true);
        });
        closeErrorButton.onClick.AddListener(() =>
        {
            fusionErrorPanel.SetActive(false);
            CheckAndCloseCanvas();
        });

        closeSuccessButton.onClick.AddListener(() =>
        {
            fusionSuccessPanel.SetActive(false);
            CheckAndCloseCanvas();
        });
    }
    void CheckAndCloseCanvas()
    {
        if (!altarPanel.activeSelf &&
            !fusionErrorPanel.activeSelf &&
            !fusionSuccessPanel.activeSelf &&
            !cluePanel.activeSelf)
        {
            canvasRoot.SetActive(false);
        }
    }

    public void OpenAltarUI(Altar altar)
    {
        currentAltar = altar;
        canvasRoot.SetActive(true);
        altarPanel.SetActive(true);
        cluePanel.SetActive(false);
        selectedImage.sprite = null;
        clueText.text = "";

        for (int i = 0; i < chests.Length; i++)
        {
            objectButtons[i].GetComponent<Image>().sprite = chests[i].chestImage;
            objectButtons[i].interactable = !objectUsed[chests[i].chestID];

            int index = i;
            objectButtons[i].onClick.RemoveAllListeners();
            objectButtons[i].onClick.AddListener(() => OnObjectSelected(index));

            clueButtons[i].onClick.RemoveAllListeners();
            clueButtons[i].onClick.AddListener(() => ShowClue(index));
        }
    }

    void ShowClue(int index)
    {
        clueText.text = chests[index].textClueAgora;
        cluePanel.SetActive(true);
        objectOptionsPanel.SetActive(false);
        selectedImage.gameObject.SetActive(false);
    }

    void OnObjectSelected(int index)
    {
        objectUsed[chests[index].chestID] = true;
        selectedImage.sprite = chests[index].chestImage;
        currentAltar.SetObject(chests[index].chestID, chests[index].chestImage);
        altarPanel.SetActive(false);
        CheckAndCloseCanvas();
    }
    private void ResetAllAltars()
    {
        Altar[] altars = FindObjectsOfType<Altar>();

        foreach (var altar in altars)
        {
            altar.ResetAltar();
        }

        foreach (var key in new List<string>(objectUsed.Keys))
        {
            objectUsed[key] = false;
        }
    }
    public void TryFusion()
    {
        canvasRoot.SetActive(true);

        Altar[] altars = FindObjectsOfType<Altar>();
        if (altars.Length < 5)
        {
            Debug.LogWarning("Faltan altares en escena.");
            return;
        }

        bool allPlaced = true;
        bool allCorrect = true;

        foreach (var altar in altars)
        {
            string placedID = altar.GetPlacedChestID();

            if (string.IsNullOrEmpty(placedID))
            {
                allPlaced = false;
                break;
            }

            var expected = System.Array.Find(chests, c => c.altarNumber == altar.altarNumber);
            if (expected == null || expected.chestID != placedID)
            {
                allCorrect = false;
            }
        }

        if (!allPlaced)
        {
            fusionErrorPanel.SetActive(true);
            Debug.Log("Fusión fallida: no todos los altares tienen objetos colocados.");
            ResetAllAltars();
            return;
        }

        if (!allCorrect)
        {
            fusionErrorPanel.SetActive(true);
            Debug.Log("Fusión fallida: los objetos están colocados en el orden incorrecto.");
            ResetAllAltars();
            return;
        }

        // Éxito
        fusionSuccessPanel.SetActive(true);
        cofreDoradoFinal.SetActive(true);
        Debug.Log("Fusión completada correctamente.");
    }

}
