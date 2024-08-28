using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RuletaController : MonoBehaviour
{
    public Button[] botones; // Botones numerados de la ruleta
    public PreguntaUIController preguntaUIController;

    public bool esSegundaParte = false;
    public bool esRuletaIzquierda = false;

    void Start()
    {
        Debug.Log("RuletaController Start ejecutado en " + gameObject.name);
        for (int i = 0; i < botones.Length; i++)
        {
            int puntuacion = i + 1;

            // Eliminar cualquier listener anterior para evitar duplicaciones
            botones[i].onClick.RemoveAllListeners();

            // Agregar un solo listener
            botones[i].onClick.AddListener(() => SeleccionarPuntuacion(puntuacion));
        }
    }

    void SeleccionarPuntuacion(int puntuacion)
    {
        Debug.Log($"Puntuación seleccionada: {puntuacion} en {(esSegundaParte ? (esRuletaIzquierda ? "ruleta izquierda" : "ruleta derecha") : "ruleta única")}");
        if (esSegundaParte)
        {
            if (esRuletaIzquierda)
            {
                preguntaUIController.RecibirPuntuacionSegundaParte(puntuacion, -1);
            }
            else
            {
                preguntaUIController.RecibirPuntuacionSegundaParte(-1, puntuacion);
            }
        }
        else
        {
            preguntaUIController.RecibirPuntuacion(puntuacion);
        }
    }
}
