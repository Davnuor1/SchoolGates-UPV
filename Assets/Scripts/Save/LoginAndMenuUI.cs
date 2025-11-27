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
    [SerializeField] private int newGameSceneBuildIndex = 0;          // escena inicial (New Game)
    [SerializeField] private int continueFallbackSceneBuildIndex = 0; // escena a la que llevar con Continue en WebGL (ej: plaza principal)

    private bool loggedIn = false;

    // Hay progreso remoto en Sheets (snapshot con datos).
    private bool hasServerProgress = false;

    // Último snapshot recibido del backend (por si lo quieres usar).
    private SnapshotDto lastBackendSnapshot;

    private void Awake()
    {
        if (panelLogin != null) panelLogin.SetActive(true);
        if (panelMenu != null) panelMenu.SetActive(false);
        if (txtError != null) txtError.text = "";
    }

    // ------------------------------------------------------
    // BOTÓN ENTER (LOGIN)
    // ------------------------------------------------------
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
        // Dejar TAN/Password globalmente accesibles
        TANManager.CurrentTAN = tan;
        TANManager.CurrentPassword = pwd;

        if (UserDataManager.Instance == null)
        {
            Debug.LogError("Falta UserDataManager en la escena de Login.");
            ShowError("Error interno de configuración.");
            yield break;
        }

        // Inicializa UserDataManager (carga JSON local si existe)
        UserDataManager.Instance.Init(tan);

        bool triedOnline = (SheetsService.Instance != null);
        bool onlineOk = false;
        string onlineErr = "";
        LoginResponse onlineResp = null;

        // ------------------------------
        // 1) Intento ONLINE (backend)
        // ------------------------------
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

        // Si el backend responde OK: usamos snapshot remoto
        if (triedOnline && onlineOk && onlineResp != null)
        {
            ApplyOnlineSnapshotAndPersist(onlineResp, chosenLangCode, pwd);
            ProceedToMenu();
            yield break;
        }
        else if (triedOnline && !onlineOk && !string.IsNullOrEmpty(onlineErr))
        {
            // Si el backend dice credenciales inválidas, no tiene sentido seguir
            string lower = onlineErr.ToLower();
            if (lower.Contains("invalid") || lower.Contains("credential"))
            {
                ShowError("TAN o contraseña incorrectos.");
                yield break;
            }

            // Si es error de red, seguimos con fallback offline
            Debug.LogWarning("Login backend falló, usando modo offline. Err=" + onlineErr);
        }

        // ------------------------------
        // 2) Fallback OFFLINE
        // ------------------------------
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

    // Aplica snapshot del backend a UserData y guarda
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

        // Aquí usas tu propio método para volcar el snapshot al UserData
        // (tiempos, gates completadas, finales, miniquests, DS, etc.)
        SnapshotBuilder.ApplyToUserData(resp.snapshot, ud, true, chosenLangCode);

        // Garantizar password en el JSON local
        ud.password = pwd;
        LocalJsonSave.SaveUserData(ud);

        // Idioma preferido
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
        // Añade más señales si quieres (por ejemplo, tiempo en alguna gate específica)
        return false;
    }

    // ------------------------------------------------------
    // PASO AL MENÚ
    // ------------------------------------------------------
    private void ProceedToMenu()
    {
        loggedIn = true;

        // Aplicar idioma a la UI
        if (UserDataManager.Instance != null && UserDataManager.Instance.currentUserData != null)
        {
            if (LocalizationManager.Instance != null)
                LocalizationManager.Instance.SetLanguage(UserDataManager.Instance.currentUserData.languageCode);
        }

        if (panelLogin != null) panelLogin.SetActive(false);
        if (panelMenu != null) panelMenu.SetActive(true);

        bool hasLocalSlot = PixelCrushers.SaveSystem.HasSavedGameInSlot(1);

#if UNITY_WEBGL && !UNITY_EDITOR
        // En WebGL: el continue real con SaveSystem es frágil entre builds/links.
        // Chapuza razonable: solo habilitamos Continue si hay progreso remoto.
        if (btnContinue != null) btnContinue.interactable = hasServerProgress;
#else
        // En Editor/Standalone: si hay slot local o progreso remoto, podemos continuar.
        if (btnContinue != null) btnContinue.interactable = (hasLocalSlot || hasServerProgress);
#endif

        if (txtError != null) txtError.text = "";
    }

    // ------------------------------------------------------
    // BOTÓN NEW GAME
    // ------------------------------------------------------
    public void OnClick_NewGame()
    {
        if (!loggedIn)
        {
            ShowError("Inicia sesión primero.");
            return;
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
            ShowError("Inicia sesión primero.");
            return;
        }

#if UNITY_WEBGL && !UNITY_EDITOR
        // WEBGL / itch.io:
        // Ignoramos el SaveSystem local (puede no existir o estar roto entre links/builds).
        // Si hay datos en el backend, hacemos "soft-continue" a una escena segura.
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
        // EDITOR / STANDALONE:
        // Primero intentamos el continue real con SaveSystem.
        if (PixelCrushers.SaveSystem.HasSavedGameInSlot(1))
        {
            PixelCrushers.SaveSystem.LoadFromSlot(1);
        }
        else if (hasServerProgress && continueFallbackSceneBuildIndex >= 0)
        {
            // Si no hay slot local pero sí progreso remoto, usamos soft-continue.
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
    // UTIL
    // ------------------------------------------------------
    private void ShowError(string msg)
    {
        if (txtError != null) txtError.text = msg;
        Debug.LogWarning(msg);
    }
}
