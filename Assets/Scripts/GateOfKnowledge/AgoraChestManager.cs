using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AgoraChestManager : MonoBehaviour
{
    public static AgoraChestManager Instance;

    [Header("Localización")]
    public AgoraCH2Localization localization;

    [Header("UI")]
    public GameObject canvasRoot;
    public GameObject chestPanel;
    public Image objectImage;
    public TextMeshProUGUI textChest;
    public TextMeshProUGUI textClueAgora;
    public GameObject robotGuardian;

    private int chestsOpened = 0;
    private int totalChests => localization.chests.Length;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        canvasRoot.SetActive(false);
    }

    public void OpenChest(string chestID)
    {
        var entry = GetChestEntry(chestID);
        if (entry == null) return;

        objectImage.sprite = entry.chestImage;
        textChest.text = entry.textChest;
        textClueAgora.text = entry.textClueAgora;

        canvasRoot.SetActive(true);
        chestPanel.SetActive(true);
    }

    public void CloseChest()
    {
        chestPanel.SetActive(false);
        canvasRoot.SetActive(false);
    }

    public void NotifyChestOpened()
    {
        chestsOpened++;
        if (chestsOpened >= totalChests)
        {
            //Aqui deberemos actualizar mision

            Debug.Log("Minijuego AgoraChallenge02 finalizado,ACTIVAR MISION");
            robotGuardian.SetActive(false);
        }
    }

    private AgoraCH2Localization.ChestEntry GetChestEntry(string id)
    {
        foreach (var chest in localization.chests)
        {
            if (chest.chestID == id)
                return chest;
        }
        Debug.LogWarning("No se encontró el cofre con ID: " + id);
        return null;
    }
}
