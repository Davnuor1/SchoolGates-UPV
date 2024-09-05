using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableObject : MonoBehaviour
{
    public void Activate()
    {
        gameObject.SetActive(true);
    }
}
