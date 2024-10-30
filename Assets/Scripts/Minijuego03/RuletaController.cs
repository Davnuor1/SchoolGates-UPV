using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class RuletaController : MonoBehaviour
{
    [SerializeField] private MinijuegoEspejos03Localization localizacion;
    public Button[] botones;
    public PreguntaUIController preguntaUIController;

    public bool esSegundaParte = false;
    public bool esRuletaIzquierda = false;

    void Start()
    {
        for (int i = 0; i < botones.Length; i++)
        {
            int puntuacion = i + 1;
            botones[i].onClick.RemoveAllListeners();
            botones[i].onClick.AddListener(() => SeleccionarPuntuacion(puntuacion));
        }
    }

    void SeleccionarPuntuacion(int puntuacion)
    {
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
