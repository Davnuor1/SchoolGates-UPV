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
    [SerializeField] private Button btnQuit; // NUEVO

    [Header("Scenes")]
    [SerializeField] private int newGameSceneBuildIndex = 0;
    [SerializeField] private int continueFallbackSceneBuildIndex = 0;

    [Header("Offline mode")]
    [SerializeField] private bool wipeLocalSlotOnNewGame = true;
    [SerializeField] private string localProfilePrefsKey = "sog_local_profile_tan";

    private bool loggedIn = false;
    private bool hasServerProgress = false;
    private SnapshotDto lastBackendSnapshot;

    private BackendConfig cfg;
    private bool offlineBuildCached = false;

    private bool IsOfflineBuild
    {
        get { return offlineBuildCached; }
    }

    private void Awake()
    {
        if (txtError != null) txtError.text = "";

        if (panelLogin != null) panelLogin.SetActive(true);
        if (panelMenu != null) panelMenu.SetActive(false);

        if (btnQuit != null) btnQuit.gameObject.SetActive(false); // por defecto oculto
    }

    private void Start()
    {
        cfg = (BackendConfigProvider.Instance != null) ? BackendConfigProvider.Instance.Config : null;
        offlineBuildCached = (cfg != null && cfg.offlineBuild);

        if (IsOfflineBuild)
        {
            BootOfflineMode();
        }
    }

    // ------------------------------------------------------
    // OFFLINE BOOT
    // ------------------------------------------------------
    private void BootOfflineMode()
    {
        if (txtError != null) txtError.text = "";

        string chosenLangCode = (LocalizationManager.Instance != null)
            ? LocalizationManager.Instance.CurrentLanguage
            : "es";

        string tan = PlayerPrefs.GetString(localProfilePrefsKey, "");
        if (string.IsNullOrEmpty(tan))
        {
            tan = "local_" + Random.Range(10000000, 99999999).ToString();
            PlayerPrefs.SetString(localProfilePrefsKey, tan);
            PlayerPrefs.Save();
        }

        TANManager.CurrentTAN = tan;
        TANManager.CurrentPassword = "";

        if (UserDataManager.Instance == null)
        {
            Debug.LogError("Falta UserDataManager en la escena de Login (offline).");
            ShowError("Error interno de configuración.");
            return;
        }

        UserDataManager.Instance.Init(tan);

        if (UserDataManager.Instance.currentUserData == null)
        {
            UserDataManager.Instance.CreateNewUserData(tan);
        }

        UserDataManager.Instance.currentUserData.languageCode = chosenLangCode;
        UserDataManager.Instance.currentUserData.password = "";

        LocalJsonSave.SaveUserData(UserDataManager.Instance.currentUserData);

        hasServerProgress = false;
        lastBackendSnapshot = null;

        ProceedToMenu_Offline();
    }

    private void ProceedToMenu_Offline()
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

        if (btnContinue != null) btnContinue.interactable = hasLocalSlot;

        // Mostrar botón Quit solo en offline y solo si NO es WebGL
#if UNITY_WEBGL && !UNITY_EDITOR
        if (btnQuit != null) btnQuit.gameObject.SetActive(false);
#else
        if (btnQuit != null) btnQuit.gameObject.SetActive(true);
