// Code written by tutmo (youtube.com/tutmo)
// For help, check out the tutorial - https://youtu.be/PNWK5o9l54w

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // ~~ 1. Controls All Player Movement
    // ~~ 2. Updates Animator to Play Idle & Walking Animations

    private float speed = 3f;
    private Rigidbody2D myRigidbody;
    private Vector3 playerMovement;
    private Animator animator;
    [SerializeField]  private SpriteRenderer spriteRendererBody;
    [SerializeField]  private SpriteRenderer spriteRendererHair;
    [SerializeField]  private SpriteRenderer spriteRendererTorso;
    [SerializeField]  private SpriteRenderer spriteRendererLegs;

    private void Start()
    {
        animator = GetComponent<Animator>();
        myRigidbody = GetComponent<Rigidbody2D>();

        spriteRendererBody = GetComponent<SpriteRenderer>();
        spriteRendererHair = GetComponent<SpriteRenderer>();
        spriteRendererTorso = GetComponent<SpriteRenderer>();
        spriteRendererLegs = GetComponent<SpriteRenderer>();
    }

    private void FixedUpdate()
    {
        playerMovement = Vector3.zero;
        //playerMovement.x = Input.GetAxisRaw("Horizontal");
        //playerMovement.y = Input.GetAxisRaw("Vertical");

        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
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
