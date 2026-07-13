using System;
using System.IO;
using UnityEngine;

public static class LeaderboardManager
{
    private static string SavePath => Path.Combine(Application.persistentDataPath, "leaderboard.json");
    private const int MaxEntries = 100; // 排行榜最大容量

    // 1. 读取排行榜
    public static LeaderboardData LoadLeaderboard()
    {
        if (!File.Exists(SavePath))
        {
            // 如果文件不存在，返回一个干净的空排行榜对象
            return new LeaderboardData();
        }

        string json = File.ReadAllText(SavePath);
        // 使用 FromJson 反序列化出包裹了 List 的外壳类
        return JsonUtility.FromJson<LeaderboardData>(json);
    }

    // 2. 提交新分数
    public static void SubmitScore(int score, bool isWon)
    {
        // 先读取当前本地的所有排行
        LeaderboardData data = LoadLeaderboard();

        // 获取当前系统时间，并格式化为易读的字符串（例如：2026-07-13 19:30:15）
        string currentTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        LeaderboardEntry newEntry = new LeaderboardEntry(score, isWon, currentTime);

        // 将新记录追加入列表
        data.entries.Add(newEntry);

        // 核心步骤：排序（按分数从高到低）
        // 使用 Lambda 表达式，b.score.CompareTo(a.score) 表示降序
        data.entries.Sort((a, b) => b.score.CompareTo(a.score));

        // 核心步骤：裁剪（如果超过了 MaxEntries 名，删除末尾多余的低分记录）
        if (data.entries.Count > MaxEntries)
        {
            data.entries.RemoveRange(MaxEntries, data.entries.Count - MaxEntries);
        }

        // 重新写入硬盘
        string updatedJson = JsonUtility.ToJson(data, true);
        File.WriteAllText(SavePath, updatedJson);

        Debug.Log($"新分数 {score} 已提交，排行榜已更新并保存。");
    }

    // 3. 【可选】清空排行榜
    public static void ClearLeaderboard()
    {
        if (File.Exists(SavePath))
        {
            File.Delete(SavePath);
            Debug.Log("排行榜已清空。");
        }
    }
}