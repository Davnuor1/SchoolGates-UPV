using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemSpawnManager : MonoBehaviour
{
    //public static ItemSpawnManager instance;

    //private void Awake(){instance = this;}

    [SerializeField] GameObject pickUpitemPrefab;
    public void SpawnItem(Vector3 position,ItemData item, int count)
    {
        for(int i = 0; i < count; i++)
        {
            Vector2 spawnOffset = Random.insideUnitCircle;
            Vector3 spawnOffset3 = spawnOffset;
            
            //Instantiate(pickUpitemPrefab, (position+spawnOffset3), Quaternion.identity);
            //Instantiate(item, (position + spawnOffset3), Quaternion.identity);
            Instantiate(item.prefab, (position + spawnOffset3), Quaternion.identity);
        }
        //GameObject o=Instantiate(pickUpitemPrefab, position, Quaternion.identity);
        //ItemData oa = Instantiate((item, position, Quaternion.identity);)
    }
}
