using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


public class CropTile
{
    public int growTimer;
    public int growStage;
    public Crop crop;
    public SpriteRenderer renderer;
    public Transform transform;
    public bool Complete
    {
        get
        {
            if (crop == null) { return false; }
            return growTimer >= crop.timeToGrow;
        }
    }

    internal void Harvested()
    {
        growTimer = 0;
        growStage = 0;
        crop = null;
        renderer.gameObject.SetActive(false);
    }
}
public class TileManager : TimeAgent
{
    [SerializeField] private Tilemap interactableMap;
    [SerializeField] private Tile hiddenInteractableTile;
    [SerializeField] private Tile plowed;
    [SerializeField] private Tile seeded;
    [SerializeField] private Tilemap cropsMap;
    [SerializeField] private GameObject cropsSpritePrebab;
    //private int numAux = 0;


    Dictionary<Vector2Int, CropTile> crops;
    Dictionary<CropTile, Vector3Int> cropsInv;

    // Start is called before the first frame update
    void Start()
    {
        crops = new Dictionary<Vector2Int, CropTile>();
        cropsInv = new Dictionary<CropTile, Vector3Int>();
        onTimeTick += Tick;
        
        Init();
        foreach(var position in interactableMap.cellBounds.allPositionsWithin)
        {
            TileBase tile = interactableMap.GetTile(position);
            if(tile!=null && tile.name == "interactable_visible")
            {
                interactableMap.SetTile(position, hiddenInteractableTile);
            }
            
        }
    }
    public void Tick()
    {
        foreach(CropTile cropTile in crops.Values)
        {
            if (cropTile.crop == null) { continue; }
            if (cropTile.Complete)
            {
                Debug.Log("Im done growing");
                continue;
            }
            cropTile.growTimer += 1;
           
            if (cropTile.growTimer >= cropTile.crop.growthStageTime[cropTile.growStage])
            {
                
                if (cropTile.growStage == 0) { cropsMap.SetTile(cropsInv[cropTile], null); }
                
                cropTile.renderer.gameObject.SetActive(true);
                cropTile.renderer.sprite = cropTile.crop.sprites[cropTile.growStage];
                cropTile.growStage += 1;
                Debug.Log("Pasando a fase:" + cropTile.growStage);
            }
            
        }
    }
    public bool IsInteractable(Vector3Int position)
    {
        TileBase tile = interactableMap.GetTile(position);
        if(tile != null)
        {
            if (tile.name == "interactable" || tile.name=="Summer_Plowed")
            {
                return true;
            }
        }
        return false;
    }
    public void PlowTile(Vector3Int position,Vector2Int toolDirection)
    {
        if (crops.ContainsKey((Vector2Int)position))
        {
            return;
        }
        CropTile crop = new CropTile();
        crops.Add((Vector2Int)position, crop);
        cropsInv.Add(crop,position);
        GameManager.instance.toolManager.AnimateHoe(toolDirection);
        GameObject go = Instantiate(cropsSpritePrebab);
        go.transform.position = cropsMap.CellToWorld(position);
        go.SetActive(false);
        crop.renderer = go.GetComponent<SpriteRenderer>();

        interactableMap.SetTile(position, plowed);
    }
    public void SeedTile(Vector3Int position, Crop toSeed) 
    {
        
        cropsMap.SetTile(position, seeded);
        //Debug.Log("PonemosSeedTILEMANAGER");
        crops[(Vector2Int)position].crop = toSeed;
        
    }
   public bool Check(Vector3Int position)
    {
        return crops.ContainsKey((Vector2Int)position);
    }
    internal void PickUp(Vector3Int tilemapPosition)
    {
        Vector2Int position = (Vector2Int)tilemapPosition;
        if (crops.ContainsKey(position) == false) { return; }
        
        CropTile cropTile = crops[position];
        
        if (cropTile.Complete)
        {
            Debug.Log("croptile.crop.count:" + cropTile.crop.count);
            GameManager.instance.itemSpawnManager.SpawnItem(tilemapPosition, cropTile.crop.yield, cropTile.crop.count);
            cropsMap.SetTile(tilemapPosition, null);
            cropTile.Harvested();
        }
    }
}
