using System.IO;
using UnityEngine;

public static class SaveSystem
{
    // 获取安全的持久化数据路径
    private static string GetSavePath(string fileName)
    {
        return Path.Combine(Application.persistentDataPath, fileName + ".json");
    }

    // 保存数据
    public static void Save(ScriptableObject dataObject, string fileName)
    {
        string json = JsonUtility.ToJson(dataObject, true); // true 表示格式化排版，方便人类阅读
        string path = GetSavePath(fileName);

        File.WriteAllText(path, json);
        Debug.Log($"游戏数据已保存至: {path}");
    }

    // 读取数据
    public static void Load(ScriptableObject dataObject, string fileName)
    {
        string path = GetSavePath(fileName);

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            // 核心：直接把 json 字符串的数据覆写到现有的 ScriptableObject 实例中
            JsonUtility.FromJsonOverwrite(json, dataObject);
            Debug.Log("游戏数据读取成功！");
        }
        else
        {
            Debug.LogWarning("未找到存档文件，将使用 ScriptableObject 的默认数据。");
        }
    }
}