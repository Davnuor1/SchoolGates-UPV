using System.Collections.Generic;
using UnityEngine;

public class MinijuegoManager : MonoBehaviour
{
    
    private List<int> puntuacionesEspejos; // Lista para almacenar las puntuaciones de cada espejo

    public int indiceEspejoActual = 0;
    private bool enSegundaFase = false;
    private PlayerMovement playerMovement;
    public GameObject portalSalida;

    public PreguntaUIController preguntaUIController;
    [SerializeField] private MinijuegoEspejos03Localization localizacionES;
    [SerializeField] private MinijuegoEspejos03Localization localizacionIT;
    [SerializeField] private MinijuegoEspejos03Localization localizacionDE;
    [SerializeField] private MinijuegoEspejos03Localization localizacionEN;
    [SerializeField] private MinijuegoEspejos03Localization localizacionFI;
    public MinijuegoEspejos03Localization localizacion;
    private string codeLanguage;
    void Start()
    {
        defineLanguage();
        indiceEspejoActual = 0;
        enSegundaFase = false;

        // Inicializar la lista de puntuaciones de acuerdo al número de espejos en la localización
        puntuacionesEspejos = new List<int>(new int[localizacion.espejos.Count]);

        // Mostrar el primer espejo
        preguntaUIController.MostrarEspejo(indiceEspejoActual);
    }
    public void defineLanguage()
    {
        codeLanguage = LocalizationManager.Instance.CurrentLanguage;
        if (codeLanguage == "es") { localizacion = localizacionES; }
        else if (codeLanguage == "it") { localizacion = localizacionIT; }
        else if (codeLanguage == "de") { localizacion = localizacionDE; }
        else if (codeLanguage == "en") { localizacion = localizacionEN; }
        else if (codeLanguage == "fi") { localizacion = localizacionFI; }
    }
    public void SiguienteEspejo()
    {
        if (!enSegundaFase)
        {
            indiceEspejoActual++;

            if (indiceEspejoActual < localizacion.espejos.Count)
            {
                preguntaUIController.MostrarEspejo(indiceEspejoActual);
            }
            else
            {
                PrepararSegundaFase();
            }
        }
        else
        {
            indiceEspejoActual++;

            if (indiceEspejoActual < espejosSegundaFase.Count)
            {
                Debug.Log($"Mostrando espejo en la segunda fase: {localizacion.espejos[espejosSegundaFase[indiceEspejoActual]].nombre}");
                preguntaUIController.MostrarSegundaParte(localizacion.espejos[espejosSegundaFase[indiceEspejoActual]]);
            }
            else
            {
                TerminarMinijuego();
            }
        }
    }

    public List<int> espejosSegundaFase; // Hacer esta lista pública para acceder desde SeleccionMultipleController

    void PrepararSegundaFase()
    {
        espejosSegundaFase = new List<int>();

        for (int i = 0; i < puntuacionesEspejos.Count; i++)
        {
            Debug.Log($"Evaluando espejo {i} con puntuación: {puntuacionesEspejos[i]}");
            if (puntuacionesEspejos[i] > 0)
            {
                espejosSegundaFase.Add(i);
                Debug.Log($"Espejo {localizacion.espejos[i].nombre} añadido para la segunda fase, recuento actual: {espejosSegundaFase.Count}");
            }
        }

        if (espejosSegundaFase.Count == 0)
        {
            Debug.Log("No hay espejos con puntuación para la segunda fase. Terminando el minijuego.");
            TerminarMinijuego();
            return;
        }

        indiceEspejoActual = 0;
        enSegundaFase = true;

        Debug.Log($"Comenzando segunda fase con {espejosSegundaFase.Count} espejos.");
        preguntaUIController.MostrarSegundaParte(localizacion.espejos[espejosSegundaFase[indiceEspejoActual]]);
    }

    public void AsignarPuntuacionEspejoActual(int puntuacion)
    {
        Debug.Log($"Asignando puntuación {puntuacion} al índice {indiceEspejoActual}");
        puntuacionesEspejos[indiceEspejoActual] = puntuacion;
    }

    void TerminarMinijuego()
    {
        Debug.Log("Minijuego terminado.");
        this.gameObject.SetActive(false);
        playerMovement = GameManager.instance.player.GetComponent<PlayerMovement>();
        playerMovement.enabled = true;
        if (DeviceDetector.isTouchDevice && GameManager.instance.tabletUI != null)
        {
            GameManager.instance.tabletUI.SetActive(true);
        }
        //SkillTreeController.Instance.Unlock("8");
        
        portalSalida.SetActive(true);

        GameManager.instance.skillTreeController.Unlock("8");
        GameManager.instance.skillTreeController.Unlock("9");
        GameManager.instance.uiManager.ToggleSkillTreeUI();
    }
}
