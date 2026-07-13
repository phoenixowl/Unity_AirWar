#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

public class DebugTools
{
    [MenuItem("Tools/打开存档文件夹")]
    public static void OpenPersistentDataPath()
    {
        // 自动调用系统文件管理器打开该路径
        EditorUtility.RevealInFinder(Application.persistentDataPath);
    }
}
#endif