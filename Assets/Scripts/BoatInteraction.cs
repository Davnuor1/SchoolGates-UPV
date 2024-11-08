using UnityEngine;

public class BoatInteraction : MonoBehaviour
{
    public float interactionRange = 2f;
    public GameObject boat;
    public BoatController boatController;
    public GameObject puntoAnclaje; // Referencia al PuntoAnclaje

    private GameObject player;
    private bool isPlayerOnBoat = false;
    private Rigidbody2D playerRigidbody;
    private BoxCollider2D playerCollider;
    private Animator playerAnimator;

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
        }

        if (player != null)
        {
            if (!isPlayerOnBoat)
            {
                CheckForBoatInteraction();
            }
            else
            {
                if (Input.GetKeyDown(KeyCode.E)) // Tecla para bajar de la barca
                {
                    TryGetOffBoat();
                }
            }
        }
    }

    void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player"); // Asegúrate de que tu jugador tenga la etiqueta "Player"
        if (player != null)
        {
            playerRigidbody = player.GetComponent<Rigidbody2D>();
            playerCollider = player.GetComponent<BoxCollider2D>();
            playerAnimator = player.GetComponent<Animator>();
        }
    }

    void CheckForBoatInteraction()
    {
        if (Input.GetKeyDown(KeyCode.E)) // Clic izquierdo
        {
            Vector2 clickPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            float distanceToBoat = Vector2.Distance(player.transform.position, boat.transform.position);
            if (distanceToBoat <= interactionRange)
            {
                GetOnBoat();
            }
        }
    }

    void GetOnBoat()
    {
        isPlayerOnBoat = true;
        boatController.enabled = true;
        boatController.SetPlayer(player);

        if (player.TryGetComponent<PlayerMovement>(out var playerMovement))
        {
            playerMovement.enabled = false; // Desactiva el script de movimiento del jugador
        }

        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = true; // Desactiva las físicas del jugador
            playerRigidbody.velocity = Vector2.zero; // Detiene cualquier movimiento
        }

        if (playerCollider != null)
        {
            playerCollider.enabled = false; // Desactiva el collider del jugador
        }

        boatController.SetBoatCollider(true); // Activa el collider de la barca cuando el jugador se sube

        // Forzar animación idle del jugador
        if (playerAnimator != null)
        {
            playerAnimator.SetBool("moving", false);

            // Mantener la última dirección en la animación idle
            float lastMoveX = playerAnimator.GetFloat("moveX");
            float lastMoveY = playerAnimator.GetFloat("moveY");

            // Asegurar que solo queda en estado idle con la última dirección de movimiento
            playerAnimator.SetFloat("moveX", lastMoveX);
            playerAnimator.SetFloat("moveY", lastMoveY);
        }
    }

    void TryGetOffBoat()
    {
        // Verificar que el PuntoAnclaje esté dentro de 4 unidades de distancia
        float distanceToAnchor = Vector2.Distance(boat.transform.position, puntoAnclaje.transform.position);

        if (distanceToAnchor <= 4f)
        {
            GetOffBoat();
        }
    }

    void GetOffBoat()
    {
        isPlayerOnBoat = false;
        boatController.RemovePlayer();

        if (player.TryGetComponent<PlayerMovement>(out var playerMovement))
        {
            playerMovement.enabled = true; // Reactiva el script de movimiento del jugador
        }

        if (playerRigidbody != null)
        {
            playerRigidbody.isKinematic = false; // Reactiva las físicas del jugador
        }

        if (playerCollider != null)
        {
            playerCollider.enabled = true; // Reactiva el collider del jugador
        }

        boatController.SetBoatCollider(false); // Desactiva el collider de la barca cuando el jugador se baja

        // Colocar al jugador en la posición del PuntoAnclaje
        player.transform.position = puntoAnclaje.transform.position;
    }
}
