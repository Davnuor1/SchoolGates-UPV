using UnityEngine;

public class BoatInteraction : MonoBehaviour
{
    public float interactionRange = 2f;
    public GameObject boat;
    public BoatController boatController;

    private GameObject player;
    private bool isPlayerOnBoat = false;
    private Rigidbody2D playerRigidbody;
    private BoxCollider2D playerCollider;

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
                    GetOffBoat();
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
    }
}
