using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TANManager : MonoBehaviour
{
    public TMP_InputField tanInputField;

    public static string CurrentTAN = ""; // Accesible globalmente

    public void ConfirmTAN()
    {
        string tan = tanInputField.text.Trim();

        if (!string.IsNullOrEmpty(tan))
        {
            CurrentTAN = tan;
            Debug.Log("TAN guardado: " + CurrentTAN);

            // Aquí podrías cargar datos más adelante

            SceneManager.LoadScene(8); // <- cambia esto
        }
        else
        {
            Debug.LogWarning("Introduce un TAN válido");
        }
    }
}
