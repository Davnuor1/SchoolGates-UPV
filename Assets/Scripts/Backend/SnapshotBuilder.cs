using System;
using System.Linq;
using UnityEngine;

public static class SnapshotBuilder
{
    // CSV utils
    public static string ToCSV(string[] arr) => (arr == null || arr.Length == 0) ? "" : string.Join(",", arr);
    public static string[] FromCSV(string csv) => string.IsNullOrEmpty(csv) ? new string[0] : csv.Split(',');

    public static SnapshotDto FromUserData(UserData u)
    {
        var dto = new SnapshotDto();
        dto.totalPlayTime = u.totalPlayTime;
        dto.gatesCompletedCSV = ToCSV(u.completedGates);
        dto.timesGameOpened = u.timesGameOpened;
        dto.finalsJSON = u.finalsChosen ?? new string[0];
        dto.miniquestsCompleted = u.miniquestsCompletedCache;
        dto.languageCode = u.languageCode;

        // gateTimes -> JSON string. Si ya lo guardas como JSON en otra propiedad, úsala.
        // Si lo tienes como pares, conviértelo aquí a {"gateId":seconds}
        try
        {
            // Ejemplo si tienes GateTimeEntry[]
            // Construcción manual sencilla
            var pairs = u.gateTimes?.Select(gt => $"\"{gt.gateId}\":{gt.seconds.ToString(System.Globalization.CultureInfo.InvariantCulture)}");
            dto.gateTimesJSON = "{" + (pairs == null ? "" : string.Join(",", pairs)) + "}";
        }
        catch { dto.gateTimesJSON = "{}"; }

        // Dialogue System snapshot: mete aquí tu string actual
        dto.dialogueSystemSaveData = u.dialogueSystemSaveData ?? "";

        return dto;
    }

    public static void ApplyToUserData(SnapshotDto dto, UserData u, bool overrideLanguageWithUserSelection, string userSelectionCode)
    {
        if (dto == null || u == null) return;

        u.totalPlayTime = dto.totalPlayTime;
        u.completedGates = FromCSV(dto.gatesCompletedCSV);
        u.timesGameOpened = dto.timesGameOpened;
        u.finalsChosen = dto.finalsJSON ?? new string[0];
        u.miniquestsCompletedCache = dto.miniquestsCompleted;

        // Idioma: respetar elección del usuario en login si existe
        if (overrideLanguageWithUserSelection && !string.IsNullOrEmpty(userSelectionCode))
            u.languageCode = userSelectionCode;
        else if (!string.IsNullOrEmpty(dto.languageCode))
            u.languageCode = dto.languageCode;

        // Guardamos el snapshot DS en UserData para poder restaurarlo luego
        u.dialogueSystemSaveData = dto.dialogueSystemSaveData ?? "";
    }
}
