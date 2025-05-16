using UnityEngine;

public class Altar : MonoBehaviour
{
    public int altarNumber;
    public Transform objectSpawnPoint;

    private string placedChestID;
    private GameObject visualObject;

    [Header("Configuración de interacción")]
    public float interactionRadius = 1f;
    public LayerMask playerLayer;

    void Update()
    {
        if (VirtualInput.GetKeyDownE())
        {
            Collider2D hit = Physics2D.OverlapCircle(transform.position, interactionRadius, playerLayer);
            if (hit != null)
            {
                AgoraAltarManager.Instance.OpenAltarUI(this);
            }
        }
    }

    public void SetObject(string chestID, Sprite sprite)
    {
        placedChestID = chestID;

        visualObject = new GameObject("PlacedObject", typeof(SpriteRenderer));
        visualObject.transform.position = objectSpawnPoint.position;

        SpriteRenderer sr = visualObject.GetComponent<SpriteRenderer>();
        sr.sprite = sprite;
        sr.sortingOrder = 13;
    }

    public void ResetAltar()
    {
        placedChestID = null;

        if (visualObject != null)
        {
            Destroy(visualObject);
            visualObject = null;
        }
    }

    public string GetPlacedChestID() => placedChestID;

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
#endif
}
