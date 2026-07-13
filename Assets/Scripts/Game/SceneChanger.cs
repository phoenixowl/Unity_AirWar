using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanger : MonoBehaviour
{
    public void GoToGame()
    {
        SceneManager.LoadScene("GameScene");
    }
}