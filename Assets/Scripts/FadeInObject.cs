using UnityEngine;
using System.Collections;

public class FadeInObject : MonoBehaviour
{
    public float fadeDuration = 1.0f;  // Duración del fade in (en segundos)

    private Renderer objectRenderer;
    private Collider2D objectCollider;
    private Color objectColor;
    private TeleportPlayer teleportEscaleras;

    void Start()
    {
        objectRenderer = GetComponent<Renderer>();
        objectColor = objectRenderer.material.color;
        objectColor.a = 0f;  // Comienza completamente transparente
        objectRenderer.material.color = objectColor;

        objectCollider = GetComponent<EdgeCollider2D>();
        objectCollider.enabled=false;
        teleportEscaleras = GetComponentInChildren<TeleportPlayer>();
        teleportEscaleras.gameObject.SetActive(false);
    }

    public void FadeIn()
    {
        objectCollider.enabled = true;
        teleportEscaleras.gameObject.SetActive(true);
        StartCoroutine(FadeInCoroutine());
    }

    private IEnumerator FadeInCoroutine()
    {
        float elapsedTime = 0f;
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / fadeDuration);
            objectColor.a = alpha;
            objectRenderer.material.color = objectColor;
            yield return null;
        }

        // Asegurar que la alfa quede completamente en 1
        objectColor.a = 1f;
        objectRenderer.material.color = objectColor;
        

    }
}
