using UnityEngine;
using UnityEngine.SceneManagement; // ← 必须引这个

public class SceneChanger : MonoBehaviour
{
    public void GoToGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void BackToTitle()
    {
        SceneManager.LoadScene("StartMenuScene");
    }

    // 重开当前场景（GameOver 时可用）
    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}