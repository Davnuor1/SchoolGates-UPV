using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class NeonEmotionManager : MonoBehaviour
{
    [Header("Auto-collect")]
    [SerializeField] private bool autoCollect = true;
    [SerializeField] private Transform neonRoot;
    [SerializeField] private bool includeInactive = true;
    [SerializeField] private bool rebuildOnAwake = true;

    [Header("Sprite sheet (5 consecutive variants per neon)")]
    [Tooltip("Resources path without extension. Example: Sprites/lights (file must be in Assets/Resources/Sprites/lights.png)")]
    [SerializeField] private string resourcesPath = "Sprites/lights";

    [SerializeField] private List<Sprite> sheetSprites = new List<Sprite>();

    [Header("Lights2D (optional)")]
    [SerializeField] private bool affectLights = true;
    [SerializeField] private float intensityMultiplier = 1f;
    [SerializeField] private Color angerLightColor = Color.red;
    [SerializeField] private Color fearLightColor = Color.cyan;
    [SerializeField] private Color joyLightColor = Color.yellow;
    [SerializeField] private Color sadnessLightColor = Color.blue;

    [Header("Debug")]
    [SerializeField] private bool debugLogging = false;

    [Header("Neons (auto-filled if autoCollect = true)")]
    [SerializeField] private List<Transform> neons = new List<Transform>();

    private readonly List<NeonRuntime> runtime = new List<NeonRuntime>();
    private readonly Dictionary<string, int> spriteNameToIndex = new Dictionary<string, int>(StringComparer.Ordinal);

    [Serializable]
    private class NeonRuntime
    {
        public Transform root;
        public SpriteRenderer sr;
        public Image uiImage;
        public Light2D light2D;

        public Sprite baseSprite;
        public int baseSpriteIndex = -1;

        public Color baseLightColor;
        public float baseLightIntensity;
        public bool hasLightBase = false;
    }

    private void Awake()
    {
        if (rebuildOnAwake)
        {
            Rebuild();
        }
    }

    [ContextMenu("Rebuild Now")]
    public void Rebuild()
    {
        LoadSheetSprites();
        CollectNeonsIfNeeded();
        BuildRuntimeCache();
        if (debugLogging)
        {
            Debug.Log(
                "NeonEmotionManager Rebuild: sheetSprites=" + sheetSprites.Count +
                " neons=" + neons.Count +
                " runtime=" + runtime.Count
            );
        }
    }

    private void LoadSheetSprites()
    {
        sheetSprites.Clear();
        spriteNameToIndex.Clear();

        if (string.IsNullOrEmpty(resourcesPath))
        {
            if (debugLogging) Debug.LogWarning("NeonEmotionManager: resourcesPath is empty.");
            return;
        }

        Sprite[] loaded = Resources.LoadAll<Sprite>(resourcesPath);
        if (loaded == null || loaded.Length == 0)
        {
            if (debugLogging) Debug.LogWarning("NeonEmotionManager: no sprites loaded from Resources at: " + resourcesPath);
            return;
        }

        for (int i = 0; i < loaded.Length; i++)
        {
            Sprite s = loaded[i];
            if (s == null) continue;
            sheetSprites.Add(s);
        }

        for (int i = 0; i < sheetSprites.Count; i++)
        {
            Sprite s = sheetSprites[i];
            if (s == null) continue;
            // last one wins if duplicates
            spriteNameToIndex[s.name] = i;
        }
    }

    private void CollectNeonsIfNeeded()
    {
        if (!autoCollect) return;
        if (neonRoot == null) return;

        neons.Clear();

        int childCount = neonRoot.childCount;
        for (int i = 0; i < childCount; i++)
        {
            Transform t = neonRoot.GetChild(i);
            if (t == null) continue;

            if (!includeInactive)
            {
                if (!t.gameObject.activeInHierarchy) continue;
            }

            neons.Add(t);
        }
    }

    private void BuildRuntimeCache()
    {
        runtime.Clear();

        int missingBase = 0;

        for (int i = 0; i < neons.Count; i++)
        {
            Transform t = neons[i];
            if (t == null) continue;

            var r = new NeonRuntime();
            r.root = t;

            // Prefer SR on the same object, else search children
            r.sr = t.GetComponent<SpriteRenderer>();
            if (r.sr == null) r.sr = t.GetComponentInChildren<SpriteRenderer>(includeInactive);

            // Optional UI Image
            r.uiImage = t.GetComponent<Image>();
            if (r.uiImage == null) r.uiImage = t.GetComponentInChildren<Image>(includeInactive);

            // Light2D usually in child
            r.light2D = t.GetComponentInChildren<Light2D>(includeInactive);

            // Cache base sprite and index
            if (r.sr != null)
            {
                r.baseSprite = r.sr.sprite;
                r.baseSpriteIndex = FindSpriteIndex(r.baseSprite);

                if (r.baseSpriteIndex < 0)
                {
                    missingBase++;
                    if (debugLogging && r.baseSprite != null)
                    {
                        Debug.LogWarning("NeonEmotionManager: base sprite not found in sheetSprites. Neon=" + t.name + " sprite=" + r.baseSprite.name);
                    }
                }
                else
                {
                    // Normalize baseSprite to the one from sheetSprites (safe)
                    r.baseSprite = sheetSprites[r.baseSpriteIndex];
                }
            }

            // Cache base light state
            if (r.light2D != null)
            {
                r.baseLightColor = r.light2D.color;
                r.baseLightIntensity = r.light2D.intensity;
                r.hasLightBase = true;
            }

            runtime.Add(r);
        }

        if (debugLogging && missingBase > 0)
        {
            Debug.LogWarning("NeonEmotionManager: neons with base sprite not found in sheetSprites: " + missingBase);
        }
    }

    /* ===================== Public API (EmotionId) ===================== */

    public void ChangeNeonsToEmotion(EmotionId emotion)
    {
        int offset = EmotionToOffset(emotion);

        for (int i = 0; i < runtime.Count; i++)
        {
            ApplyEmotionToRuntime(runtime[i], emotion, offset);
        }
    }

    public void ResetNeonsToBase()
    {
        for (int i = 0; i < runtime.Count; i++)
        {
            ResetRuntimeToBase(runtime[i]);
        }
    }

    public void ChangeSingleNeonToEmotion(EmotionId emotion, int index)
    {
        if (index < 0 || index >= runtime.Count) return;
        int offset = EmotionToOffset(emotion);
        ApplyEmotionToRuntime(runtime[index], emotion, offset);
    }

    public void ResetSingleNeonToBase(int index)
    {
        if (index < 0 || index >= runtime.Count) return;
        ResetRuntimeToBase(runtime[index]);
    }

    /* ===================== Backward-compatible wrappers (string) ===================== */

    public void ChangeNeonsToEmotion(string emotionKey)
    {
        ChangeNeonsToEmotion(EmotionUtils.ToEmotionId(emotionKey));
    }

    public void RestoreNeonsToBase()
    {
        ResetNeonsToBase();
    }

    public void ChangeSingleNeonToEmotion(string emotionKey, int index)
    {
        ChangeSingleNeonToEmotion(EmotionUtils.ToEmotionId(emotionKey), index);
    }

    // keep your exact old name
    public void ResetSingleNeonTobase(int index)
    {
        ResetSingleNeonToBase(index);
    }

    /* ===================== Internals ===================== */

    private void ApplyEmotionToRuntime(NeonRuntime r, EmotionId emotion, int offset)
    {
        // Sprites
        if (r.sr != null)
        {
            Sprite target = GetVariantSprite(r, offset);
            if (target != null) r.sr.sprite = target;
        }
        if (r.uiImage != null)
        {
            Sprite target = GetVariantSprite(r, offset);
            if (target != null) r.uiImage.sprite = target;
        }

        // Lights
        if (affectLights && r.light2D != null)
        {
            r.light2D.color = GetLightColor(emotion);

            if (r.hasLightBase)
            {
                r.light2D.intensity = r.baseLightIntensity * intensityMultiplier;
            }
        }
    }

    private void ResetRuntimeToBase(NeonRuntime r)
    {
        if (r.sr != null)
        {
            if (r.baseSprite != null) r.sr.sprite = r.baseSprite;
        }
        if (r.uiImage != null)
        {
            if (r.baseSprite != null) r.uiImage.sprite = r.baseSprite;
        }

        if (affectLights && r.light2D != null && r.hasLightBase)
        {
            r.light2D.color = r.baseLightColor;
            r.light2D.intensity = r.baseLightIntensity;
        }
    }

    private int EmotionToOffset(EmotionId emotion)
    {
        // base = 0
        switch (emotion)
        {
            case EmotionId.Anger: return 1;
            case EmotionId.Fear: return 2;
            case EmotionId.Joy: return 3;
            case EmotionId.Sadness: return 4;
            default: return 0;
        }
    }

    private Color GetLightColor(EmotionId emotion)
    {
        switch (emotion)
        {
            case EmotionId.Anger: return angerLightColor;
            case EmotionId.Fear: return fearLightColor;
            case EmotionId.Joy: return joyLightColor;
            case EmotionId.Sadness: return sadnessLightColor;
            default: return joyLightColor;
        }
    }

    private Sprite GetVariantSprite(NeonRuntime r, int offset)
    {
        if (offset == 0) return r.baseSprite != null ? r.baseSprite : (r.sr != null ? r.sr.sprite : null);

        // 1) index-based if we have baseSpriteIndex
        if (r.baseSpriteIndex >= 0 && r.baseSpriteIndex < sheetSprites.Count)
        {
            int idx = r.baseSpriteIndex + offset;
            if (idx >= 0 && idx < sheetSprites.Count)
            {
                Sprite s = sheetSprites[idx];
                if (s != null) return s;
            }
        }

        // 2) name-based fallback: lights_123 -> lights_124, lights_125...
        Sprite baseS = r.baseSprite != null ? r.baseSprite : (r.sr != null ? r.sr.sprite : null);
        if (baseS != null)
        {
            Sprite byName = FindVariantByName(baseS.name, offset);
            if (byName != null) return byName;
        }

        return baseS;
    }

    private int FindSpriteIndex(Sprite s)
    {
        if (s == null) return -1;

        int idx;
        if (spriteNameToIndex.TryGetValue(s.name, out idx))
        {
            return idx;
        }

        // fallback linear search by reference or name
        for (int i = 0; i < sheetSprites.Count; i++)
        {
            Sprite a = sheetSprites[i];
            if (a == null) continue;
            if (a == s) return i;
            if (a.name == s.name) return i;
        }

        return -1;
    }

    private Sprite FindVariantByName(string baseName, int offset)
    {
        if (string.IsNullOrEmpty(baseName)) return null;

        int us = baseName.LastIndexOf('_');
        if (us <= 0 || us >= baseName.Length - 1) return null;

        string prefix = baseName.Substring(0, us + 1);
        string numStr = baseName.Substring(us + 1);

        int n;
        if (!int.TryParse(numStr, out n)) return null;

        string targetName = prefix + (n + offset);

        int idx;
        if (spriteNameToIndex.TryGetValue(targetName, out idx))
        {
            if (idx >= 0 && idx < sheetSprites.Count) return sheetSprites[idx];
        }

        // last fallback linear
        for (int i = 0; i < sheetSprites.Count; i++)
        {
            Sprite s = sheetSprites[i];
            if (s != null && s.name == targetName) return s;
        }

        return null;
    }
}
