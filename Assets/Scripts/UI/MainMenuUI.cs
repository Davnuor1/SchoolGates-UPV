using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainMenuUI : MonoBehaviour
{
    [SerializeField] private Button continueButton;
    [SerializeField] private int newGameSceneBuildIndex = 0;

    private void OnEnable()
    {
        // Habilita/deshabilita Continue según haya o no partida guardada en el slot 1.
        bool hasSave = PixelCrushers.SaveSystem.HasSavedGameInSlot(1);
        if (continueButton != null) continueButton.interactable = hasSave;
    }

    public void OnClick_NewGame()
    {
        // Borra el slot 1 y reinicia el juego cargando una escena inicial.
        PixelCrushers.SaveSystem.DeleteSavedGameInSlot(1);
        TryDeleteLocalUserDataIfAny();
        // Empezar desde la escena 0. No es necesario borrar el slot para funcionar;
        // al guardar más tarde, se sobreescribirá.
        SceneManager.LoadScene(newGameSceneBuildIndex);
    }

    public void OnClick_Continue()
    {
        if (PixelCrushers.SaveSystem.HasSavedGameInSlot(1))
        {
            // Carga la escena y aplica el estado guardado (con Save Current Scene activo).
            PixelCrushers.SaveSystem.LoadFromSlot(1);
        }
        else
        {
            Debug.Log("No hay partida guardada.");
        }
    }
    private void TryDeleteLocalUserDataIfAny()
    {
        // Si estas usando tu JSON local de pruebas por TAN:
        // Asegurate de que TANManager.CurrentTAN tenga valor en este punto (vienes del login).
#if !UNITY_WEBGL || UNITY_EDITOR
        if (!string.IsNullOrEmpty(TANManager.CurrentTAN))
        {
            LocalJsonSave.DeleteUserData(TANManager.CurrentTAN);
        }
#endif
    }
}
