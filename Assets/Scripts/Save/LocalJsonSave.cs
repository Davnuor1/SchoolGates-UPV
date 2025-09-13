using System.IO;
using UnityEngine;

public static class LocalJsonSave
{
    private static string GetPath(string tan)
    {
        return Application.persistentDataPath + "/userdata_" + tan + ".json";
    }

    public static void SaveUserData(UserData data)
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        if (data == null || string.IsNullOrEmpty(data.tan)) return;
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(GetPath(data.tan), json);
        Debug.Log("UserData guardado en: " + GetPath(data.tan));
#endif
    }

    public static UserData LoadUserData(string tan)
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        string path = GetPath(tan);
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            var data = JsonUtility.FromJson<UserData>(json);
            Debug.Log("UserData cargado desde: " + path);
            return data;
        }
#endif
        return null;
    }

    public static bool ExistsUserData(string tan)
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        return File.Exists(GetPath(tan));
#else
        return false;
#endif
    }

    public static void DeleteUserData(string tan)
    {
#if !UNITY_WEBGL || UNITY_EDITOR
        string path = GetPath(tan);
        if (File.Exists(path))
        {
            File.Delete(path);
            Debug.Log("UserData JSON borrado: " + path);
        }
#endif
    }
}
