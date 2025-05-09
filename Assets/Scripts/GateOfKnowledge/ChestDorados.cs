using UnityEngine;

public class ChestDorados : MonoBehaviour
{
    public string chestID;
    private bool playerInRange;
    private bool hasBeenOpened = false;

    private Collider2D myCollider;

    void Awake()
    {
        myCollider = GetComponent<Collider2D>();
    }

    void Update()
    {
        if (playerInRange && !hasBeenOpened && Input.GetKeyDown(KeyCode.E))
        {
            hasBeenOpened = true;
            if (myCollider != null)
                myCollider.enabled = false;

            AgoraChestManager.Instance.OpenChest(chestID);
            AgoraChestManager.Instance.NotifyChestOpened();
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && Vector3.Distance(other.transform.position, transform.position) < 2.5f)
        {
            playerInRange = true;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
