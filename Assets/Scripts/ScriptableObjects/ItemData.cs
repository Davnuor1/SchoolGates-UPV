using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Item Data", menuName = "Item Data", order = 50)]
public class ItemData : ScriptableObject
{
    public string itemName = "Item Name";
    public Sprite icon;
    public string itemType = "Item Type";
    public ToolAction onAction;
    public ToolAction onTilemapAction;
    public ToolAction onItemUsed;
    public Crop crop;
    public int price;
    public GameObject prefab;
}
