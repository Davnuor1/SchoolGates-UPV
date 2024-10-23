using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DecreaseStatOnHit : MonoBehaviour
{
    [SerializeField] public string statName;
    [SerializeField] public double amount;
    // Start is called before the first frame update
    private void OnTriggerEnter2D(Collider2D collision)
    {
        GameManager.instance.statsManager.ModifyEnergy(amount);
    }
}