#endif

        if (txtError != null) txtError.text = "";
    }

    // ------------------------------------------------------
    // BOTÓN ENTER (LOGIN) - solo online build
    // ------------------------------------------------------
    public void OnClick_Enter()
    {
        if (IsOfflineBuild)
        {
            ProceedToMenu_Offline();
            return;
        }

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

        if (UserDataManager.Instance == null)
        {
            Debug.LogError("Falta UserDataManager en la escena de Login.");
            ShowError("Error interno de configuración.");
            yield break;
        }

        UserDataManager.Instance.Init(tan);

        bool triedOnline = (SheetsService.Instance != null);
        bool onlineOk = false;
        string onlineErr = "";
        LoginResponse onlineResp = null;

        if (triedOnline)
        {
            bool done = false;
            yield return StartCoroutine(
                SheetsService.Instance.LoginAsync(tan, pwd, result =>
                {
                    if (!result.ok)
                    {
                        onlineOk = false;
                        onlineErr = result.error;
                    }
                    else
                    {
                        onlineOk = true;
                        onlineResp = result.value;
                    }
                    done = true;
                })
            );
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
            string lower = onlineErr.ToLower();
            if (lower.Contains("invalid") || lower.Contains("credential"))
            {
                ShowError("TAN o contraseña incorrectos.");
                yield break;
            }

            Debug.LogWarning("Login backend falló, usando modo offline. Err=" + onlineErr);
        }

        var existing = LocalJsonSave.LoadUserData(tan);
        if (existing != null)
        {
            if (existing.password != pwd)
            {
                ShowError("TAN o contraseña incorrectos (offline).");
                yield break;
            }

            var udm = UserDataManager.Instance;
            udm.currentUserData = existing;
            udm.currentUserData.languageCode = chosenLangCode;
            LocalJsonSave.SaveUserData(udm.currentUserData);

            hasServerProgress = false;
            lastBackendSnapshot = null;
            ProceedToMenu();
        }
        else
        {
            var udm = UserDataManager.Instance;
            udm.CreateNewUserData(tan);
            udm.SetPassword(pwd);
            udm.currentUserData.languageCode = chosenLangCode;
            LocalJsonSave.SaveUserData(udm.currentUserData);

            hasServerProgress = false;
            lastBackendSnapshot = null;
            ProceedToMenu();
        }
    }

    private void ApplyOnlineSnapshotAndPersist(LoginResponse resp, string chosenLangCode, string pwd)
    {
        var udm = UserDataManager.Instance;
        var ud = udm.currentUserData;

        if (ud == null)
        {
            udm.CreateNewUserData(TANManager.CurrentTAN);
            ud = udm.currentUserData;
        }

        lastBackendSnapshot = resp.snapshot;

        SnapshotBuilder.ApplyToUserData(resp.snapshot, ud, true, chosenLangCode);

        ud.password = pwd;
        LocalJsonSave.SaveUserData(ud);

        if (LocalizationManager.Instance != null)
            LocalizationManager.Instance.SetLanguage(ud.languageCode);

        hasServerProgress = HasProgress(resp.snapshot);
    }

    private bool HasProgress(SnapshotDto snap)
    {
        if (snap == null) return false;
        if (snap.totalPlayTime > 0f) return true;
        if (!string.IsNullOrEmpty(snap.gatesCompletedCSV)) return true;
        if (snap.miniquestsCompleted > 0) return true;
        if (snap.finalsJSON != null && snap.finalsJSON.Length > 0) return true;
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

#if UNITY_WEBGL && !UNITY_EDITOR
        if (btnContinue != null) btnContinue.interactable = hasServerProgress;
#else
        if (btnContinue != null) btnContinue.interactable = (hasLocalSlot || hasServerProgress);
#endif

        // Quit oculto fuera de offline
        if (btnQuit != null) btnQuit.gameObject.SetActive(false);

        if (txtError != null) txtError.text = "";
    }

    // ------------------------------------------------------
    // BOTÓN NEW GAME
    // ------------------------------------------------------
    public void OnClick_NewGame()
    {
        if (!loggedIn)
        {
            if (IsOfflineBuild) ProceedToMenu_Offline();
            else { ShowError("Inicia sesión primero."); return; }
        }

        if (IsOfflineBuild)
        {
            if (wipeLocalSlotOnNewGame)
            {
                PixelCrushers.SaveSystem.DeleteSavedGameInSlot(1);
            }

            if (UserDataManager.Instance != null && UserDataManager.Instance.currentUserData != null)
            {
                string tan = UserDataManager.Instance.currentUserData.tan;
                UserDataManager.Instance.CreateNewUserData(tan);
                UserDataManager.Instance.currentUserData.password = "";
                LocalJsonSave.SaveUserData(UserDataManager.Instance.currentUserData);
            }
        }

        SceneManager.LoadScene(newGameSceneBuildIndex);
    }

    // ------------------------------------------------------
    // BOTÓN CONTINUE
    // ------------------------------------------------------
    public void OnClick_Continue()
    {
        if (!loggedIn)
        {
            if (IsOfflineBuild) ProceedToMenu_Offline();
            else { ShowError("Inicia sesión primero."); return; }
        }

        if (IsOfflineBuild)
        {
            if (PixelCrushers.SaveSystem.HasSavedGameInSlot(1))
                PixelCrushers.SaveSystem.LoadFromSlot(1);
            else
                ShowError("No hay partida guardada.");
            return;
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        if (hasServerProgress && continueFallbackSceneBuildIndex >= 0)
        {
            Debug.Log("[LoginAndMenuUI] Continue (WebGL): soft-continue a escena " + continueFallbackSceneBuildIndex);
            SceneManager.LoadScene(continueFallbackSceneBuildIndex);
        }
        else
        {
            ShowError("No hay partida guardada.");
        }
#else
        if (PixelCrushers.SaveSystem.HasSavedGameInSlot(1))
        {
            PixelCrushers.SaveSystem.LoadFromSlot(1);
        }
        else if (hasServerProgress && continueFallbackSceneBuildIndex >= 0)
        {
            Debug.Log("[LoginAndMenuUI] Continue: sin slot local pero con progreso remoto. Escena fallback " + continueFallbackSceneBuildIndex);
            SceneManager.LoadScene(continueFallbackSceneBuildIndex);
        }
        else
        {
            ShowError("No hay partida guardada.");
        }
#endif
    }

    // ------------------------------------------------------
    // BOTÓN QUIT (solo offline)
    // ------------------------------------------------------
    public void OnClick_Quit()
    {
        // Seguridad: solo tiene sentido en offline
        if (!IsOfflineBuild) return;

#if UNITY_WEBGL && !UNITY_EDITOR
        ShowError("No se puede cerrar el juego en WebGL.");
#else
        Application.Quit();
#endif
    }

    // ------------------------------------------------------
    // UTIL
    // ------------------------------------------------------
    private void ShowError(string msg)
    {
        if (txtError != null) txtError.text = msg;
        Debug.LogWarning(msg);
    }
}