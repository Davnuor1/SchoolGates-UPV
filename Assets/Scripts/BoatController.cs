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
    private JoystickVirtual joystick; //  NUEVO

    void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        boatCollider = GetComponent<Collider2D>();
        rb.gravityScale = 0;

        // Detectar joystick si estamos en tablet
        if (DeviceDetector.isTouchDevice)
        {
            joystick = FindObjectOfType<JoystickVirtual>();
        }
    }

    void FixedUpdate()
    {
        if (playerOnBoard)
        {
            boatMovement = Vector2.zero;

            float horizontalInput;
            float verticalInput;

            if (DeviceDetector.isTouchDevice && joystick != null)
            {
                horizontalInput = joystick.Horizontal();
                verticalInput = joystick.Vertical();
            }
            else
            {
                horizontalInput = Input.GetAxisRaw("Horizontal");
                verticalInput = Input.GetAxisRaw("Vertical");
            }

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
            rb.velocity = Vector2.zero;
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
            rb.velocity = Vector2.zero;
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

        Vector3 playerPosition = player.transform.localPosition;
        player.transform.localPosition = new Vector3(0, 0.8f, playerPosition.z);
    }

    public void RemovePlayer()
    {
        player.transform.SetParent(null);
        playerOnBoard = false;
        rb.velocity = Vector2.zero;
        this.enabled = false;
    }

    public void SetBoatCollider(bool isActive)
    {
        boatCollider.enabled = isActive;
    }
}
