using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InputTextController : MonoBehaviour
{
    public TMP_InputField inputField;
    public Button enviarButton;

    private string textoUsuario;

    void Start()
    {
        enviarButton.onClick.AddListener(EnviarTexto);
    }

    public void EnviarTexto()
    {
        textoUsuario = inputField.text;
        // Aquí puedes hacer lo que quieras con el texto, por ejemplo, almacenarlo para usarlo más adelante
        CerrarInput();
    }

    public void CerrarInput()
    {
        // Ocultamos o desactivamos la UI de entrada de texto
        gameObject.SetActive(false);
    }
}
