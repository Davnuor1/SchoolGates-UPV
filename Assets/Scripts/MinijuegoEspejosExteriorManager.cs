using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class MinijuegoEspejosExteriorManager : MonoBehaviour
{
    [Header("Localization")]
    [SerializeField] private MinijuegoEspejosExteriorLocalization localizacion;

    [Header("UI References")]
    [SerializeField] private GameObject canvasMinijuego;  // Canvas del minijuego
    [SerializeField] private TMP_Text topMessageText;     // Texto superior (recuadro de mensaje)
    [SerializeField] private Image mirrorImage;           // Imagen del espejo (opcional)
    [SerializeField] private TMP_Text mirrorText;         // Texto que aparece dentro del espejo
    [SerializeField] private Image demonImage;            // Imagen del minidemonio

    [Header("Phase 1 Buttons")]
    [SerializeField] private GameObject panelPhase1Buttons; // Panel que contiene los botones "Repeat" y "Respond"
    [SerializeField] private Button buttonRepeat;
    [SerializeField] private Button buttonRespond;

    [Header("Phase 2: Response Options")]
    [SerializeField] private GameObject panelResponseOptions; // Panel que muestra las 3 opciones
    [SerializeField] private List<Button> responseButtons;    // Lista de 3 botones para respuestas

    [Header("Typewriter Effect")]
    [SerializeField] private TypewriterEffectDavid typewriterEffect;

    private int currentMirrorIndex = 0;
    private int responseRound = 0; // 0 = aún no se ha seleccionado respuesta, 1 = primera respuesta seleccionada, 2 = segunda respuesta completada
    private int firstResponseSelected = -1; // índice de la respuesta elegida en la primera ronda
    private PlayerMovement playerMovement;
    private Animator playerAnimator;
    private bool estaActivo = false;
    public GameObject espejoParaQuitar;

    private void Start()
    {
        // Asignar los textos de los botones de fase 1 según la localización
        buttonRepeat.GetComponentInChildren<TMP_Text>().text = localizacion.buttonRepeatText;
        buttonRespond.GetComponentInChildren<TMP_Text>().text = localizacion.buttonRespondText;

        // Ocultamos inicialmente el canvas del minijuego
        //canvasMinijuego.SetActive(false);
        panelResponseOptions.SetActive(false);
    }

    // Se llama cuando el jugador entra en contacto con el objeto trigger
    public void StartMinijuego()
    {
        playerMovement = GameManager.instance.player.GetComponent<PlayerMovement>();
        playerAnimator = GameManager.instance.player.GetComponent<Animator>();
        playerAnimator.SetBool("moving", false);
        playerMovement.enabled = false;
        GameManager.instance.uiManager.canToggle = false;
        Debug.Log("Aqui activamos el canvas");
        canvasMinijuego.SetActive(true);
        estaActivo = canvasMinijuego.activeSelf;
        Debug.Log(estaActivo);

        
        currentMirrorIndex = 0;
        ShowCurrentMirror();
    }

    private void ShowCurrentMirror()
    {
        // Configurar el mensaje superior al valor inicial
        topMessageText.text = localizacion.topMessageInitial;

        // Obtener los datos del espejo actual
        MinijuegoEspejosExteriorLocalization.MirrorData currentMirror = localizacion.espejos[currentMirrorIndex];

        // (Opcional) Puedes actualizar la imagen del espejo o el título si fuera necesario

        // Reproducir el efecto typewriter en el texto del espejo
        typewriterEffect.Play(currentMirror.mirrorText, mirrorText);

        // Mostrar los botones de la fase 1 y ocultar el panel de respuestas
        panelPhase1Buttons.SetActive(true);
        panelResponseOptions.SetActive(false);

        // Reiniciar variables de respuesta
        responseRound = 0;
        firstResponseSelected = -1;
    }

    public void OnRepeatButton()
    {
        // Repetir la animación del texto del espejo
        MinijuegoEspejosExteriorLocalization.MirrorData currentMirror = localizacion.espejos[currentMirrorIndex];
        typewriterEffect.Play(currentMirror.mirrorText, mirrorText);
    }

    public void OnRespondButton()
    {
        // Ocultar los botones de fase 1 y mostrar el panel de respuestas
        panelPhase1Buttons.SetActive(false);

        // Cambiar el mensaje superior para incluir el título del espejo
        MinijuegoEspejosExteriorLocalization.MirrorData currentMirror = localizacion.espejos[currentMirrorIndex];
        topMessageText.text = string.Format(localizacion.topMessageRespondFormat, currentMirror.mirrorTitle);

        // Mostrar el panel de respuestas y configurar los botones
        panelResponseOptions.SetActive(true);
        PopulateResponseButtons();
    }

    private void PopulateResponseButtons()
    {
        MinijuegoEspejosExteriorLocalization.MirrorData currentMirror = localizacion.espejos[currentMirrorIndex];
        for (int i = 0; i < responseButtons.Count; i++)
        {
            TMP_Text btnText = responseButtons[i].GetComponentInChildren<TMP_Text>();
            btnText.text = currentMirror.responses[i];
            responseButtons[i].interactable = true;  // Asegurarse de que estén activos
        }
    }

    // Método llamado por cada botón de respuesta. El parámetro responseIndex indica qué botón fue pulsado (0, 1 o 2)
    public void OnResponseSelected(int responseIndex)
    {
        if (responseRound == 0)
        {
            // Primera respuesta seleccionada
            firstResponseSelected = responseIndex;
            responseRound = 1;
            // Actualizar el mensaje superior para la segunda ronda
            topMessageText.text = localizacion.topMessageSecondRound;
            // Desactivar el botón elegido para que no se pueda seleccionar de nuevo
            responseButtons[responseIndex].interactable = false;
        }
        else if (responseRound == 1)
        {
            if (responseIndex == firstResponseSelected)
            {
                Debug.Log("You cannot select the same answer. Choose a different option.");
                return;
            }
            else
            {
                responseRound = 2;
                // Se completa la respuesta del espejo; pasar al siguiente espejo
                NextMirror();
            }
        }
    }

    private void NextMirror()
    {
        currentMirrorIndex++;
        if (currentMirrorIndex >= localizacion.espejos.Count)
        {
            EndMinijuego();
        }
        else
        {
            ShowCurrentMirror();
        }
    }

    private void EndMinijuego()
    {
        // Finalizar el minijuego, ocultar el canvas, reiniciar o transicionar a otra escena
        playerMovement = GameManager.instance.player.GetComponent<PlayerMovement>();
        playerMovement.enabled = true;
        GameManager.instance.uiManager.canToggle = true;

        canvasMinijuego.SetActive(false);
        espejoParaQuitar.SetActive(false);
        // Aquí podrías reactivar el control del jugador, etc.
    }
}
