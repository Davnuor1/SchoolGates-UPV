using UnityEngine;

public class SpikeController : MonoBehaviour
{
    public GameObject spikeParent; // Referencia al objeto padre que agrupa los pinchos
    public GameObject maligorParent; // Referencia al objeto padre que agrupa los maligores
    public Sprite submergedSprite; // Sprite cuando está totalmente sumergido
    public Sprite halfEmergingSprite; // Sprite cuando está medio emergido
    public Sprite emergedSprite; // Sprite cuando está totalmente emergido
    public Sprite maligorDormidoSprite; // Sprite para los maligores dormidos
    public Sprite maligorMedioDespiertoSprite; // Sprite para los maligores medio despiertos
    public Sprite maligorDespiertoSprite; // Sprite para los maligores despiertos

    public float vibrationIntensity = 0.1f; // Intensidad de la vibración
    public float vibrationFrequency = 0.05f; // Frecuencia del cambio de posición durante la vibración

    private SpriteRenderer[] spikeRenderers;
    private EdgeCollider2D[] spikeColliders;
    private BoxCollider2D[] spikeCollidersTrigger; // Array para el segundo collider trigger
    private SpriteRenderer[] maligorRenderers;
    private Vector3[] originalPositions; // Posiciones originales de los maligores
    private float timeCounter = 0f;
    private bool isEmerging = false;
    private bool isFullyEmerged = false;
    private bool isVibrating = false;

    private void Start()
    {
        if (spikeParent == null || maligorParent == null)
        {
            Debug.LogError("Spike Parent o Maligor Parent no están asignados en el inspector.");
            return;
        }

        // Obtiene todos los SpriteRenderer y BoxCollider2D de los pinchos hijos del objeto spikeParent
        spikeRenderers = spikeParent.GetComponentsInChildren<SpriteRenderer>();
        spikeColliders = spikeParent.GetComponentsInChildren<EdgeCollider2D>();
        spikeCollidersTrigger = spikeParent.GetComponentsInChildren<BoxCollider2D>();

        // Filtra los colliders para diferenciar entre los colliders normales y los triggers
        spikeColliders = System.Array.FindAll(spikeColliders, c => !c.isTrigger);
        spikeCollidersTrigger = System.Array.FindAll(spikeCollidersTrigger, c => c.isTrigger);

        // Obtiene todos los SpriteRenderer de los maligores hijos del objeto maligorParent
        maligorRenderers = maligorParent.GetComponentsInChildren<SpriteRenderer>();
        originalPositions = new Vector3[maligorRenderers.Length];

        for (int i = 0; i < maligorRenderers.Length; i++)
        {
            originalPositions[i] = maligorRenderers[i].transform.localPosition;
        }

        SetSprite(submergedSprite);
        SetMaligorSprite(maligorDormidoSprite);
        SetSpikeState(false); // Comienza sumergido
    }

    private void Update()
    {
        if (spikeParent == null || maligorParent == null) return;

        timeCounter += Time.deltaTime;

        if (!isEmerging && !isFullyEmerged && timeCounter >= 8f && !isVibrating)
        {
            // Inicia la vibración 2 segundos antes de emerger
            StartCoroutine(VibrateMaligores());
        }

        if (!isEmerging && !isFullyEmerged && timeCounter >= 10f)
        {
            StopVibration();
            StartCoroutine(Emerge());
        }
        else if (isFullyEmerged && timeCounter >= 3f)
        {
            StartCoroutine(Submerge());
        }
    }

    private System.Collections.IEnumerator Emerge()
    {
        isEmerging = true;
        timeCounter = 0f;

        // Cambia el sprite a medio emergido para los pinchos y maligores
        SetSprite(halfEmergingSprite);
        SetMaligorSprite(maligorMedioDespiertoSprite);
        yield return new WaitForSeconds(0.25f);

        // Cambia el sprite a totalmente emergido y activa los colliders
        SetSprite(emergedSprite);
        SetSpikeState(true);
        SetMaligorSprite(maligorDespiertoSprite);
        isEmerging = false;
        isFullyEmerged = true;
    }

    private System.Collections.IEnumerator Submerge()
    {
        isEmerging = true;
        timeCounter = 0f;

        // Cambia el sprite a medio emergido para los pinchos y maligores
        SetSprite(halfEmergingSprite);
        SetMaligorSprite(maligorMedioDespiertoSprite);
        yield return new WaitForSeconds(0.25f);

        // Cambia el sprite a totalmente sumergido y desactiva los colliders
        SetSprite(submergedSprite);
        SetSpikeState(false);
        SetMaligorSprite(maligorDormidoSprite);
        isEmerging = false;
        isFullyEmerged = false;
    }

    private System.Collections.IEnumerator VibrateMaligores()
    {
        isVibrating = true;
        while (isVibrating)
        {
            for (int i = 0; i < maligorRenderers.Length; i++)
            {
                Vector3 randomOffset = Random.insideUnitSphere * vibrationIntensity;
                maligorRenderers[i].transform.localPosition = originalPositions[i] + randomOffset;
            }
            yield return new WaitForSeconds(vibrationFrequency);
        }
    }

    private void StopVibration()
    {
        isVibrating = false;
        // Restablecer posiciones originales
        for (int i = 0; i < maligorRenderers.Length; i++)
        {
            maligorRenderers[i].transform.localPosition = originalPositions[i];
        }
    }

    private void SetSprite(Sprite newSprite)
    {
        foreach (var renderer in spikeRenderers)
        {
            renderer.sprite = newSprite;
        }
    }

    private void SetMaligorSprite(Sprite newSprite)
    {
        foreach (var renderer in maligorRenderers)
        {
            renderer.sprite = newSprite;
        }
    }

    private void SetSpikeState(bool isActive)
    {
        foreach (var collider in spikeColliders)
        {
            collider.enabled = isActive;
        }

        foreach (var trigger in spikeCollidersTrigger)
        {
            trigger.enabled = isActive;
        }

        foreach (var renderer in spikeRenderers)
        {
            renderer.sortingOrder = isActive ? 12 : 11;
        }
    }
}
