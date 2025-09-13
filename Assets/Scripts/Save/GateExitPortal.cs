using UnityEngine;
using UnityEngine.SceneManagement;

public class GateExitPortal : MonoBehaviour
{
    public string sceneToLoad;

    public void OnUsePortal()
    {
        if (UserDataManager.Instance != null)
        {
            UserDataManager.Instance.EndGateSession(true);
        }
        SceneManager.LoadScene(sceneToLoad);
    }
}
