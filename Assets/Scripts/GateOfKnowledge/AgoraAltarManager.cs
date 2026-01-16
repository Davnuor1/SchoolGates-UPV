using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class AgoraAltarManager : MonoBehaviour
{
    public static AgoraAltarManager Instance;

    [Header("Referencias")]
    //public AgoraCH2Localization localization;
    [Header("Traducciones")]
    [SerializeField] private AgoraCH2Localization localizacionES;
    [SerializeField] private AgoraCH2Localization localizacionIT;
    [SerializeField] private AgoraCH2Localization localizacionDE;
    [SerializeField] private AgoraCH2Localization localizacionEN;
    [SerializeField] private AgoraCH2Localization localizacionFI;
    public AgoraCH2Localization localization;
    private string codeLanguage;
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
        defineLanguage();
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
        fusionErrorPanel.GetComponentInChildren<TextMeshProUGUI>().text = localization.fusionErrorText;
        fusionSuccessPanel.GetComponentInChildren<TextMeshProUGUI>().text = localization.fusionSuccessText;

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
            GameManager.instance.skillTreeController.Unlock("2");
            GameManager.instance.skillTreeController.Unlock("3");
            GameManager.instance.uiManager.ToggleSkillTreeUI();
            
        });
    }
    public void defineLanguage()
    {
        codeLanguage = LocalizationManager.Instance.CurrentLanguage;
        if (codeLanguage == "es") { localization = localizacionES; }
        else if (codeLanguage == "it") { localization = localizacionIT; }
        else if (codeLanguage == "de") { localization = localizacionDE; }
        else if (codeLanguage == "en") { localization = localizacionEN; }
        else if (codeLanguage == "fi") { localization = localizacionFI; }
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
