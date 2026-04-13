using UnityEngine;

public class LocalBuildLoginBypass : MonoBehaviour
{
    [Header("Panels")]
    public GameObject loginPanel;
    public GameObject menuPanel;

    [Header("Opcional: desactivar el script que controla el login online")]
    public MonoBehaviour loginControllerToDisable;

    private void Start()
    {
        var cfg = BackendConfigProvider.Instance != null ? BackendConfigProvider.Instance.Config : null;
        bool offline = cfg != null && cfg.offlineBuild;

        if (!offline) return;

        if (loginControllerToDisable != null)
            loginControllerToDisable.enabled = false;

        if (loginPanel != null) loginPanel.SetActive(false);
        if (menuPanel != null) menuPanel.SetActive(true);
    }
}