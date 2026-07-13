using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LeaderboardPanel : MonoBehaviour
{
    [SerializeField] private GameObject rowPrefab;
    [SerializeField] private Transform contentParent;
    [SerializeField] Button returnButton;

    // 可以在打开面板时自动刷新
    private void OnEnable()
    {
        RefreshLeaderboardUI();
    }

    void Start()
    {
        returnButton.onClick.AddListener(() => SceneManager.LoadScene("StartMenuScene"));
    }
    /// <summary>
    /// 核心方法：刷新排行榜界面
    /// </summary>
    public void RefreshLeaderboardUI()
    {
        // 步骤 1：安全清空老旧的 UI 节点，防止重复打开时列表无限叠加
        // 注意：这里用倒序循环删除，避免正序删除导致的索引塌陷 Bug
        for (int i = contentParent.childCount - 1; i >= 0; i--)
        {
            Destroy(contentParent.GetChild(i).gameObject);
        }

        // 步骤 2：调用之前的管理器，从本地读取最新的 LeaderboardData
        LeaderboardData data = LeaderboardManager.LoadLeaderboard();

        // 步骤 3：遍历数据，动态生成 UI
        for (int i = 0; i < data.entries.Count; i++)
        {
            LeaderboardEntry entry = data.entries[i];

            // 实例化预制体，并直接认 Content 为它的“亲生父亲”
            GameObject newRow = Instantiate(rowPrefab, contentParent);

            // 获取预制体身上的控制脚本
            LeaderboardRow rowUI = newRow.GetComponent<LeaderboardRow>();

            if (rowUI != null)
            {
                // 将数据灌入这一行
                rowUI.SetData(entry.score, entry.isWon, entry.timestamp);
            }
        }
    }
}