using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class WiseManPracticesGameManager : MonoBehaviour
{
    public WiseManPracticesLocalization localizationData;

    [Header("UI Panels")]
    public GameObject panelVignettes;
    public GameObject panelDialogue;
    public GameObject panelOptions;

    [Header("Conflict Help Panels")]
    public GameObject panelConflicts1;
    public GameObject panelConflicts3;

    [Header("UI Elements")]
    public Image vignetteBackground;
    public TMP_Text vignetteText;
    public Button buttonNextVignette;

    public TMP_Text dialogueText;
    public Image characterPortrait;
    public Button buttonNextDialogue;

    public Button[] optionButtons;
    public TMP_Text[] optionTexts;
    public TMP_Text feedbackText;
    public Button buttonHelpConflict1;
    public Button buttonHelpConflict3;
    public Button buttonCloseConflicts1;
    public Button buttonCloseConflicts3;

    private int currentVignetteIndex = 0;
    private int currentConflictIndex = 0;
    private int currentDialogueIndex = 0;
    private HashSet<int> selectedResponses = new HashSet<int>(); // Para el Conflicto 2

    private void Start()
    {
        // Inicializar UI
        panelConflicts1.SetActive(false);
        panelConflicts3.SetActive(false);
        buttonHelpConflict1.gameObject.SetActive(false);
        buttonHelpConflict3.gameObject.SetActive(false);

        // Configurar botones de ayuda
        buttonHelpConflict1.onClick.AddListener(() => OpenConflictPanel(1));
        buttonHelpConflict3.onClick.AddListener(() => OpenConflictPanel(3));

        buttonCloseConflicts1.onClick.AddListener(() => CloseConflictPanel(1));
        buttonCloseConflicts3.onClick.AddListener(() => CloseConflictPanel(3));

        //ShowNextVignette();
    }

    private void Update()
    {
        // Activar el botón de ayuda solo en los conflictos correspondientes
        buttonHelpConflict1.gameObject.SetActive(currentConflictIndex == 1);
        buttonHelpConflict3.gameObject.SetActive(currentConflictIndex == 3);
    }

    public void ShowNextVignette()
    {
        List<WiseManPracticesLocalization.Vignette> relevantVignettes = localizationData.vignettes
        .FindAll(v => v.placement == GetCurrentVignettePlacement());

        if (currentVignetteIndex < relevantVignettes.Count)
        {
            var vignette = relevantVignettes[currentVignetteIndex];

            panelVignettes.SetActive(true);
            panelDialogue.SetActive(false);
            panelOptions.SetActive(false);

            vignetteBackground.sprite = vignette.backgroundImage;
            vignetteText.text = vignette.text;

            buttonNextVignette.onClick.RemoveAllListeners();
            buttonNextVignette.onClick.AddListener(() =>
            {
                currentVignetteIndex++;
                ShowNextVignette();
            });
        }
        else
        {
            Debug.Log("Todas las viñetas de este segmento han sido mostradas.");
            panelVignettes.SetActive(false);
            currentVignetteIndex = 0;

            if (currentConflictIndex == 2) //  Asegurar inicio correcto del Conflicto 3
            {
                Debug.Log("Iniciando el Conflicto 3 tras las viñetas.");
                selectedResponses.Clear(); //  Limpiar respuestas anteriores
                ShowDialogue();
            }
            else
            {
                StartDialogue();
            }
        }
    }




    private WiseManPracticesLocalization.VignettePlacement GetCurrentVignettePlacement()
    {
        switch (currentConflictIndex)
        {
            case 0: return WiseManPracticesLocalization.VignettePlacement.BeforeConflict1;
            case 1: return WiseManPracticesLocalization.VignettePlacement.AfterConflict1;
            case 2: return WiseManPracticesLocalization.VignettePlacement.BeforeConflict2;
            case 3: return WiseManPracticesLocalization.VignettePlacement.AfterConflict2;
            case 4: return WiseManPracticesLocalization.VignettePlacement.BeforeConflict3;
            case 5: return WiseManPracticesLocalization.VignettePlacement.AfterConflict3;
            default: return WiseManPracticesLocalization.VignettePlacement.Conclusion;
        }
    }

    public void StartDialogue()
    {
        panelVignettes.SetActive(false);
        panelDialogue.SetActive(true);
        selectedResponses.Clear(); // Reiniciar respuestas vistas en conflictos como el 2
        ShowDialogue();
    }

    private void ShowDialogue()
    {
        if (currentDialogueIndex >= localizationData.conflicts[currentConflictIndex].dialogues.Count)
        {
            EndConflict(); // Si no hay más diálogos en el conflicto, finalizarlo correctamente
            return;
        }

        var dialogue = localizationData.conflicts[currentConflictIndex].dialogues[currentDialogueIndex];

        dialogueText.text = dialogue.text;
        characterPortrait.sprite = dialogue.portrait;
        buttonNextDialogue.gameObject.SetActive(false);
        panelOptions.SetActive(false); // Asegurar que las opciones NO se muestren si no son necesarias

        if (dialogue.requiresResponse && dialogue.responseOptions.Count > 0)
        {
            ShowOptions(dialogue);
        }
        else
        {
            buttonNextDialogue.onClick.RemoveAllListeners();
            buttonNextDialogue.onClick.AddListener(() =>
            {
                currentDialogueIndex++;

                if (currentDialogueIndex < localizationData.conflicts[currentConflictIndex].dialogues.Count)
                {
                    ShowDialogue();
                }
                else
                {
                    EndConflict();
                }
            });

            buttonNextDialogue.gameObject.SetActive(true);
        }
    }

    private void ShowOptions(WiseManPracticesLocalization.Dialogue dialogue)
    {
        panelOptions.SetActive(true);

        for (int i = 0; i < optionButtons.Length; i++)
        {
            if (i < dialogue.responseOptions.Count)
            {
                optionTexts[i].text = dialogue.responseOptions[i].responseText;
                optionButtons[i].gameObject.SetActive(true);
                optionButtons[i].interactable = !selectedResponses.Contains(i); // Habilitar solo si no ha sido seleccionado antes

                int index = i;
                optionButtons[i].onClick.RemoveAllListeners();
                optionButtons[i].onClick.AddListener(() => SelectOption(index));
            }
            else
            {
                optionButtons[i].gameObject.SetActive(false);
            }
        }
    }


    public void SelectOption(int index)
    {
        var dialogue = localizationData.conflicts[currentConflictIndex].dialogues[currentDialogueIndex];

        // Asignar el feedback al texto del diálogo
        dialogueText.text = dialogue.responseOptions[index].responseFeedback;

        panelOptions.SetActive(false);
        panelDialogue.SetActive(true);

        selectedResponses.Add(index); // Registrar la opción seleccionada
        optionButtons[index].interactable = false; // Bloquear opción ya seleccionada

        // Conflicto 2: forzar a seleccionar todas las respuestas antes de avanzar
        if (localizationData.conflicts[currentConflictIndex].requiresAllFeedback)
        {
            if (selectedResponses.Count < dialogue.responseOptions.Count)
            {
                panelOptions.SetActive(true); // Seguir mostrando opciones hasta que se seleccionen todas
            }
            else
            {
                buttonNextDialogue.gameObject.SetActive(true); // Permitir avanzar al siguiente diálogo
            }
        }
        // Conflicto 3: bifurcación a Conflicto 4 o Conflicto 5
        else if (localizationData.conflicts[currentConflictIndex].isBranchingConflict && currentDialogueIndex == 0)
        {
            if (index == 0)
            {
                Debug.Log("El jugador eligió la primera opción, avanzando a Conflicto 4.");
                currentConflictIndex = 3; // Ir a Conflicto 4
            }
            else
            {
                Debug.Log("El jugador eligió la segunda opción, avanzando a Conflicto 5.");
                currentConflictIndex = 4; // Ir a Conflicto 5
            }

            currentDialogueIndex = 0;
            ShowDialogue();
        }
        else
        {
            buttonNextDialogue.gameObject.SetActive(true);
        }
    }



    private void EndConflict()
    {
        Debug.Log("Finalizando conflicto: " + currentConflictIndex);

        panelOptions.SetActive(false);
        panelDialogue.SetActive(false);

        if (currentConflictIndex == 1) //  Mostrar viñetas después del Conflicto 2
        {
            Debug.Log("Mostrando viñetas después del Conflicto 2.");
            currentConflictIndex++; //  Avanzar al Conflicto 3 después de las viñetas
            currentVignetteIndex = 0; //  Reiniciar índice de viñetas
            ShowNextVignette();
            return;
        }
        else if (currentConflictIndex == 2) // Verificar que el Conflicto 3 inicia correctamente
        {
            Debug.Log("Iniciando el Conflicto 3 después de las viñetas.");
            currentDialogueIndex = 0;
            selectedResponses.Clear(); //  Limpiar respuestas seleccionadas del Conflicto 2
            ShowDialogue();
            return;
        }
        else if (currentConflictIndex == 3 || currentConflictIndex == 4)
        {
            StartFinalDialogue(); // Evita mostrar ambos conflictos 4 y 5
        }
        else if (currentConflictIndex < localizationData.conflicts.Count - 1)
        {
            currentConflictIndex++;
            currentDialogueIndex = 0;
            ShowNextVignette();
        }
        else
        {
            StartFinalDialogue();
        }
    }










    private void StartFinalDialogue()
    {
        Debug.Log("Minijuego Finalizado.");
        panelDialogue.SetActive(false);
    }

    public void OpenConflictPanel(int conflictNumber)
    {
        if (conflictNumber == 1)
        {
            panelConflicts1.SetActive(true);
        }
        else if (conflictNumber == 3)
        {
            panelConflicts3.SetActive(true);
        }
    }

    public void CloseConflictPanel(int conflictNumber)
    {
        if (conflictNumber == 1)
        {
            panelConflicts1.SetActive(false);
        }
        else if (conflictNumber == 3)
        {
            panelConflicts3.SetActive(false);
        }
    }

    public void StartConflict(int conflictNumber)
    {
        currentConflictIndex = conflictNumber;
    }
}
