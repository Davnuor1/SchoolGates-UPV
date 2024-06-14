using UnityEngine;

public class BoatController : MonoBehaviour
{
    public float moveSpeed = 5f;
    private GameObject player;
    private bool playerOnBoard = false;

    void Update()
    {
        if (playerOnBoard)
        {
            BoatMovement();
        }
    }

    void BoatMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");
        Vector3 movement = new Vector3(moveHorizontal, moveVertical, 0f);
        transform.Translate(movement * moveSpeed * Time.deltaTime);
    }

    public void SetPlayer(GameObject playerObject)
    {
        player = playerObject;
        playerOnBoard = true;
        player.transform.SetParent(transform);
        player.transform.localPosition = Vector3.zero; // Ajusta según sea necesario para colocar al jugador en la barca
    }

    public void RemovePlayer()
    {
        player.transform.SetParent(null);
        playerOnBoard = false;
        this.enabled = false;
    }
}
