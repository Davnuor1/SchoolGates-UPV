using System;
using UnityEngine;

[Serializable]
public class SnapshotDto
{
    public float totalPlayTime;
    public string gatesCompletedCSV;    // "1,2,3"
    public string gateTimesJSON;        // JSON objeto { "1":123.4, "2":56.7 }
    public int timesGameOpened;
    public string[] finalsJSON;         // ["Final2"]
    public int miniquestsCompleted;
    public string languageCode;         // "es"/"en"/"de"/"it"/"fi"
    public string dialogueSystemSaveData;

    // Informativos (server):
    public string last_version_used;
    public string updated_at;
}

// Requests/responses:
[Serializable] public class LoginRequest { public string action = "login"; public string apiKey; public string tan; public string password; public string versionId; }
[Serializable] public class LoginResponse { public bool ok; public string error; public string language; public SnapshotDto snapshot; }

[Serializable] public class SaveRequest { public string action = "save"; public string apiKey; public string tan; public string versionId; public SnapshotDto snapshot; }
[Serializable] public class SaveResponse { public bool ok; public string error; }

[Serializable] public class LoadRequest { public string action = "load"; public string apiKey; public string tan; }
[Serializable] public class LoadResponse { public bool ok; public string error; public SnapshotDto snapshot; }

// Resultado tipado para callbacks
public struct Result<T>
{
    public bool ok;
    public string error;
    public T value;
    public static Result<T> Ok(T v) => new Result<T> { ok = true, value = v };
    public static Result<T> Fail(string e) => new Result<T> { ok = false, error = e };
}
