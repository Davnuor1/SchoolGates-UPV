using System;
using System.Collections;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;

public class SheetsService : MonoBehaviour
{
    public static SheetsService Instance { get; private set; }

    BackendConfig cfg;
    bool busySaving = false;
    string pendingSaveJsonKey = "sog_pending_save_json";

    private bool IsOfflineBuild
    {
        get { return cfg != null && cfg.offlineBuild; }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        cfg = BackendConfigProvider.Instance.Config;

        if (IsOfflineBuild)
        {
            if (cfg != null && cfg.debugLogging) Debug.Log("SheetsService: Offline build activo. No se haran requests.");
            return;
        }

        TryFlushPending();
    }

    public IEnumerator LoginAsync(string tan, string password, Action<Result<LoginResponse>> cb)
    {
        if (IsOfflineBuild)
        {
            if (cfg != null && cfg.debugLogging) Debug.Log("LoginAsync omitido (offline build).");

            var resp = new LoginResponse();
            resp.ok = true;

            // Si tu LoginResponse tiene estos campos, los relleno.
            // Si tu clase no los tiene, elimina estas 2 lineas:
            resp.language = (LocalizationManager.Instance != null) ? LocalizationManager.Instance.CurrentLanguage : "es";
            resp.snapshot = new SnapshotDto();

            cb(Result<LoginResponse>.Ok(resp));
            yield break;
        }

        var url = cfg.apiUrl;
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
            var resp = JsonUtility.FromJson<LoginResponse>(txt);
            if (cfg.debugLogging) Debug.Log("Login response: " + txt);
            if (!resp.ok) { cb(Result<LoginResponse>.Fail(resp.error)); return; }
            cb(Result<LoginResponse>.Ok(resp));
        });
    }

    public IEnumerator SaveAsync(string tan, SnapshotDto snapshot, Action<Result<bool>> cb)
    {
        if (IsOfflineBuild)
        {
            if (cfg != null && cfg.debugLogging) Debug.Log("SaveAsync omitido (offline build). Guardado solo local.");
            cb?.Invoke(Result<bool>.Ok(true));
            yield break;
        }

        var req = new SaveRequest
        {
            apiKey = cfg.apiKey,
            tan = tan,
            versionId = cfg.versionId,
            snapshot = snapshot
        };
        var json = JsonUtility.ToJson(req);

        if (busySaving)
        {
            PlayerPrefs.SetString(pendingSaveJsonKey, json);
            PlayerPrefs.Save();
            if (cfg.debugLogging) Debug.Log("Save encolado (busy).");
            cb?.Invoke(Result<bool>.Ok(true));
            yield break;
        }

        busySaving = true;
        bool finished = false;

        yield return PostJson(cfg.apiUrl, json, (ok, txt, err) =>
        {
            finished = true;

            if (!ok)
            {
                PlayerPrefs.SetString(pendingSaveJsonKey, json);
                PlayerPrefs.Save();
                if (cfg.debugLogging) Debug.LogWarning("Save fallido. Cacheado para reintentar. Err=" + err);
                cb?.Invoke(Result<bool>.Fail(err));
            }
            else
            {
                var resp = JsonUtility.FromJson<SaveResponse>(txt);
                if (cfg.debugLogging) Debug.Log("Save response: " + txt);

                if (resp.ok)
                {
                    if (PlayerPrefs.GetString(pendingSaveJsonKey, "") == json)
                    {
                        PlayerPrefs.DeleteKey(pendingSaveJsonKey);
                        PlayerPrefs.Save();
                    }
                    cb?.Invoke(Result<bool>.Ok(true));
                }
                else
                {
                    PlayerPrefs.SetString(pendingSaveJsonKey, json);
                    PlayerPrefs.Save();
                    cb?.Invoke(Result<bool>.Fail(resp.error));
                }
            }
        });

        if (!finished && cfg.debugLogging) Debug.LogWarning("SaveAsync terminó sin callback.");
        busySaving = false;

        TryFlushPending();
    }

    public void TryFlushPending()
    {
        if (IsOfflineBuild) return;

        var pending = PlayerPrefs.GetString(pendingSaveJsonKey, "");
        if (string.IsNullOrEmpty(pending)) return;

        StartCoroutine(PostJson(cfg.apiUrl, pending, (ok, txt, err) =>
        {
            if (!ok)
            {
                if (cfg.debugLogging) Debug.LogWarning("Flush pendiente fallido: " + err);
                return;
            }

            var resp = JsonUtility.FromJson<SaveResponse>(txt);
            if (resp.ok)
            {
                if (cfg.debugLogging) Debug.Log("Flush pendiente OK.");
                PlayerPrefs.DeleteKey(pendingSaveJsonKey);
                PlayerPrefs.Save();
            }
        }));
    }

    private IEnumerator PostJson(string url, string json, Action<bool, string, string> cb, int timeoutSec = 15)
    {
        if (IsOfflineBuild)
        {
            cb(false, null, "OFFLINE_BUILD");
            yield break;
        }

        var raw = Encoding.UTF8.GetBytes(json);
        var req = new UnityWebRequest(url, "POST");
        req.uploadHandler = new UploadHandlerRaw(raw);
        req.downloadHandler = new DownloadHandlerBuffer();
        req.SetRequestHeader("Content-Type", "application/json");
        req.timeout = timeoutSec;

        if (cfg.debugLogging) Debug.Log("POST " + url + " : " + json);

        yield return req.SendWebRequest();

#if UNITY_2021_3_OR_NEWER
        bool hasErr = req.result != UnityWebRequest.Result.Success;
#else
        bool hasErr = req.isNetworkError || req.isHttpError;
#endif

        if (hasErr) cb(false, null, req.error);
        else cb(true, req.downloadHandler.text, null);

        req.Dispose();
    }
}