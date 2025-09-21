using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class SheetsService : MonoBehaviour
{
    public static SheetsService Instance { get; private set; }

    private BackendConfig cfg;
    private bool busySaving = false;
    private const string PendingSaveJsonKey = "sog_pending_save_json";

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        cfg = BackendConfigProvider.Instance.Config;
        TryFlushPending();
    }

    // ----------------- LOGIN -----------------

    public IEnumerator LoginAsync(string tan, string password, Action<Result<LoginResponse>> cb)
    {
        var url = cfg.apiUrl;

#if UNITY_WEBGL && !UNITY_EDITOR
        // WebGL: evitar preflight => usar form fields
        WWWForm form = new WWWForm();
        form.AddField("action", "login");
        form.AddField("apiKey", cfg.apiKey);
        form.AddField("tan", tan);
        form.AddField("password", password);
        form.AddField("versionId", cfg.versionId);

        using (var req = UnityWebRequest.Post(url, form))
        {
            if (cfg.debugLogging) Debug.Log("[SheetsService] POST(form) login " + url);
            yield return req.SendWebRequest();

#if UNITY_2021_3_OR_NEWER
            bool hasErr = req.result != UnityWebRequest.Result.Success;
#else
            bool hasErr = req.isNetworkError || req.isHttpError;
#endif
            if (hasErr)
            {
                if (cfg.debugLogging) Debug.LogWarning("[SheetsService] Login error: " + req.error);
                cb(Result<LoginResponse>.Fail(req.error));
            }
            else
            {
                var txt = req.downloadHandler.text;
                if (cfg.debugLogging) Debug.Log("[SheetsService] Login response: " + txt);
                var resp = JsonUtility.FromJson<LoginResponse>(txt);
                if (resp == null) { cb(Result<LoginResponse>.Fail("PARSE_ERROR")); yield break; }
                cb(resp.ok ? Result<LoginResponse>.Ok(resp) : Result<LoginResponse>.Fail(resp.error));
            }
        }
#else
        // Editor/PC: JSON normal
        var body = new LoginRequest
        {
            apiKey = cfg.apiKey,
            tan = tan,
            password = password,
            versionId = cfg.versionId
        };
        yield return PostJson(url, JsonUtility.ToJson(body), (ok, txt, err) =>
        {
            if (!ok) { cb(Result<LoginResponse>.Fail(err)); return; }
            if (cfg.debugLogging) Debug.Log("[SheetsService] Login response: " + txt);
            var resp = JsonUtility.FromJson<LoginResponse>(txt);
            if (resp == null) { cb(Result<LoginResponse>.Fail("PARSE_ERROR")); return; }
            cb(resp.ok ? Result<LoginResponse>.Ok(resp) : Result<LoginResponse>.Fail(resp.error));
        });
#endif
    }

    // ----------------- SAVE -----------------

    public IEnumerator SaveAsync(string tan, SnapshotDto snapshot, Action<Result<bool>> cb)
    {
        // Construimos un SaveRequest y su JSON (lo usaremos para cachear pendientes)
        var reqObj = new SaveRequest
        {
            apiKey = cfg.apiKey,
            tan = tan,
            versionId = cfg.versionId,
            snapshot = snapshot
        };
        var json = JsonUtility.ToJson(reqObj);

        // Cola simple: si ya hay un save en curso, cachea y listo
        if (busySaving)
        {
            PlayerPrefs.SetString(PendingSaveJsonKey, json);
            PlayerPrefs.Save();
            if (cfg.debugLogging) Debug.Log("[SheetsService] Save encolado (busy).");
            cb?.Invoke(Result<bool>.Ok(true));
            yield break;
        }

        busySaving = true;

#if UNITY_WEBGL && !UNITY_EDITOR
        // WebGL: enviar como form (evitar preflight). El snapshot va como string JSON en un campo.
        WWWForm form = new WWWForm();
        form.AddField("action", "save");
        form.AddField("apiKey", cfg.apiKey);
        form.AddField("tan", tan);
        form.AddField("versionId", cfg.versionId);
        form.AddField("snapshot", JsonUtility.ToJson(snapshot));

        using (var req = UnityWebRequest.Post(cfg.apiUrl, form))
        {
            if (cfg.debugLogging) Debug.Log("[SheetsService] POST(form) save " + cfg.apiUrl);
            yield return req.SendWebRequest();

#if UNITY_2021_3_OR_NEWER
            bool hasErr = req.result != UnityWebRequest.Result.Success;
#else
            bool hasErr = req.isNetworkError || req.isHttpError;
#endif
            if (hasErr)
            {
                // Cachea para reintentar luego
                PlayerPrefs.SetString(PendingSaveJsonKey, json);
                PlayerPrefs.Save();
                if (cfg.debugLogging) Debug.LogWarning("[SheetsService] Save fallido. Cacheado. Err=" + req.error);
                cb?.Invoke(Result<bool>.Fail(req.error));
            }
            else
            {
                var txt = req.downloadHandler.text;
                if (cfg.debugLogging) Debug.Log("[SheetsService] Save response: " + txt);
                var resp = JsonUtility.FromJson<SaveResponse>(txt);
                if (resp != null && resp.ok)
                {
                    // Limpia pendiente si coincide
                    if (PlayerPrefs.GetString(PendingSaveJsonKey, "") == json)
                    {
                        PlayerPrefs.DeleteKey(PendingSaveJsonKey);
                        PlayerPrefs.Save();
                    }
                    cb?.Invoke(Result<bool>.Ok(true));
                }
                else
                {
                    PlayerPrefs.SetString(PendingSaveJsonKey, json);
                    PlayerPrefs.Save();
                    cb?.Invoke(Result<bool>.Fail(resp != null ? resp.error : "PARSE_ERROR"));
                }
            }
        }
#else
        // Editor/PC: JSON normal
        yield return PostJson(cfg.apiUrl, json, (ok, txt, err) =>
        {
            if (!ok)
            {
                PlayerPrefs.SetString(PendingSaveJsonKey, json);
                PlayerPrefs.Save();
                if (cfg.debugLogging) Debug.LogWarning("[SheetsService] Save fallido. Cacheado. Err=" + err);
                cb?.Invoke(Result<bool>.Fail(err));
            }
            else
            {
                if (cfg.debugLogging) Debug.Log("[SheetsService] Save response: " + txt);
                var resp = JsonUtility.FromJson<SaveResponse>(txt);
                if (resp != null && resp.ok)
                {
                    if (PlayerPrefs.GetString(PendingSaveJsonKey, "") == json)
                    {
                        PlayerPrefs.DeleteKey(PendingSaveJsonKey);
                        PlayerPrefs.Save();
                    }
                    cb?.Invoke(Result<bool>.Ok(true));
                }
                else
                {
                    PlayerPrefs.SetString(PendingSaveJsonKey, json);
                    PlayerPrefs.Save();
                    cb?.Invoke(Result<bool>.Fail(resp != null ? resp.error : "PARSE_ERROR"));
                }
            }
        });
#endif

        busySaving = false;
        TryFlushPending();
    }

    // ----------------- FLUSH PENDIENTE -----------------

    public void TryFlushPending()
    {
        var pending = PlayerPrefs.GetString(PendingSaveJsonKey, "");
        if (string.IsNullOrEmpty(pending)) return;

#if UNITY_WEBGL && !UNITY_EDITOR
        // En WebGL, reintenta el pendiente como form (no JSON)
        SaveRequest reqObj = null;
        try { reqObj = JsonUtility.FromJson<SaveRequest>(pending); } catch { reqObj = null; }
        if (reqObj == null || reqObj.snapshot == null)
        {
            if (cfg.debugLogging) Debug.LogWarning("[SheetsService] TryFlushPending: JSON pendiente inválido.");
            return;
        }

        WWWForm form = new WWWForm();
        form.AddField("action", "save");
        form.AddField("apiKey", reqObj.apiKey);
        form.AddField("tan", reqObj.tan);
        form.AddField("versionId", reqObj.versionId);
        form.AddField("snapshot", JsonUtility.ToJson(reqObj.snapshot));

        StartCoroutine(FlushForm(form));
#else
        // En Editor/PC, podemos enviar el JSON directamente
        StartCoroutine(PostJson(cfg.apiUrl, pending, (ok, txt, err) =>
        {
            if (!ok)
            {
                if (cfg.debugLogging) Debug.LogWarning("[SheetsService] Flush pendiente fallido: " + err);
                return;
            }
            var resp = JsonUtility.FromJson<SaveResponse>(txt);
            if (resp != null && resp.ok)
            {
                if (cfg.debugLogging) Debug.Log("[SheetsService] Flush pendiente OK.");
                PlayerPrefs.DeleteKey(PendingSaveJsonKey);
                PlayerPrefs.Save();
            }
        }));
#endif
    }

