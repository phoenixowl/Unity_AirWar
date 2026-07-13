using UnityEngine;

[CreateAssetMenu(menuName = "Game/SettingConfig")]

public class SettingConfigSO : ScriptableObject
{
    public bool useMouseControl = false;
    public bool useAutoFire = false;
}