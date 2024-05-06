using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Data/Tool Action/Seed")]
public class SeedTile : ToolAction
{
    public override bool OnApplyToTilemap(Vector3Int tilemapPosition, Tilemap tilemap,Item item,Vector2Int toolDirection)
    {
        if (GameManager.instance.tileManager.Check(tilemapPosition))
        {
            GameManager.instance.tileManager.SeedTile(tilemapPosition,item.data.crop);
            return true;
        }
        else {  }
            

        return false;
    }

    public override void OnItemUsed(Item usedItem, Inventory inventory)
    {
        //Debug.Log("Fase2Remove");
        inventory.Remove(Toolbar_UI.numSlot);
        GameManager.instance.uiManager.RefreshInventoryUI("Toolbar");
    }
}
