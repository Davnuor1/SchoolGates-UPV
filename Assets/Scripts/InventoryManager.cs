using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public Dictionary<string, Inventory> inventoryByName = new Dictionary<string, Inventory>();
    [Header("Backpack")]
    public Inventory backpack;
    public int backpackSlotCount;
    [Header("Toolbar")]
    public Inventory toolbar;
    public int toolbarSlotCount;
    

    [Header("SellShop2")]
    public Inventory sellShop2;
    public int sellShopSlotCount2;
    private void Awake()
    {
        backpack = new Inventory(backpackSlotCount);
        sellShop2 = new Inventory(sellShopSlotCount2);
        toolbar = new Inventory(toolbarSlotCount);
        

        
        inventoryByName.Add("Backpack", backpack);
        inventoryByName.Add("SellShop2", sellShop2);
        inventoryByName.Add("Toolbar", toolbar);
        

    }
    public void Add(string inventoryName,Item item)
    {
        if(inventoryByName.ContainsKey(inventoryName)){
            inventoryByName[inventoryName].Add(item);
        }
    }

    public Inventory GetInventoryByName(string inventoryName)
    {
        
        if (inventoryByName.ContainsKey(inventoryName))
        {
            
            return inventoryByName[inventoryName];
            
        }
        return null;
    }
}
