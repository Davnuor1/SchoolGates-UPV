using UnityEngine;

public class Maligor : MonoBehaviour
{
    public string nombreTagJugador = "Player"; // Tag asignado al jugador
    public float rangoDeteccion = 30f; // Rango de detección en unidades
    public float velocidad = 0.5f; // Velocidad de movimiento del demonio

    private Transform objetivo; // Transform del jugador
    private bool jugadorEnRango = false;
    private float tiempoBusqueda = 1f; // Tiempo entre búsquedas del jugador
    private float tiempoTranscurrido = 0f;

    void Update()
    {
        tiempoTranscurrido += Time.deltaTime;

        if (objetivo == null && tiempoTranscurrido >= tiempoBusqueda)
        {
            // Busca el jugador por tag
            GameObject jugadorEncontrado = GameObject.FindWithTag(nombreTagJugador);

            if (jugadorEncontrado != null)
            {
                objetivo = jugadorEncontrado.transform;
            }

            tiempoTranscurrido = 0f; // Resetea el tiempo de búsqueda
        }

        if (objetivo != null)
        {
            float distanciaAlJugador = Vector3.Distance(transform.position, objetivo.position);

            if (distanciaAlJugador <= rangoDeteccion)
            {
                jugadorEnRango = true;
            }
            else
            {
                jugadorEnRango = false;
            }

            if (jugadorEnRango)
            {
                PerseguirJugador();
            }
        }
    }

    void PerseguirJugador()
    {
        Vector3 direccion = (objetivo.position - transform.position).normalized;
        transform.position += direccion * velocidad * Time.deltaTime;
    }
}
