using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ToolAction : ScriptableObject
{
    public virtual bool OnApply(Vector2 worldPoint,Vector2Int toolDirection, Animator animator)
    {
        Debug.LogWarning("OnApply is not implemented");
        return true;
    }

    public virtual bool OnApplyToTilemap(Vector3Int tilemapPosition,Tilemap tilemap,Item item,Vector2Int toolDirection)
    {
        Debug.LogWarning("OnApplyToTilemap is not implemented");
        return true;
    }
    public virtual void OnItemUsed(Item itemUsed, Inventory inventory)
    {

    }
}
