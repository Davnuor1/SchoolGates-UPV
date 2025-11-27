using System;
using System.Collections.Generic;

public static class EmotionUtils
{
    // Normaliza cualquier texto local a un EmotionId.
    public static EmotionId ToEmotionId(string text, string langCode = null)
    {
        if (string.IsNullOrWhiteSpace(text)) return EmotionId.Joy; // fallback inocuo
        var t = text.Trim().ToLowerInvariant();

        // Sinónimos por idioma (puedes ampliar sin problema)
        // ES
        if (t == "ira" || t == "enfado" || t == "rabia") return EmotionId.Anger;
        if (t == "miedo") return EmotionId.Fear;
        if (t == "felicidad" || t == "alegría" || t == "alegria") return EmotionId.Joy;
        if (t == "tristeza") return EmotionId.Sadness;

        // EN
        if (t == "anger") return EmotionId.Anger;
        if (t == "fear") return EmotionId.Fear;
        if (t == "joy" || t == "happiness") return EmotionId.Joy;
        if (t == "sadness") return EmotionId.Sadness;

        // IT
        if (t == "rabbia" || t == "collera") return EmotionId.Anger;
        if (t == "paura") return EmotionId.Fear;
        if (t == "gioia" || t == "felicità" || t == "felicita") return EmotionId.Joy;
        if (t == "tristezza") return EmotionId.Sadness;

        // DE
        if (t == "wut" || t == "zorn") return EmotionId.Anger;
        if (t == "angst") return EmotionId.Fear;
        if (t == "freude") return EmotionId.Joy;
        if (t == "traurigkeit") return EmotionId.Sadness;

        // FI
        if (t == "Suuttumus") return EmotionId.Anger;
        if (t == "pelko") return EmotionId.Fear;
        if (t == "ilo") return EmotionId.Joy;
        if (t == "Surullisuus") return EmotionId.Sadness;

        // Si llega un texto no contemplado, intenta heurística simple
        if (t.Contains("anger") || t.Contains("ira") || t.Contains("rabb") || t.Contains("wut")) return EmotionId.Anger;
        if (t.Contains("fear") || t.Contains("miedo") || t.Contains("paur") || t.Contains("angst")) return EmotionId.Fear;
        if (t.Contains("joy") || t.Contains("felic") || t.Contains("gioia") || t.Contains("freude") || t.Contains("ilo")) return EmotionId.Joy;
        if (t.Contains("sad") || t.Contains("trist") || t.Contains("trau") || t.Contains("suru")) return EmotionId.Sadness;

        return EmotionId.Joy;
    }

    // Devuelve el nombre localizado de una emoción usando tu SO de localización
    public static string GetLocalizedName(GateOfEmotionsLocalization loc, EmotionId id)
    {
        switch (id)
        {
            case EmotionId.Anger: return loc.anger;
            case EmotionId.Fear: return loc.fear;
            case EmotionId.Joy: return loc.joy;
            case EmotionId.Sadness: return loc.sadness;
            default: return loc.joy;
        }
    }
}
