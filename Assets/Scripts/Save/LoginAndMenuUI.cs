using System.Collections;
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

    [Header("Scenes")]
    [SerializeField] private int newGameSceneBuildIndex = 0;          // escena inicial del juego (Hub)
    [SerializeField] private int continueFallbackSceneBuildIndex = 0;  // si no hay slot local, cargar esta (normalmente la misma Hub)

    private bool loggedIn = false;
    private bool hasServerProgress = false; // nuevo: progreso detectado en snapshot del backend

    private void Awake()
    {
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

        string chosenLangCode = (LocalizationManager.Instance != null)
            ? LocalizationManager.Instance.CurrentLanguage
            : "es";

        StartCoroutine(LoginFlow(tan, pwd, chosenLangCode));
    }

    private IEnumerator LoginFlow(string tan, string pwd, string chosenLangCode)
    {
        TANManager.CurrentTAN = tan;
        TANManager.CurrentPassword = pwd;
        UserDataManager.Instance.Init(tan);

        // Online primero
        bool triedOnline = SheetsService.Instance != null;
        bool onlineOk = false;
        string onlineErr = "";
        LoginResponse onlineResp = null;

        if (triedOnline)
        {
            bool done = false;
            yield return StartCoroutine(SheetsService.Instance.LoginAsync(tan, pwd, result =>
            {
                if (!result.ok) { onlineOk = false; onlineErr = result.error; }
                else { onlineOk = true; onlineResp = result.value; }
                done = true;
            }));
            while (!done) yield return null;
        }

        if (triedOnline && onlineOk && onlineResp != null)
        {
            ApplyOnlineSnapshotAndPersist(onlineResp, chosenLangCode, pwd);
            ProceedToMenu();
            yield break;
        }
        else if (triedOnline && !onlineOk && !string.IsNullOrEmpty(onlineErr))
        {
            if (onlineErr.ToLower().Contains("invalid") || onlineErr.ToLower().Contains("credential"))
            {
                ShowError("TAN o contraseña incorrectos.");
                yield break;
            }
            // si es error de red, caerá a offline
        }

        // Offline fallback
        var existing = LocalJsonSave.LoadUserData(tan);
        if (existing != null)
        {
            if (existing.password != pwd)
            {
                ShowError("TAN o contraseña incorrectos (offline).");
                yield break;
            }

            // Usa idioma elegido en login
            UserDataManager.Instance.currentUserData.languageCode = chosenLangCode;
            LocalJsonSave.SaveUserData(UserDataManager.Instance.currentUserData);

            // Estima progreso offline si quieres (opcional)
            hasServerProgress = false; // sin servidor no lo sabemos con certeza
            ProceedToMenu();
        }
        else
        {
            // Crear perfil nuevo offline
            UserDataManager.Instance.SetPassword(pwd);
            UserDataManager.Instance.currentUserData.languageCode = chosenLangCode;
            LocalJsonSave.SaveUserData(UserDataManager.Instance.currentUserData);

            hasServerProgress = false;
            ProceedToMenu();
        }
    }

    private void ApplyOnlineSnapshotAndPersist(LoginResponse resp, string chosenLangCode, string pwd)
    {
        var udm = UserDataManager.Instance;
        var ud = udm.currentUserData;

        ud.timesGameOpened++;

        SnapshotBuilder.ApplyToUserData(resp.snapshot, ud, true, chosenLangCode);
        ud.password = pwd;

        if (LocalizationManager.Instance != null)
            LocalizationManager.Instance.SetLanguage(ud.languageCode);

        LocalJsonSave.SaveUserData(ud);

        // Detecta progreso de servidor para habilitar Continue sin slot local
        hasServerProgress = HasProgress(resp.snapshot);
    }

    private bool HasProgress(SnapshotDto snap)
    {
        if (snap == null) return false;
        if (snap.totalPlayTime > 0) return true;
        if (!string.IsNullOrEmpty(snap.gatesCompletedCSV)) return true;
        if (snap.miniquestsCompleted > 0) return true;
        if (snap.finalsJSON != null && snap.finalsJSON.Length > 0) return true;
        // Añade otros indicadores si quieres (por ejemplo, si guardas última escena)
        return false;
    }

    private void ProceedToMenu()
    {
        loggedIn = true;

        if (UserDataManager.Instance != null && UserDataManager.Instance.currentUserData != null)
        {
            if (LocalizationManager.Instance != null)
                LocalizationManager.Instance.SetLanguage(UserDataManager.Instance.currentUserData.languageCode);
        }

        if (panelLogin != null) panelLogin.SetActive(false);
        if (panelMenu != null) panelMenu.SetActive(true);

        bool hasLocalSlot = PixelCrushers.SaveSystem.HasSavedGameInSlot(1);
        if (btnContinue != null) btnContinue.interactable = (hasLocalSlot || hasServerProgress);

        if (txtError != null) txtError.text = "";
    }

    public void OnClick_NewGame()
    {
        if (!loggedIn) { ShowError("Inicia sesión primero."); return; }
        SceneManager.LoadScene(newGameSceneBuildIndex);
    }

    public void OnClick_Continue()
    {
        if (!loggedIn) { ShowError("Inicia sesión primero."); return; }

        if (PixelCrushers.SaveSystem.HasSavedGameInSlot(1))
        {
            PixelCrushers.SaveSystem.LoadFromSlot(1);
        }
        else if (hasServerProgress)
        {
            // No hay slot local pero hay progreso en servidor: carga escena base y deja que el Restorer aplique DS
            SceneManager.LoadScene(continueFallbackSceneBuildIndex);
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
