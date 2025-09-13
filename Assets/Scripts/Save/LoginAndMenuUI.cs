using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class LoginAndMenuUI : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject panelLogin;
    [SerializeField] private GameObject panelMenu;

    [Header("Login UI")]
    [SerializeField] private TMP_InputField inputTan;
    [SerializeField] private TMP_InputField inputPassword;
    [SerializeField] private TextMeshProUGUI txtError;

    [Header("Menu UI")]
    [SerializeField] private Button btnNewGame;
    [SerializeField] private Button btnContinue;
    [SerializeField] private int newGameSceneBuildIndex = 0; // tu escena inicial

    private bool loggedIn = false;

    private void Awake()
    {
        if (UserDataManager.Instance == null)
        {
            Debug.LogError("Falta UserDataSystem en LoginScene con UserDataManager.");
        }
        if (panelLogin != null) panelLogin.SetActive(true);
        if (panelMenu != null) panelMenu.SetActive(false);
        if (txtError != null) txtError.text = "";
    }

    public void OnClick_Enter()
    {
        string tan = inputTan != null ? inputTan.text.Trim() : "";
        string pwd = inputPassword != null ? inputPassword.text.Trim() : "";

        if (string.IsNullOrEmpty(tan) || string.IsNullOrEmpty(pwd))
        {
            ShowError("Introduce TAN y contraseña.");
            return;
        }

        // Comprobar si existe UserData previo
        var existing = LocalJsonSave.LoadUserData(tan);

        if (existing != null)
        {
            // Ya hay datos previos para ese TAN: verifica contraseña
            if (existing.password != pwd)
            {
                ShowError("TAN o contraseña incorrectos.");
                return;
            }

            // Ok: inicializa con ese TAN (UserDataManager cargará el JSON)
            TANManager.CurrentTAN = tan;
            TANManager.CurrentPassword = pwd;

            UserDataManager.Instance.Init(tan);
            // No reasignes password: ya está en el JSON

            ProceedToMenu();
        }
        else
        {
            // No hay datos previos; creamos nuevos y fijamos password
            TANManager.CurrentTAN = tan;
            TANManager.CurrentPassword = pwd;

            UserDataManager.Instance.Init(tan);
            UserDataManager.Instance.SetPassword(pwd);

            // Guarda un primer JSON mínimo para dejarlo persistido
            LocalJsonSave.SaveUserData(UserDataManager.Instance.currentUserData);

            ProceedToMenu();
        }
    }

    private void ProceedToMenu()
    {
        loggedIn = true;
        // 1) Determinar idioma preferido del usuario y aplicarlo ANTES de mostrar el menú
        string lang = "es";
        if (UserDataManager.Instance != null && UserDataManager.Instance.currentUserData != null)
        {
            if (!string.IsNullOrEmpty(UserDataManager.Instance.currentUserData.languageCode))
                lang = UserDataManager.Instance.currentUserData.languageCode;
        }
        if (LocalizationManager.Instance != null)
        {
            LocalizationManager.Instance.SetLanguage(lang);
        }
        // 2) Cambiar paneles
        if (panelLogin != null) panelLogin.SetActive(false);
        if (panelMenu != null) panelMenu.SetActive(true);

        // Decide si Continue está disponible:
        bool hasSlot = PixelCrushers.SaveSystem.HasSavedGameInSlot(1);

        // Opcional: exige además que exista JSON del TAN actual
        bool hasUserDataForTan = LocalJsonSave.ExistsUserData(TANManager.CurrentTAN);

        if (btnContinue != null) btnContinue.interactable = (hasSlot && hasUserDataForTan);

        if (txtError != null) txtError.text = "";
    }

    public void OnClick_NewGame()
    {
        if (!loggedIn)
        {
            ShowError("Inicia sesión primero.");
            return;
        }

        // Empezar partida nueva: no es obligatorio borrar el slot aquí.
        SceneManager.LoadScene(newGameSceneBuildIndex);
    }

    public void OnClick_Continue()
    {
        if (!loggedIn)
        {
            ShowError("Inicia sesión primero.");
            return;
        }

        if (PixelCrushers.SaveSystem.HasSavedGameInSlot(1))
        {
            PixelCrushers.SaveSystem.LoadFromSlot(1);
        }
        else
        {
            ShowError("No hay partida guardada.");
        }
    }

    private void ShowError(string msg)
    {
        if (txtError != null) txtError.text = msg;
        Debug.LogWarning(msg);
    }
}
