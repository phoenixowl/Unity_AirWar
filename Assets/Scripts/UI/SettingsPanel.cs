using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingsPanel : MonoBehaviour
{

    [SerializeField] SettingConfigSO settingConfigSO;
    [SerializeField] Button backButton;
    [SerializeField] Toggle mouseControlToggle;
    [SerializeField] Toggle autoFireToggle;
    bool mouseControl = false;
    bool autoFire = false;

    void Start()
    {
        if (settingConfigSO && backButton && mouseControlToggle)
        {
            SaveSystem.Load(settingConfigSO, "settings_config");

            mouseControl = settingConfigSO.useMouseControl;
            mouseControlToggle.isOn = mouseControl;

            autoFire = settingConfigSO.useAutoFire;
            autoFireToggle.isOn = autoFire;

            mouseControlToggle.onValueChanged.AddListener(onMouseControlToggled);
            autoFireToggle.onValueChanged.AddListener(onAutoFireToggled);
            backButton.onClick.AddListener(onBackButtonClicked);
        }
    }

    void onMouseControlToggled(bool value)
    {
        mouseControl = value;
        settingConfigSO.useMouseControl = mouseControl;
    }
    void onAutoFireToggled(bool value)
    {
        autoFire = value;
        settingConfigSO.useAutoFire = autoFire;
    }

    void onBackButtonClicked()
    {
        SaveSystem.Save(settingConfigSO, "settings_config");
        SceneManager.LoadScene("StartMenuScene");
    }
}
