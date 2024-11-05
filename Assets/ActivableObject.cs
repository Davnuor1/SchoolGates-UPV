using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivableObject : MonoBehaviour
{
    public static bool Activar=true;

    public void Start()
    {
        if (Activar)
        {
            gameObject.SetActive(true);
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
    public void ToggleActivar()
    {
        Activar = !Activar;
    }
    public void Activate()
    {
        gameObject.SetActive(true);
    }
}
