using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player : MonoBehaviour
{
    public InventoryManager inventory;
    [SerializeField] private Tilemap tileMarker;
    [SerializeField] Animator changeSceneAnimator;
    private Vector3 direction;
    private Vector3Int selectedTilePosition;
    bool selectable;
    float range = 2f;
    private int auxHor;
    private int auxVer;
    
    
    

    private void Awake()
    {
        inventory = GetComponent<InventoryManager>();
        
        
            
    }
    private void Update()
    {
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");
        direction = new Vector3(horizontal, vertical-30f);

        //SelectTile();
        //CanSelectCheck();
        if (Input.GetKeyDown(KeyCode.Space)){
            //ChangePositionPlayer();
        }
        //Marker();
        /* if (Input.GetKeyDown(KeyCode.Space) && selectable == true)
        {
            Vector3Int position2= GetTileBase(Input.mousePosition);
            Vector3Int positionPlayer = GetTileBase(direction);
      
            auxHor = position2.x - (positionPlayer.x+7);
            auxVer = position2.y - (positionPlayer.y+4);
            Vector2Int toolDirection = new Vector2Int(auxHor, auxVer);

            GameManager.instance.toolManager.UseTool(position2, inventory.toolbar, direction,toolDirection);
        }
        if (Input.GetKeyDown(KeyCode.V) && selectable == true)
        {
            Vector3Int position2 = GetTileBase(Input.mousePosition);
            Vector3Int positionPlayer = GetTileBase(direction);
            auxHor = position2.x - (positionPlayer.x + 7);
            auxVer = position2.y - (positionPlayer.y + 4);
            Vector2Int toolDirection = new Vector2Int(auxHor, auxVer);

            GameManager.instance.toolManager.UseTool2(inventory.toolbar,toolDirection);
        } */
    }
    public void DropItem(Item item)
    {
        Vector2 spawnLocation = transform.position;
        
        Vector2 spawnOffset = Random.insideUnitCircle;
        if (spawnOffset.x < 0)
        {
            spawnOffset.x = (float)(spawnOffset.x - 0.3);
        }
        if (spawnOffset.x >= 0)
        {
            spawnOffset.x = (float)(spawnOffset.x + 0.3);
        }
        if (spawnOffset.y < 0)
        {
            spawnOffset.y = (float)(spawnOffset.y - 0.3);
        }
        if (spawnOffset.y >= 0)
        {
            spawnOffset.y = (float)(spawnOffset.y + 0.3);
        }

        //spawnOffset.x = (float)(spawnOffset.x + 0.1);
        //spawnOffset.y = (float)(spawnOffset.y + 0.1);
        //Debug.Log(spawnOffset);
        Item droppedItem= Instantiate(item, spawnLocation + spawnOffset, 
            Quaternion.identity);
        droppedItem.rb2d.AddForce(spawnOffset * .2f, ForceMode2D.Impulse);
    }
    public void DropItem(Item item, int numToDrop)
    {
        for(int i = 0; i < numToDrop; i++)
        {
            DropItem(item);
        }
    
    }
    public void DropItemPosition(Item item, int numToDrop, Vector2 position)
    {
        for (int i = 0; i < numToDrop; i++)
        {
            Vector2 spawnOffset = Random.insideUnitCircle;
            if (spawnOffset.x < 0)
            {
                spawnOffset.x = (float)(spawnOffset.x - 0.3);
            }
            if (spawnOffset.x >= 0)
            {
                spawnOffset.x = (float)(spawnOffset.x + 0.3);
            }
            if (spawnOffset.y < 0)
            {
                spawnOffset.y = (float)(spawnOffset.y - 0.3);
            }
            if (spawnOffset.y >= 0)
            {
                spawnOffset.y = (float)(spawnOffset.y + 0.3);
            }

            //spawnOffset.x = (float)(spawnOffset.x + 0.1);
            //spawnOffset.y = (float)(spawnOffset.y + 0.1);
            //Debug.Log(spawnOffset);
            Item droppedItem = Instantiate(item, position + spawnOffset,
                Quaternion.identity);
            droppedItem.rb2d.AddForce(spawnOffset * .2f, ForceMode2D.Impulse);
        }
    }
    private void SelectTile()
    {
        selectedTilePosition = GetTileBase(Input.mousePosition);
    }
    void CanSelectCheck()
    {
        Vector2 characterposition = transform.position;
        Vector2 cameraPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        selectable = Vector2.Distance(characterposition, cameraPosition) < range;
        //GameManager.instance.markerManager.Show(selectable);
    }

    public Vector3Int GetTileBase(Vector2 mousePosition)
    {
        Vector3 worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
        Vector3Int gridPosition = tileMarker.WorldToCell(worldPosition);
        TileBase tile = tileMarker.GetTile(gridPosition);
        //Debug.Log("Tile in position:" + gridPosition + "is" + tile);
        return gridPosition;
    }
    //private void Marker()
    //{
    //    //Vector3Int gridPosition = GetTileBase(Input.mousePosition);
    //    GameManager.instance.markerManager.markedCellPosition = selectedTilePosition;
    //}
    public void ChangePositionPlayer(Vector2 nextPosition)
    {
        changeSceneAnimator.SetTrigger("FadeOut");
        this.transform.position = new Vector2(10, -2);
        changeSceneAnimator.SetTrigger("FadeIn");
    }
}
    