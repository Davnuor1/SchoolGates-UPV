using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public ItemManager itemManager;
    public TileManager tileManager;
    public UI_Manager uiManager;
    public ToolManager toolManager;
    public MarkerManager markerManager;
    public DayTimeController dayTimeController;
    public ItemSpawnManager itemSpawnManager;
    public CurrencyManager currencyManager;

    public Player player;
    private void Awake()
    {
        if(instance!=null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
        itemManager = GetComponent<ItemManager>();
        tileManager = GetComponent<TileManager>();
        uiManager = GetComponent<UI_Manager>();
        toolManager = GetComponent<ToolManager>();
        markerManager = GetComponent<MarkerManager>();
        dayTimeController = GetComponent<DayTimeController>();
        itemSpawnManager = GetComponent<ItemSpawnManager>();
        currencyManager = GetComponent<CurrencyManager>();

        player = FindObjectOfType<Player>();
    }
}
