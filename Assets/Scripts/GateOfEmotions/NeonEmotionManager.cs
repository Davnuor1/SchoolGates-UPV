using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Rendering.Universal;

public class NeonEmotionManager : MonoBehaviour
{
    [Header("GameObjects de neones (puedes arrastrarlos todos de golpe)")]
    public List<GameObject> neonObjects;

    [Header("Todos los sprites de neones (lights_0 a lights_291)")]
    public Sprite[] neonSprites;

    private class NeonData
    {
        public SpriteRenderer spriteRenderer;
        public Sprite originalSprite;
        public Light2D light2D;
        public Color originalColor;
    }

    private Dictionary<GameObject, NeonData> neonDataDict = new Dictionary<GameObject, NeonData>();
    private bool initialized = false;

    private void InitializeIfNeeded()
    {
        if (initialized) return;

        foreach (var obj in neonObjects)
        {
            if (obj == null) continue;

            NeonData data = new NeonData();

            data.spriteRenderer = obj.GetComponent<SpriteRenderer>();
            data.originalSprite = data.spriteRenderer != null ? data.spriteRenderer.sprite : null;

            data.light2D = obj.GetComponentInChildren<Light2D>();
            data.originalColor = data.light2D != null ? data.light2D.color : Color.white;

            neonDataDict[obj] = data;
        }

        initialized = true;
    }

    public void ChangeNeonsToEmotion(string emotion)
    {
        InitializeIfNeeded();
        int offset = GetEmotionOffset(emotion.ToLower());
        Color targetColor = GetEmotionColor(emotion.ToLower());

        foreach (var entry in neonDataDict)
        {
            NeonData data = entry.Value;

            // Cambiar sprite
            if (data.spriteRenderer != null && data.spriteRenderer.sprite != null)
            {
                string spriteName = data.spriteRenderer.sprite.name;
                if (spriteName.StartsWith("lights_") && int.TryParse(spriteName.Substring(7), out int baseIndex))
                {
                    string newName = $"lights_{baseIndex + offset}";
                    Sprite newSprite = FindSpriteByName(newName);
                    if (newSprite != null)
                    {
                        data.spriteRenderer.sprite = newSprite;
                    }
                    else
                    {
                        Debug.LogWarning($"Sprite '{newName}' no encontrado.");
                    }
                }
            }

            // Cambiar color de luz
            if (data.light2D != null)
            {
                data.light2D.color = targetColor;
            }
        }
    }

    public void RestoreNeonsToBase()
    {
        InitializeIfNeeded();

        foreach (var entry in neonDataDict)
        {
            NeonData data = entry.Value;

            if (data.spriteRenderer != null && data.originalSprite != null)
            {
                data.spriteRenderer.sprite = data.originalSprite;
            }

            if (data.light2D != null)
            {
                data.light2D.color = data.originalColor;
            }
        }
    }

    private Sprite FindSpriteByName(string name)
    {
        foreach (var sprite in neonSprites)
        {
            if (sprite.name == name)
                return sprite;
        }
        return null;
    }

    private int GetEmotionOffset(string emotion)
    {
        return emotion switch
        {
            "anger" => 1,
            "fear" => 2,
            "joy" => 3,
            "sadness" => 4,
            _ => 0
        };
    }

    private Color GetEmotionColor(string emotion)
    {
        return emotion switch
        {
            "anger" => new Color32(183, 48, 53, 255),     // #B73035
            "fear" => new Color32(151, 219, 206, 255),    // #97DBCE
            "joy" => new Color32(216, 219, 37, 255),      // #D8DB25
            "sadness" => new Color32(47, 94, 241, 255),   // #2F5EF1
            _ => Color.white
        };
    }
}