#if UNITY_WEBGL && !UNITY_EDITOR
    private IEnumerator FlushForm(WWWForm form)
    {
        using (var req = UnityWebRequest.Post(cfg.apiUrl, form))
        {
            if (cfg.debugLogging) Debug.Log("[SheetsService] POST(form) flush " + cfg.apiUrl);
            yield return req.SendWebRequest();

#if UNITY_2021_3_OR_NEWER
            bool hasErr = req.result != UnityWebRequest.Result.Success;
#else
            bool hasErr = req.isNetworkError || req.isHttpError;
#endif
            if (hasErr)
            {
                if (cfg.debugLogging) Debug.LogWarning("[SheetsService] Flush pendiente fallido: " + req.error);
            }
            else
            {
                var txt = req.downloadHandler.text;
                var resp = JsonUtility.FromJson<SaveResponse>(txt);
                if (resp != null && resp.ok)
                {
                    if (cfg.debugLogging) Debug.Log("[SheetsService] Flush pendiente OK.");
                    PlayerPrefs.DeleteKey(PendingSaveJsonKey);
                    PlayerPrefs.Save();
                }
            }
        }
    }
#endif

    // ----------------- HTTP JSON (Editor/PC) -----------------

    private IEnumerator PostJson(string url, string json, Action<bool, string, string> cb, int timeoutSec = 15)
    {
        byte[] raw = Encoding.UTF8.GetBytes(json);
        using (var req = new UnityWebRequest(url, "POST"))
        {
            req.uploadHandler = new UploadHandlerRaw(raw);
            req.downloadHandler = new DownloadHandlerBuffer();
            req.SetRequestHeader("Content-Type", "application/json");
            req.timeout = timeoutSec;

            if (cfg.debugLogging) Debug.Log("[SheetsService] POST(json) " + url + " : " + json);

            yield return req.SendWebRequest();

#if UNITY_2021_3_OR_NEWER
            bool hasErr = req.result != UnityWebRequest.Result.Success;
#else
            bool hasErr = req.isNetworkError || req.isHttpError;
#endif
            if (hasErr) cb(false, null, req.error);
            else cb(true, req.downloadHandler.text, null);
        }
    }
}
