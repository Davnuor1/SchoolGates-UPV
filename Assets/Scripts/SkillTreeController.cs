using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class SkillTreeController : MonoBehaviour
{
    //public static SkillTreeController Instance { get; private set; }

    [System.Serializable]
    public class SkillSlot
    {
        public string id;             // id único del skill (ej. "empathy", "calm_1", etc.)
        public Image slotFrame;       // imagen del cuadro/casilla
        public Image iconTarget;      // hijo donde pintar el icono
        public Sprite iconSprite;     // sprite del icono cuando está desbloqueado
    }

    [Header("Slots del árbol")]
    [SerializeField] private SkillSlot[] slots;

    [Header("Colores")]
    [SerializeField] private Color lockedFrameColor = new Color(1, 1, 1, 0.35f); // grisáceo
    [SerializeField] private Color unlockedFrameColor = Color.white;

    private Dictionary<string, SkillSlot> slotById;

    private void Awake()
    {
        //if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        //Instance = this;

        slotById = new Dictionary<string, SkillSlot>(slots.Length);
        foreach (var s in slots)
        {
            if (s != null && !string.IsNullOrEmpty(s.id))
            {
                slotById[s.id] = s;
            }
        }
    }

    private void OnEnable()
    {
        RefreshAllFromUserData();
    }

    // Refresca todos los slots leyendo UserData
    public void RefreshAllFromUserData()
    {
        if (UserDataManager.Instance == null || UserDataManager.Instance.currentUserData == null)
        {
            // Muestra todo bloqueado si no hay datos
            foreach (var s in slots) SetLockedVisual(s);
            return;
        }

        var unlocked = new HashSet<string>(UserDataManager.Instance.currentUserData.unlockedSkills ?? new string[0]);

        foreach (var s in slots)
        {
            if (s == null) continue;

            if (unlocked.Contains(s.id))
                SetUnlockedVisual(s);
            else
                SetLockedVisual(s);
        }
    }

    // Método público para desbloquear desde cualquier script
    public void Unlock(string skillId, bool saveNow = false, bool animate = true)
    {
        if (string.IsNullOrEmpty(skillId)) return;

        bool changed = UserDataManager.Instance.UnlockSkillId(skillId, saveNow);
        if (!changed)
        {
            // ya estaba desbloqueado; aseguramos UI
            if (slotById.TryGetValue(skillId, out var slotA))
                SetUnlockedVisual(slotA);
            return;
        }

        if (slotById.TryGetValue(skillId, out var slot))
        {
            SetUnlockedVisual(slot);
            if (animate) PlayUnlockAnim(slot);
        }
    }

    private void SetLockedVisual(SkillSlot s)
    {
        if (s == null) return;
        if (s.slotFrame != null) s.slotFrame.color = lockedFrameColor;
        if (s.iconTarget != null)
        {
            s.iconTarget.enabled = false;
            s.iconTarget.sprite = null;
        }
    }

    private void SetUnlockedVisual(SkillSlot s)
    {
        if (s == null) return;
        if (s.slotFrame != null) s.slotFrame.color = unlockedFrameColor;
        if (s.iconTarget != null)
        {
            s.iconTarget.enabled = true;
            s.iconTarget.sprite = s.iconSprite;
        }
    }

    private void PlayUnlockAnim(SkillSlot s)
    {
        if (s == null || s.iconTarget == null) return;
        var rt = s.iconTarget.rectTransform;
        StopAllCoroutines();
        StartCoroutine(Punch(rt, 1.0f, 1.15f, 0.15f));
    }

    private System.Collections.IEnumerator Punch(RectTransform rt, float from, float to, float dur)
    {
        float t = 0f;
        rt.localScale = Vector3.one * from;
        while (t < dur)
        {
            t += Time.unscaledDeltaTime;
            float k = t / dur;
            float s = Mathf.Lerp(from, to, k);
            rt.localScale = Vector3.one * s;
            yield return null;
        }
        // vuelta rápida a 1
        float backDur = 0.12f;
        t = 0f;
        while (t < backDur)
        {
            t += Time.unscaledDeltaTime;
            float k = t / backDur;
            float s = Mathf.Lerp(to, 1f, k);
            rt.localScale = Vector3.one * s;
            yield return null;
        }
        rt.localScale = Vector3.one;
    }
}
