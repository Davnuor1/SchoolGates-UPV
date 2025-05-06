using UnityEngine;
using System.Collections.Generic;

public class NeonEmotionManager : MonoBehaviour
{
    [Header("GameObjects de neones (puedes arrastrarlos todos de golpe)")]
    public List<GameObject> neonObjects;

    [Header("Todos los sprites de neones (lights_0 a lights_291)")]
    public Sprite[] neonSprites;

    private Dictionary<SpriteRenderer, Sprite> originalSprites = new Dictionary<SpriteRenderer, Sprite>();
    private bool initialized = false;

    private void InitializeIfNeeded()
    {
        if (initialized) return;

        foreach (var obj in neonObjects)
        {
            if (obj == null) continue;

            SpriteRenderer sr = obj.GetComponent<SpriteRenderer>();
            if (sr != null)
            {
                if (!originalSprites.ContainsKey(sr))
                {
                    originalSprites[sr] = sr.sprite;
                }
            }
            else
            {
                Debug.LogWarning($"El objeto '{obj.name}' no tiene SpriteRenderer.");
            }
        }

        initialized = true;
    }

    public void ChangeNeonsToEmotion(string emotion)
    {
        InitializeIfNeeded();

        int offset = GetEmotionOffset(emotion.ToLower());

        foreach (var pair in originalSprites)
        {
            SpriteRenderer sr = pair.Key;
            Sprite currentSprite = sr.sprite;

            if (currentSprite == null || !currentSprite.name.StartsWith("lights_")) continue;

            if (int.TryParse(currentSprite.name.Substring(7), out int baseIndex))
            {
                string newName = $"lights_{baseIndex + offset}";
                Sprite newSprite = FindSpriteByName(newName);

                if (newSprite != null)
                {
                    sr.sprite = newSprite;
                }
                else
                {
                    Debug.LogWarning($"Sprite '{newName}' no encontrado.");
                }
            }
        }
    }

    public void RestoreNeonsToBase()
    {
        InitializeIfNeeded();

        foreach (var pair in originalSprites)
        {
            if (pair.Key != null && pair.Value != null)
            {
                pair.Key.sprite = pair.Value;
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
}
