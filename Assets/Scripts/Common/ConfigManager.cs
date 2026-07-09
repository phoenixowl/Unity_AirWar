using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    public static ConfigManager Instance { get; private set; }

    [SerializeField] GameConfigSO gameConfig;

    public GameConfigSO GameConfig => gameConfig;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
}