using UnityEngine;

public class ConfigManager : MonoBehaviour
{
    public static ConfigManager Instance { get; private set; }

    [SerializeField] StatsConfigSO statsConfigSO;

    public StatsConfigSO StatsConfigSO => statsConfigSO;

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