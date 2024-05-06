using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TimeAgent))]
public class ItemSpawner : MonoBehaviour
{
    [SerializeField] ItemData toSpawn;
    [SerializeField] int count;
    //[SerializeField] float spread = 2f;
    [SerializeField] float probability;
    [SerializeField] GameObject spawner;

    private void Start()
    {
        TimeAgent timeAgent = GetComponent<TimeAgent>();
        timeAgent.onTimeTick += Spawn;
    }
    void Spawn()
    {
        if (UnityEngine.Random.value<probability)

        {
            Vector3 position = spawner.transform.position;

            //position.x = spread * UnityEngine.Random.value - spread / 2;
            //position.y = spread * UnityEngine.Random.value - spread / 2;
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
                spawnOffset.y = (float)(spawnOffset.x - 0.3);
            }
            if (spawnOffset.y >= 0)
            {
                spawnOffset.y = (float)(spawnOffset.x + 0.3);
            }
            Vector3 spawnOffset3 = spawnOffset;
            Debug.Log(position);

            GameManager.instance.itemSpawnManager.SpawnItem((position + spawnOffset3), toSpawn, count); 
        }
    }
}
