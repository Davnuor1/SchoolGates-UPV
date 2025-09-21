using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class Exit : MonoBehaviour
{
    public void CerrarJuego()
    {
        if (SceneManager.GetActiveScene().name != "LoginScene")
        {
            // 1) Actualiza tiempo + snapshot DS + etc. (ya lo haces dentro)
            UserDataManager.Instance.SaveAndUpdateTime();

            // 2) Lanza la corutina que sube a Sheets y luego vuelve al login
            StartCoroutine(SaveThenGoToLogin());
        }
        else
        {
            SceneManager.LoadScene("LoginScene");
        }
    }

    private IEnumerator SaveThenGoToLogin()
    {
        // 2.a) Guardado local (Pixel Crushers)
        PixelCrushers.SaveSystem.SaveToSlot(1);

        // 2.b) Construir DTO y enviar a Sheets
        var udm = UserDataManager.Instance;
        var dto = SnapshotBuilder.FromUserData(udm.currentUserData);
        string tan = udm.currentUserData.tan;

        bool finished = false;
        yield return SheetsService.Instance.StartCoroutine(
            SheetsService.Instance.SaveAsync(tan, dto, res =>
            {
                if (!res.ok) Debug.LogWarning("Save online fallido (cacheado para reintentar): " + res.error);
                finished = true;
            })
        );

        // 2.c) Volver al login
        SceneManager.LoadScene("LoginScene");
    }
}
