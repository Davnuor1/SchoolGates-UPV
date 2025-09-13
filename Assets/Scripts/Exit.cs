using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Exit : MonoBehaviour
{
    public void CerrarJuego()
    {
        if (SceneManager.GetActiveScene().name != "LoginScene")
        {
            UserDataManager.Instance.SaveAndUpdateTime();
            StartCoroutine(SaveThenGoToLogin());
        }
        else
        {
            SceneManager.LoadScene("LoginScene");
        }
    }


    private IEnumerator SaveThenGoToLogin()
    {
        PixelCrushers.SaveSystem.SaveToSlot(1);
        yield return null;
        yield return null;
        SceneManager.LoadScene("LoginScene");

    }
}
