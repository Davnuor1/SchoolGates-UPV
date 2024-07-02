using UnityEngine;

public class BoatController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private GameObject player;
    private bool playerOnBoard = false;
    private Animator animator;
    private Animator playerAnimator;
    private Rigidbody2D rb;
    private Collider2D boatCollider;

    private Vector2 boatMovement;

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boatCollider = GetComponent<Collider2D>(); // Obtener el collider de la barca
        rb.gravityScale = 0; // Ensure gravity scale is zero
    }

    void FixedUpdate()
    {
        if (playerOnBoard)
        {
            boatMovement = Vector2.zero;

            float horizontalInput = Input.GetAxisRaw("Horizontal");
            float verticalInput = Input.GetAxisRaw("Vertical");

            if (Mathf.Abs(horizontalInput) > Mathf.Abs(verticalInput))
            {
                boatMovement.x = horizontalInput;
                boatMovement.y = 0;
            }
            else
            {
                boatMovement.x = 0;
                boatMovement.y = verticalInput;
            }

            UpdateAnimationAndMove();
        }
        else
        {
            rb.velocity = Vector2.zero; // Stop the boat when the player is not on board
        }
    }

    void UpdateAnimationAndMove()
    {
        if (boatMovement != Vector2.zero)
        {
            MoveBoat();
            animator.SetFloat("MoveX", boatMovement.x);
            animator.SetFloat("MoveY", boatMovement.y);
            playerAnimator.SetFloat("moveX", boatMovement.x);
            playerAnimator.SetFloat("moveY", boatMovement.y);
            animator.SetBool("Moving", true);
        }
        else
        {
            animator.SetBool("Moving", false);
            rb.velocity = Vector2.zero; // Stop the boat when there is no input
        }
    }

    void MoveBoat()
    {
        Vector2 newPosition = rb.position + boatMovement * moveSpeed * Time.fixedDeltaTime;
        rb.MovePosition(newPosition);
    }

    public void SetPlayer(GameObject playerObject)
    {
        player = playerObject;
        playerOnBoard = true;
        player.transform.SetParent(transform);
        playerAnimator = player.GetComponent<Animator>();

        // Mantener la posición Z del jugador sin cambios y ajustar la posición Y a 0.8 unidades más arriba
        Vector3 playerPosition = player.transform.localPosition;
        player.transform.localPosition = new Vector3(0, 0.8f, playerPosition.z);
    }

    public void RemovePlayer()
    {
        player.transform.SetParent(null);
        playerOnBoard = false;
        rb.velocity = Vector2.zero; // Stop the boat when the player gets off
        this.enabled = false;
    }

    public void SetBoatCollider(bool isActive)
    {
        boatCollider.enabled = isActive; // Activar o desactivar el collider de la barca
    }
}
