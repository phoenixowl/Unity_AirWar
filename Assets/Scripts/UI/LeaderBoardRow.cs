using UnityEngine;
using TMPro;
using UnityEngine.UI; // 如果是老版 Text，改为 using UnityEngine.UI;

public class LeaderboardRow : MonoBehaviour
{
    [SerializeField] private Text scoreText;
    [SerializeField] private Text timeText;
    [SerializeField] private Text isWonText;
    /// <summary>
    /// 外部调用此方法来填充这一行的数据
    /// </summary>
    public void SetData(int score, bool isWon, string time)
    {
        scoreText.text = score.ToString();
        isWonText.text = isWon ? "是" : "否";
        timeText.text = time;
    }
}