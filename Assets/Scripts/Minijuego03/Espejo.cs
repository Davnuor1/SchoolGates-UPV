using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Espejo : MonoBehaviour
{
    [SerializeField] private MinijuegoEspejos03Localization localizacion;
    [SerializeField] private int idEspejo;
    [SerializeField] private TextMeshProUGUI textoEspejo;
    [SerializeField] private List<TextMeshProUGUI> textoRespuestas;

    private void Start()
    {
        // Cargar nombre y respuestas del espejo desde el ScriptableObject
        textoEspejo.text = localizacion.espejos[idEspejo].nombre;
        for (int i = 0; i < textoRespuestas.Count; i++)
        {
            textoRespuestas[i].text = localizacion.espejos[idEspejo].respuestas[i];
        }
    }
}
