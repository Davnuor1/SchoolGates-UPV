using UnityEngine;

public class BoatInteraction : MonoBehaviour
{
    public float interactionRange = 2f;
    public GameObject boat;
    public BoatController boatController;

    private GameObject player;
    private bool isPlayerOnBoat = false;

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
    }

    void CheckForBoatInteraction()
    {
        if (Input.GetMouseButtonDown(0)) // Clic izquierdo
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
        Debug.Log("desactivando movimiento jugador fueraaa");
        if (player.TryGetComponent<PlayerMovement>(out var playerMovement))
        {
            Debug.Log("desactivando movimiento jugador");
            playerMovement.enabled = false; // Desactiva el script de movimiento del jugador
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
    }
}
