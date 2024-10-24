using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CurrencyManager : MonoBehaviour
{
    [SerializeField] int amount;
    [SerializeField] TMPro.TextMeshProUGUI text;
    [SerializeField] Item itemToBuy;
    [SerializeField] Item itemToBuy2;
    [SerializeField] Player player;
    [SerializeField] GameObject shop;
    [SerializeField] GameObject sellShop;

    [SerializeField] Item toolForToolbar1;
    [SerializeField] Item toolForToolbar2;
    [SerializeField] Item toolForToolbar3;

    private Inventory sellInventory;
    private int price=0;


    private void Start()
    {
        amount = 100;
        UpdateText();
        //sellInventory = GameManager.instance.player.inventory.GetInventoryByName("SellShop2");
        //player.inventory.Add("Toolbar", toolForToolbar1);
        //player.inventory.Add("Toolbar", toolForToolbar2);
        //player.inventory.Add("Toolbar", toolForToolbar3);
        //GameManager.instance.uiManager.RefreshInventoryUI("Toolbar");

    }

    private void UpdateText()
    {
        text.text = amount.ToString();
    }

    public void Buy()
    {
        price = itemToBuy.data.price;
        if (amount >= price)
        {
            player.inventory.Add("Backpack", itemToBuy);
            amount -= price;
            GameManager.instance.uiManager.RefreshInventoryUI("Backpack");
            UpdateText();
        }

    }

    public void Buy2()
    {
        price = itemToBuy2.data.price;
        if (amount >= price)
        {
            player.inventory.Add("Backpack", itemToBuy2);
            amount -= price;
            GameManager.instance.uiManager.RefreshInventoryUI("Backpack");
            UpdateText();
        }

    }

    public void ToggleShopUI()
    {
        if (shop != null)
        {
            if (!shop.activeSelf)
            {
                sellShop.SetActive(false);
                shop.SetActive(true);
                //RefreshInventoryUI("Backpack");
            }
            else
            {
                shop.SetActive(false);
            }
        }

    }
    public void ToggleSellShopUI()
    {
        if (sellShop != null)
        {
            if (!sellShop.activeSelf)
            {
                shop.SetActive(false);
                sellShop.SetActive(true);
                //RefreshInventoryUI("Backpack");
            }
            else
            {
                sellShop.SetActive(false);
            }
        }

    }
    public void Sell()
    {
        price = sellInventory.slots[0].item.data.price;
        sellInventory.Remove(0);
        GameManager.instance.uiManager.RefreshInventoryUI("SellShop2");
        amount += price;
        UpdateText();
    }

}
