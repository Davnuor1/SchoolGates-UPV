using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float speed = 4f;
    private Rigidbody2D myRigidbody;
    private Vector3 playerMovement;
    private Animator animator;

    [SerializeField] private SpriteRenderer spriteRendererBody;
    [SerializeField] private SpriteRenderer spriteRendererHair;
    [SerializeField] private SpriteRenderer spriteRendererTorso;
    [SerializeField] private SpriteRenderer spriteRendererLegs;

    private JoystickVirtual joystick;

    private void Start()
    {
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();

        spriteRendererBody = GetComponent<SpriteRenderer>();
        spriteRendererHair = GetComponent<SpriteRenderer>();
        spriteRendererTorso = GetComponent<SpriteRenderer>();
        spriteRendererLegs = GetComponent<SpriteRenderer>();

        if (DeviceDetector.isTouchDevice)
        {
            joystick = FindObjectOfType<JoystickVirtual>();
        }
    }

    private void FixedUpdate()
    {
        playerMovement = Vector3.zero;

        float horizontalInput;
        float verticalInput;

        // Prioridad: FrustratedInputOverride > Joystick > Teclado
        FrustratedInputOverride inputOverride = GetComponent<FrustratedInputOverride>();

        if (inputOverride != null)
        {
            horizontalInput = inputOverride.GetHorizontal();
            verticalInput = inputOverride.GetVertical();
        }
        else if (DeviceDetector.isTouchDevice && joystick != null)
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
            playerMovement.x = horizontalInput;
            playerMovement.y = 0;
        }
        else
        {
            playerMovement.x = 0;
            playerMovement.y = verticalInput;
        }

        UpdateAnimationAndMove();
    }

    private void UpdateAnimationAndMove()
    {
        if (playerMovement != Vector3.zero)
        {
            MoveCharacter();
            animator.SetFloat("moveX", playerMovement.x);
            animator.SetFloat("moveY", playerMovement.y);
            animator.SetBool("moving", true);
        }
        else
        {
            animator.SetBool("moving", false);
        }
    }

    private void MoveCharacter()
    {
        myRigidbody.MovePosition(transform.position + playerMovement * speed * Time.deltaTime);
    }

    private void AdjustSortingLayer()
    {
        spriteRendererBody.sortingOrder = (int)(transform.position.y * -32);
        spriteRendererHair.sortingOrder = (int)(transform.position.y * -32);
        spriteRendererTorso.sortingOrder = (int)(transform.position.y * -32);
        spriteRendererLegs.sortingOrder = (int)(transform.position.y * -32);
    }
}
