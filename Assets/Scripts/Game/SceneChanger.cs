using UnityEngine;
using UnityEngine.SceneManagement; // ← 必须引这个

public class SceneChanger : MonoBehaviour
{
    public void GoToGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}