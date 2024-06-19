using UnityEngine;

public class BoatController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private GameObject player;
    private bool playerOnBoard = false;
    private Animator animator;
    private Animator playerAnimator;

    private Vector3 boatMovement;

    void Start()
    {
        animator = GetComponent<Animator>();
        
    }

    void FixedUpdate()
    {
        if (playerOnBoard)
        {
            boatMovement = Vector3.zero;

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
    }

    void UpdateAnimationAndMove()
    {
        if (boatMovement != Vector3.zero)
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
        }
    }

    void MoveBoat()
    {
        transform.Translate(boatMovement * moveSpeed * Time.fixedDeltaTime);
    }

    public void SetPlayer(GameObject playerObject)
    {
        player = playerObject;
        playerOnBoard = true;
        player.transform.SetParent(transform);
        playerAnimator = player.GetComponent<Animator>();

        // Mantener la posición Z del jugador sin cambios y ajustar la posición Y a 0.4 unidades más arriba
        Vector3 playerPosition = player.transform.localPosition;
        player.transform.localPosition = new Vector3(0, 0.4f, playerPosition.z);
    }

    public void RemovePlayer()
    {
        player.transform.SetParent(null);
        playerOnBoard = false;
        this.enabled = false;
    }
}
