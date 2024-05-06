using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ToolManager : MonoBehaviour
{
    public Animator animator;
    public Movement character;
    public Rigidbody2D rgbd2d;
    [SerializeField] float offsetDistance = 1f;
    [SerializeField] Tilemap interactableMap;
    //[SerializeField] float sizeInteractableArea = 1.2f;
    [SerializeField] ToolAction onTilePickUp;

    
    public bool UseTool(Vector3Int position, Inventory inventory, Vector3 direction,Vector2Int toolDirection)
    {
        if (GameManager.instance.tileManager.IsInteractable(position))
        {
            //Debug.Log("Tile is interactable");

            Item item = inventory.slots[Toolbar_UI.selectedSlot.slotID].item;

            try 
            { 
                if (item.data==null)
                {

                }
            } catch (NullReferenceException )
            {
                Debug.Log("HOLAAA");
                PickUpTile(position,toolDirection);
                return false;
            }
            
            //Debug.Log("Fase1");
            if (item.data.onTilemapAction == null) { return false; }
            //Debug.Log("Fase2");
            
            bool complete = item.data.onTilemapAction.OnApplyToTilemap(position, interactableMap,item,toolDirection);
            if (complete == true)
            {
                //animator.SetTrigger("act");
                //Debug.Log("Fase1Remove");
                if (item.data.onItemUsed) { item.data.onItemUsed.OnItemUsed(item, inventory); }
                
            }

            return complete;
            

        }
        return false;
        
    }
    public void AnimateHoe(Vector2Int toolDirection)
    {
        //Debug.Log("horizontal"+ direction.x);
        //Debug.Log("vertical"+ direction.y);
        animator.SetInteger("toolDirHorizontal", toolDirection.x);
        animator.SetInteger("toolDirVertical", toolDirection.y);
        animator.SetTrigger("useHoe"); 
    }
    public bool UseTool2(Inventory inventory,Vector2Int toolDirection)
    {
        Vector2 position =rgbd2d.position+ character.lastMotionVector * offsetDistance;
        Item item = inventory.slots[Toolbar_UI.selectedSlot.slotID].item;
        
        if (item.data.name == null) { return false; }
        
        if (item.data.onAction == null) { return false; }
        //animator.SetTrigger("act");
        bool complete=item.data.onAction.OnApply(position,toolDirection,animator);
        

        return complete;
    }
    private void PickUpTile(Vector3Int position,Vector2Int toolDirection)
    {
        if (onTilePickUp == null) { return ; }
        onTilePickUp.OnApplyToTilemap(position, interactableMap, null,toolDirection);
    }
}
