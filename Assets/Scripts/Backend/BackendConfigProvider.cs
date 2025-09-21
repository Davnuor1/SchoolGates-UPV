using UnityEngine;

public class BackendConfigProvider : MonoBehaviour
{
    public static BackendConfigProvider Instance { get; private set; }

    [SerializeField] private BackendConfig config;
    public BackendConfig Config => config;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }
}
