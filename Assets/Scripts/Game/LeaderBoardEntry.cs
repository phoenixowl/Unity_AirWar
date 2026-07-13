using System;
using System.Collections.Generic;

[Serializable]
public class LeaderboardEntry
{
    public int score;
    public bool isWon;
    public string timestamp; // 存储可读的时间戳

    public LeaderboardEntry(int score, bool isWon, string timestamp)
    {
        this.score = score;
        this.isWon = isWon;
        this.timestamp = timestamp;
    }
}

// 核心：这个外壳类是专门用来绕过 JsonUtility 无法直接序列化 List 的限制
[Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
}