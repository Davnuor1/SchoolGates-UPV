using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeactivateMaligores : MonoBehaviour
{
    [SerializeField] public List<GameObject> maligores;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        foreach(GameObject maligor in maligores)
        {
            maligor.SetActive(false);
        }
    }
}
