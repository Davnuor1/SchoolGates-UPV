using UnityEngine;

public class MapaInteractivo : MonoBehaviour
{
    // Referencia al panel que se activará
    public GameObject panelMapa;
    // Referencia al jugador
    private GameObject player;
    // Distancia mínima para activar el panel
    public float distanciaActivacion = 1f;

    void Start()
    {
        // Intentar encontrar al jugador al inicio
        FindPlayer();
    }

    void Update()
    {
        // Si no se ha encontrado el jugador, intentar encontrarlo
        if (player == null)
        {
            FindPlayer();
            return;  // No seguir ejecutando si aún no hay jugador
        }

        if (player != null)
        {
            // Calcular la distancia entre el jugador y el objeto que representa el mapa
            float distancia = Vector2.Distance(transform.position, player.transform.position);
            //Debug.Log(distancia);

            // Si la distancia es menor o igual a la distancia de activación y se presiona la tecla "E"
            if (distancia <= distanciaActivacion && Input.GetKeyDown(KeyCode.E))
            {
                // Activar o desactivar el panel del mapa
                Debug.Log("entramos tercer if");
                panelMapa.SetActive(!panelMapa.activeSelf);
            }
        }
    }
    void FindPlayer()
    {
        // Buscar al jugador por su tag
        player = GameObject.FindGameObjectWithTag("Player");

        if (player == null)
        {
            Debug.LogWarning("Jugador no encontrado. Asegúrate de que el objeto jugador tiene el tag 'Player'.");
        }
    }
}
