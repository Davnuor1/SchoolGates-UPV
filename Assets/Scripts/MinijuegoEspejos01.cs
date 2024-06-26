using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MinijuegoEspejos01 : MonoBehaviour
{
    [SerializeField] public GameObject botonLike;
    [SerializeField] public GameObject botonDislike;
    [SerializeField] public GameObject RuedaRating;
    [SerializeField] TextMeshProUGUI textoEspejos;
    List<string> espejos;
    public int idEspejo=0;
    // Start is called before the first frame update
    void Start()
    {
        espejos = new List<string>();
        nombresEspejos();
        textoEspejos.text = espejos[idEspejo];
    }

    
    public void darBotonLike()
    {
        botonLike.SetActive(false);
        botonDislike.SetActive(false);
        RuedaRating.SetActive(true);
    }

    public void darBotonDislike()
    {
        idEspejo += 1;
        //romper espejo
        textoEspejos.text = espejos[idEspejo];
    }
    public void nombresEspejos()
    {
        espejos.Add("Fearful/ anxious");
        espejos.Add("Optimistic");
        espejos.Add("Impulsive");
        espejos.Add("Open to experience");
        espejos.Add("Approval seeking");
        espejos.Add("Independent");
        espejos.Add("Underachiever");
        espejos.Add("Determined");
        espejos.Add("Insecure");
        espejos.Add("Resourceful");
        espejos.Add("Empathic");
        espejos.Add("Hostile");
        espejos.Add("Faithful");
        espejos.Add("Materialistic");

    }
    public void darRating()
    {
        //guardar puntuacion
        idEspejo += 1;
        botonLike.SetActive(true);
        botonDislike.SetActive(true);
        RuedaRating.SetActive(false);
        textoEspejos.text = espejos[idEspejo];
        
    }
}
