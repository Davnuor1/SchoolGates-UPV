using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class EmotionsFrustrationManager : MonoBehaviour
{
    private GameObject player;

    [Header("Configuración del minijuego")]
    public float duration = 40f;
    public float interval = 4f;
    public Collider2D limitArea;

    [Header("Panel de introducción")]
    public GameObject panelFrustrationIntro;
    public TextMeshProUGUI textIntro;
    public Button buttonNext;
    //public EmotionsFrustrationLocalization localization;
    public GameObject canvasGateOfEmotions;
    public EmotionsFrustrationMeditation meditationManager;
    private float timer = 0f;
    private bool gameActive = false;
    private bool isPaused = false;

    [Header("Traducciones")]
    [SerializeField] private EmotionsFrustrationLocalization localizacionES;
    [SerializeField] private EmotionsFrustrationLocalization localizacionIT;
    [SerializeField] private EmotionsFrustrationLocalization localizacionDE;
    [SerializeField] private EmotionsFrustrationLocalization localizacionEN;
    [SerializeField] private EmotionsFrustrationLocalization localizacionFI;
    public EmotionsFrustrationLocalization localization;
    private string codeLanguage;
    private Coroutine minigameCoroutine;
    

    private FrustratedInputOverride inputOverride;

    public void Start()
    {
        defineLanguage();
    }
    public void defineLanguage()
    {
        codeLanguage = LocalizationManager.Instance.CurrentLanguage;
        if (codeLanguage == "es") { localization = localizacionES; }
        else if (codeLanguage == "it") { localization = localizacionIT; }
        else if (codeLanguage == "de") { localization = localizacionDE; }
        else if (codeLanguage == "en") { localization = localizacionEN; }
        else if (codeLanguage == "fi") { localization = localizacionFI; }
    }
    public void StartFrustrationMinigame()
    {
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Jugador no encontrado en la escena.");
            return;
        }
        if (gameActive) return;
        Debug.Log("minijuegoActivo");
        
        if (player == null)
        {
            Debug.LogError("Jugador no asignado.");
            return;
        }

        // Añadir el override si no existe
        inputOverride = player.GetComponent<FrustratedInputOverride>();
        if (inputOverride == null)
        {
            inputOverride = player.AddComponent<FrustratedInputOverride>();
        }

        limitArea.enabled = true;
        gameActive = true;
        timer = 0f;
        minigameCoroutine = StartCoroutine(ControlLoop());
        StartCoroutine(ShowIntroAfterDelay(5f));
    }

    private IEnumerator ControlLoop()
    {
        
        while (timer < duration)
        {
            if (!isPaused)
            {
                Debug.Log("Tiempo acumulado: " + timer);
                timer += interval;
                RandomizeControls();
            }
            yield return new WaitForSeconds(interval);
        }
        Debug.Log("Minijuego finalizado");
        EndMinigame();
    }

    private void RandomizeControls()
    {
        List<KeyCode> keys = new List<KeyCode> { KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D };
        Dictionary<string, KeyCode> map = new();

        List<string> directions = new List<string> { "up", "down", "left", "right" };
        for (int i = 0; i < directions.Count; i++)
        {
            int randomIndex = Random.Range(0, keys.Count);
            map[directions[i]] = keys[randomIndex];
            keys.RemoveAt(randomIndex);
        }

        inputOverride.SetControlMapping(map);
        foreach (var pair in map)
        {
            //Debug.Log(pair.Key + " -> " + pair.Value);
        }
    }

    private IEnumerator ShowIntroAfterDelay(float delay)
    {
        Debug.Log("Esperando " + delay + " segundos para mostrar panel...");
        yield return new WaitForSeconds(delay);
        Debug.Log("Mostrando panel ahora.");
        isPaused = true;
        canvasGateOfEmotions.SetActive(true);
        panelFrustrationIntro.SetActive(true);
        textIntro.text = localization.introText;
        buttonNext.GetComponentInChildren<TextMeshProUGUI>().text = localization.nextButtonText;

        buttonNext.onClick.RemoveAllListeners();
        buttonNext.onClick.AddListener(() =>
        {
            Debug.Log("Botón NEXT presionado");
            panelFrustrationIntro.SetActive(false);
            canvasGateOfEmotions.SetActive(false);
            isPaused = false;

        });
    }

    private void EndMinigame()
    {
        gameActive = false;
        limitArea.enabled = false;

        if (inputOverride != null)
        {
            Destroy(player.GetComponent<FrustratedInputOverride>());
        }

        Debug.Log("Minijuego EmotionsFrustration finalizado.");
        meditationManager.StartMeditation();
    }
}
