using UnityEngine;

[DefaultExecutionOrder(-100)]
public class ConfigManager : MonoBehaviour
{
    public static ConfigManager Instance { get; private set; }

    [SerializeField] StatsConfigSO statsConfigSO;
    [SerializeField] SettingConfigSO settingConfigSO;

    public StatsConfigSO StatsConfigSO => statsConfigSO;
    public SettingConfigSO SettingConfigSO => settingConfigSO;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        SaveSystem.Load(SettingConfigSO, "settings_config");
    }
}