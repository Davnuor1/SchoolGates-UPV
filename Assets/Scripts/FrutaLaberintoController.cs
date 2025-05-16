using UnityEngine;

public class FrutaLaberintoController : MonoBehaviour
{
    private Transform player;
    public float followSpeed = 50f;
    public float followDistance = 1f; // Distancia deseada entre la fruta y el jugador
    public bool isFollowing = false;
    private Vector3 velocity = Vector3.zero;

    private static FrutaLaberintoController frutaActualSiguiendo = null;


    private void Update()
    {
        if (isFollowing && player != null)
        {
            // Calcular la distancia actual entre la fruta y el jugador
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            // Calcular la posición objetivo
            Vector3 targetPosition = player.position - (player.position - transform.position).normalized * followDistance;

            // Mover la fruta hacia la posición objetivo con una velocidad específica
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, followSpeed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && frutaActualSiguiendo == null)
        {
            player = other.transform;
            isFollowing = true;
            frutaActualSiguiendo = this;
        }

    }
    public void DeactivateFruit()
    {
        isFollowing = false;
        if (frutaActualSiguiendo == this)
        {
            frutaActualSiguiendo = null;
        }
        gameObject.SetActive(false); // Desactivar la fruta
    }
}
