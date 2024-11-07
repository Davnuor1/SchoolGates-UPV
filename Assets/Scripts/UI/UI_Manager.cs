using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public Dictionary<string, Inventory_UI> inventoryUIByName = new Dictionary<string, Inventory_UI>();
    public GameObject inventoryPanel;
    public GameObject menuPanel;
    public GameObject skillTreePanel;
    public GameObject bookOfCluesPanel;
    public GameObject travelJournalPanel;
    public GameObject statsPanel;


    public List<Inventory_UI> inventory_UIs;
    public static Slot_UI draggedSlot;
    public static Image draggedIcon;
    public static bool dragSingle;
    public bool canToggle=true;

    public void Awake()
    {
        Initialize();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab)||Input.GetKeyDown(KeyCode.B))
        {
            if (canToggle)
            {
                ToggleMenuUI();
            }
            //ToggleInventoryUI();
            
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            dragSingle = true;
        }
        else
        {
            dragSingle = false;
        }
    }
    public void ToggleInventoryUI()
    {
        if (inventoryPanel != null)
        {
            if (!inventoryPanel.activeSelf)
            {
                inventoryPanel.SetActive(true);
                RefreshInventoryUI("Backpack");
            }
            else
            {
                inventoryPanel.SetActive(false);
            }
        }

    }
    public void ToggleMenuUI()
    {
        if (menuPanel != null)
        {
            if (!menuPanel.activeSelf)
            {
                if (!statsPanel.activeSelf & !inventoryPanel.activeSelf & !skillTreePanel.activeSelf & !bookOfCluesPanel.activeSelf & !travelJournalPanel.activeSelf)
                {
                    menuPanel.SetActive(true);
                }
                
                
            }
            else
            {
                menuPanel.SetActive(false);
            }
        }

    }
    public void ToggleStatsUI()
    {
        if (statsPanel != null)
        {
            if (!statsPanel.activeSelf)
            {
                statsPanel.SetActive(true);

            }
            else
            {
                statsPanel.SetActive(false);
            }
        }

    }
    public void ToggleSkillTreeUI()
    {
        if (skillTreePanel != null)
        {
            if (!skillTreePanel.activeSelf)
            {
                skillTreePanel.SetActive(true);

            }
            else
            {
                skillTreePanel.SetActive(false);
            }
        }

    }
    public void ToggleBookOfCluesUI()
    {
        if (bookOfCluesPanel != null)
        {
            if (!bookOfCluesPanel.activeSelf)
            {
                bookOfCluesPanel.SetActive(true);

            }
            else
            {
                bookOfCluesPanel.SetActive(false);
            }
        }

    }
    public void ToggleTravelJournalUI()
    {
        if (travelJournalPanel != null)
        {
            if (!travelJournalPanel.activeSelf)
            {
                travelJournalPanel.SetActive(true);

            }
            else
            {
                travelJournalPanel.SetActive(false);
            }
        }

    }
    public void RefreshInventoryUI(string inventoryName)
    {
        if (inventoryUIByName.ContainsKey(inventoryName))
        {
            inventoryUIByName[inventoryName].Refresh();
        }
    }
    public void RefreshAll()
    {
        foreach(KeyValuePair<string, Inventory_UI>keyValuePair in inventoryUIByName)
        {
            keyValuePair.Value.Refresh();
        }
    }
    public Inventory_UI GetInventoryUI(string inventoryName)
    {
        if (inventoryUIByName.ContainsKey(inventoryName))
        {
            return inventoryUIByName[inventoryName];
        }
        Debug.LogWarning("There is no inventory ui for " + inventoryName);
        return null;
    }
    void Initialize()
    {
        foreach(Inventory_UI ui in inventory_UIs)
        {
            if (!inventoryUIByName.ContainsKey(ui.inventoryName))
            {
                inventoryUIByName.Add(ui.inventoryName, ui);
            }
        }
    }
}
