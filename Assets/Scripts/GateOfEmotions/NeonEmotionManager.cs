using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NeonEmotionManager : MonoBehaviour
{
    [System.Serializable]
    public class NeonTarget
    {
        [Header("Render de este neón (elige uno)")]
        public SpriteRenderer spriteRenderer; // Mundo 2D
        public Image uiImage;                 // UI (opcional)

        [Header("Sprites por emoción")]
        public Sprite baseSprite;
        public Sprite angerSprite;
        public Sprite fearSprite;
        public Sprite joySprite;
        public Sprite sadnessSprite;

        // Devuelve el sprite correcto o base si falta
        public Sprite GetSpriteFor(EmotionId id)
        {
            switch (id)
            {
                case EmotionId.Anger: return angerSprite != null ? angerSprite : baseSprite;
                case EmotionId.Fear: return fearSprite != null ? fearSprite : baseSprite;
                case EmotionId.Joy: return joySprite != null ? joySprite : baseSprite;
                case EmotionId.Sadness: return sadnessSprite != null ? sadnessSprite : baseSprite;
                default: return baseSprite;
            }
        }

        public void ApplySprite(Sprite s)
        {
            if (spriteRenderer != null) spriteRenderer.sprite = s;
            if (uiImage != null) uiImage.sprite = s;
        }
    }

    [Header("Lista de neones a controlar")]
    public List<NeonTarget> neons = new List<NeonTarget>();

    /* ========== API NUEVA (EmotionId) ========== */

    public void ChangeNeonsToEmotion(EmotionId emotion)
    {
        foreach (var n in neons)
        {
            if (n == null) continue;
            n.ApplySprite(n.GetSpriteFor(emotion));
        }
    }

    public void ResetNeonsToBase()
    {
        foreach (var n in neons)
        {
            if (n == null) continue;
            n.ApplySprite(n.baseSprite);
        }
    }

    public void ChangeSingleNeonToEmotion(EmotionId emotion, int index)
    {
        if (index < 0 || index >= neons.Count) return;
        var n = neons[index];
        if (n == null) return;
        n.ApplySprite(n.GetSpriteFor(emotion));
    }

    public void ResetSingleNeonToBase(int index)
    {
        if (index < 0 || index >= neons.Count) return;
        var n = neons[index];
        if (n == null) return;
        n.ApplySprite(n.baseSprite);
    }

    /* ======= WRAPPERS COMPAT. ANTIGUA (strings) ======= */
    // Reutilizan tu EmotionUtils.ToEmotionId(string)

    public void ChangeNeonsToEmotion(string emotionKey)
        => ChangeNeonsToEmotion(EmotionUtils.ToEmotionId(emotionKey));

    public void RestoreNeonsToBase() => ResetNeonsToBase();

    public void ChangeSingleNeonToEmotion(string emotionKey, int index)
        => ChangeSingleNeonToEmotion(EmotionUtils.ToEmotionId(emotionKey), index);

    // Mantengo el nombre exacto que usabas:
    public void ResetSingleNeonTobase(int index) => ResetSingleNeonToBase(index);
}
