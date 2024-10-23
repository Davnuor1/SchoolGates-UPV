using UnityEngine;

public class VendingMachine : MonoBehaviour
{
    public float interactionDistance = 1.5f;
    private Player player;
    private BoxCollider2D boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
        if (boxCollider == null)
        {
            Debug.LogError("No BoxCollider2D found on the vending machine!");
        }
    }

    private void Update()
    {
        if (player == null)
        {
            player = FindObjectOfType<Player>();
        }

        if (player != null && Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(mousePosition, Vector2.zero);

            if (hit.collider != null && hit.collider == boxCollider)
            {
                TryInteract();
            }
        }
    }

    private void TryInteract()
    {
        if (player == null) return;

        float distance = Vector2.Distance(player.transform.position, transform.position);
        if (distance <= interactionDistance)
        {
            GameManager.instance.statsManager.ModifyEnergy(20);
            //GameManager.instance.statsManager.SetKarma(37);
            Debug.Log("Interacted with vending machine: Energy +20, Karma set to 37");
        }
    }
}
