using UnityEngine;
using System.Collections;
using PixelCrushers.DialogueSystem;

public class DisableOnDSBool : MonoBehaviour
{
    [Tooltip("Nombre de la variable bool del Dialogue System.")]
    public string variableName = "Miniquest07Completed";

    [Tooltip("Objeto a desactivar. Si está vacío, se usa este mismo GameObject.")]
    public GameObject target;

    [Tooltip("Cada cuántos segundos comprobar la variable.")]
    public float pollInterval = 0.2f;

    [Tooltip("Si es true, al desactivar una vez deja de comprobar.")]
    public bool oneShot = true;

    private void Awake()
    {
        if (target == null) target = gameObject;
    }

    private void OnEnable()
    {
        StartCoroutine(WatchVariable());
    }

    private IEnumerator WatchVariable()
    {
        // Espera a que Dialogue System esté listo.
        while (DialogueManager.instance == null) yield return null;

        var wait = new WaitForSeconds(pollInterval);
        while (enabled && target != null)
        {
            bool v = DialogueLua.GetVariable(variableName).asBool;
            if (v)
            {
                if (target.activeSelf) target.SetActive(false);
                if (oneShot) yield break;
            }
            yield return wait;
        }
    }
}
