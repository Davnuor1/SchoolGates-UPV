using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GatePeopleController : MonoBehaviour
{
    public static GatePeopleController Instance; // Singleton para acceder desde otros scripts
    public GameObject portal; // Referencia al GameObject del portal
    private int frutasDesactivadas = 0; // Contador de frutas desactivadas

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        //portal.SetActive(false); // Asegúrate de que el portal esté desactivado inicialmente
    }

    public void IncrementarFrutasDesactivadas()
    {
        frutasDesactivadas++;
        if (frutasDesactivadas >= 5)
        {
            portal.SetActive(true); // Activar el portal si se han desactivado 5 frutas
        }
    }
}
