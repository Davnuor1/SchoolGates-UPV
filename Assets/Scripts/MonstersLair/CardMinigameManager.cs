using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardMinigameManager : MonoBehaviour
{
    [Header("Localizaciones")]
    [SerializeField] private CardMinigameLocalization locES;
    [SerializeField] private CardMinigameLocalization locEN;
    [SerializeField] private CardMinigameLocalization locDE;
    [SerializeField] private CardMinigameLocalization locIT;
    [SerializeField] private CardMinigameLocalization locFI;

    private CardMinigameLocalization loc;

    [Header("Panel raíz del minijuego")]
    [SerializeField] private GameObject panelRoot;  // Canvas/Panel del minijuego
    [SerializeField] private GameObject canvas;

    [Header("UI - Instrucciones y resultado")]
    [SerializeField] private TextMeshProUGUI txtInstruction;
    [SerializeField] private TextMeshProUGUI txtResult;

    [Header("UI - Botones")]
    [SerializeField] private Button btnConfirm;
    [SerializeField] private TextMeshProUGUI txtConfirm;
    [SerializeField] private Button btnReset;              //  antes btnCancel
    [SerializeField] private TextMeshProUGUI txtReset;     //  antes txtCancel


    [Header("Cartas (toggles)")]
    [SerializeField] private Toggle tYourself;
    [SerializeField] private Toggle tMonster;
    [SerializeField] private Toggle tPeople;
    [SerializeField] private Toggle tLove;

    [Header("Labels bajo cartas")]
    [SerializeField] private TextMeshProUGUI lblYourself;
    [SerializeField] private TextMeshProUGUI lblMonster;
    [SerializeField] private TextMeshProUGUI lblPeople;
    [SerializeField] private TextMeshProUGUI lblLove;

    // bits: Y=1, M=2, P=4, L=8
    private const int BIT_Y = 1;
    private const int BIT_M = 2;
    private const int BIT_P = 4;
    private const int BIT_L = 8;

    private string currentFinalId = null;

    private void Awake()
    {
        if (btnConfirm != null) btnConfirm.onClick.AddListener(OnClickConfirm);
        if (btnReset != null) btnReset.onClick.AddListener(OnClickReset);

        // Escuchar cambios en toggles
        if (tYourself != null) tYourself.onValueChanged.AddListener(_ => RefreshCombination());
        if (tMonster != null) tMonster.onValueChanged.AddListener(_ => RefreshCombination());
        if (tPeople != null) tPeople.onValueChanged.AddListener(_ => RefreshCombination());
        if (tLove != null) tLove.onValueChanged.AddListener(_ => RefreshCombination());
    }

    private void OnDestroy()
    {
        if (btnConfirm != null) btnConfirm.onClick.RemoveListener(OnClickConfirm);
        if (btnReset != null) btnReset.onClick.RemoveListener(OnClickReset);

        if (tYourself != null) tYourself.onValueChanged.RemoveAllListeners();
        if (tMonster != null) tMonster.onValueChanged.RemoveAllListeners();
        if (tPeople != null) tPeople.onValueChanged.RemoveAllListeners();
        if (tLove != null) tLove.onValueChanged.RemoveAllListeners();
    }

    private void OnEnable()
    {
        DefineLanguage();
        ApplyLocalization();
        ResetUI();
        RefreshCombination();
    }

    private void DefineLanguage()
    {
        string code = (LocalizationManager.Instance != null)
            ? LocalizationManager.Instance.CurrentLanguage
            : "es";

        if (code == "en" && locEN != null) loc = locEN;
        else if (code == "de" && locDE != null) loc = locDE;
        else if (code == "it" && locIT != null) loc = locIT;
        else if (code == "fi" && locFI != null) loc = locFI;
        else loc = locES;
    }

    private void ApplyLocalization()
    {
        if (loc == null) return;

        if (txtInstruction != null) txtInstruction.text = loc.instructionText;
        if (txtConfirm != null) txtConfirm.text = loc.btnConfirmLabel;
        if (txtReset != null && btnReset != null) txtReset.text = loc.btnCancelLabel;

        if (lblYourself != null) lblYourself.text = loc.nameYourself;
        if (lblMonster != null) lblMonster.text = loc.nameMonster;
        if (lblPeople != null) lblPeople.text = loc.namePeople;
        if (lblLove != null) lblLove.text = loc.nameLove;
    }

    private void ResetUI()
    {
        Debug.Log("ResetUI iniciado");
        ResetSelectionOnly();
        
    }

    // Calcula el bitmask y determina final
    private void RefreshCombination()
    {
        Debug.Log("Refresh iniciado");
        int mask = 0;
        if (tYourself != null && tYourself.isOn) mask |= BIT_Y;
        if (tMonster != null && tMonster.isOn) mask |= BIT_M;
        if (tPeople != null && tPeople.isOn) mask |= BIT_P;
        if (tLove != null && tLove.isOn) mask |= BIT_L;

        string finalId = null;
        string resultText = "";

        // PRIORIDAD:
        // 1) Final 5: TODAS las cartas (Y + M + P + L)
        if (mask == (BIT_Y | BIT_M | BIT_P | BIT_L))
        {
            finalId = "Final5";
            resultText = loc != null ? loc.final5Text : "Final 5";
        }
        // 2) Final 3: Love + Yourself + People (sin Monster)
        else if ((mask & (BIT_L | BIT_Y | BIT_P)) == (BIT_L | BIT_Y | BIT_P) && (mask & BIT_M) == 0)
        {
            finalId = "Final3";
            resultText = loc != null ? loc.final3Text : "Final 3";
        }
        // 3) Final 4: Love + People + Monster (sin Yourself)
        else if ((mask & (BIT_L | BIT_P | BIT_M)) == (BIT_L | BIT_P | BIT_M) && (mask & BIT_Y) == 0)
        {
            finalId = "Final4";
            resultText = loc != null ? loc.final4Text : "Final 4";
        }
        // 4) Final 1: Monster solo o Monster + Love (y nada más)
        else if (mask == BIT_M || mask == (BIT_M | BIT_L))
        {
            finalId = "Final1";
            resultText = loc != null ? loc.final1Text : "Final 1";
        }
        // 5) Final 2: People solo o People + Love (y nada más)
        else if (mask == BIT_P || mask == (BIT_P | BIT_L))
        {
            finalId = "Final2";
            resultText = loc != null ? loc.final2Text : "Final 2";
        }

        currentFinalId = finalId;

        if (txtResult != null) txtResult.text = resultText;
        if (btnConfirm != null) btnConfirm.interactable = !string.IsNullOrEmpty(currentFinalId);
    }


    private void OnClickConfirm()
    {
        if (string.IsNullOrEmpty(currentFinalId)) return;

        // Registrar final en UserData (para tu cinematica después)
        if (UserDataManager.Instance != null)
        {
            UserDataManager.Instance.RegisterFinalChosen(currentFinalId);
            UserDataManager.Instance.SaveAndUpdateTime(); // opcional
        }

        // Si quieres dejar guardado con PixelCrushers:
        // PixelCrushers.SaveSystem.SaveToSlot(1);

        CloseMinigame();
    }

    private void OnClickReset()
    {
        ResetSelectionOnly();
    }

    private void ResetSelectionOnly()
    {
        if (tYourself != null) tYourself.isOn = false;
        if (tMonster != null) tMonster.isOn = false;
        if (tPeople != null) tPeople.isOn = false;
        if (tLove != null) tLove.isOn = false;

        currentFinalId = null;
        if (txtResult != null) txtResult.text = "";
        if (btnConfirm != null) btnConfirm.interactable = false;
    }

    public void OpenMinigame()
    {
        if (panelRoot != null) panelRoot.SetActive(true);
        if (canvas != null) canvas.SetActive(true);
        Debug.Log("CardMinigame iniciado, dentro del manager");
        // bloquear player si procede
        //TryBlockPlayer(true);

        // reset y aplicar loc
        DefineLanguage();
        ApplyLocalization();
        ResetUI();
        RefreshCombination();
    }

    public void CloseMiniggameExternal() // por si quieres cerrarlo desde fuera
    {
        CloseMinigame();
    }

    private void CloseMinigame()
    {
        if (panelRoot != null) panelRoot.SetActive(false);
        //TryBlockPlayer(false);
        canvas.SetActive(false);

    }

    private void TryBlockPlayer(bool block)
    {
        // Si tienes acceso al PlayerMovement, lo puedes habilitar/deshabilitar aquí
        // Ejemplo (ajústalo a tu proyecto):
        if (GameManager.instance != null && GameManager.instance.player != null)
        {
            var pm = GameManager.instance.player.GetComponent<PlayerMovement>();
            if (pm != null) pm.enabled = !block;

            if (block && DeviceDetector.isTouchDevice && GameManager.instance.tabletUI != null)
                GameManager.instance.tabletUI.SetActive(false);
            if (!block && DeviceDetector.isTouchDevice && GameManager.instance.tabletUI != null)
                GameManager.instance.tabletUI.SetActive(true);
        }
    }
}
