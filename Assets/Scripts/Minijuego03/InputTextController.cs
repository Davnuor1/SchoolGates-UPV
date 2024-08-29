using UnityEngine;
using TMPro;

public class InputTextController : MonoBehaviour
{
    public TMP_InputField inputField; // El campo de texto donde el usuario escribe
    public GameObject inputTextPanel; // El panel que contiene el campo de texto y el botón de enviar
    private string textoUsuario; // La variable donde almacenaremos el texto

    public void MostrarPanelDeTexto()
    {
        inputTextPanel.SetActive(true);
        inputField.text = ""; // Limpiar el campo de texto al mostrarlo
    }

    public void EnviarTexto()
    {
        textoUsuario = inputField.text;
        Debug.Log("Texto ingresado por el usuario: " + textoUsuario);

        CerrarPanelDeTexto();

        // Llamar a la función para manejar la selección múltiple
        FindObjectOfType<PreguntaUIController>().ManejarTextoEnviado();
    }

    public void CerrarPanelDeTexto()
    {
        inputTextPanel.SetActive(false);
    }

    public string ObtenerTextoUsuario()
    {
        return textoUsuario;
    }
}
