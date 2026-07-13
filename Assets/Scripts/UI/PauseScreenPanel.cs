using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseScreenPanel : MonoBehaviour
{
    [SerializeField] public Button resumeButton;
    [SerializeField] public Button backtoLobbyButton;

    void Start()
    {
        resumeButton?.onClick.AddListener(() => EventBus.Emit(new GamePauseEvent(false)));
        backtoLobbyButton?.onClick.AddListener(() => SceneManager.LoadScene("StartMenuScene"));
    }

    void OnEnable()
    {
        EventBus.Subscribe<GamePauseEvent>(OnGamePaused);
        EventBus.Subscribe<GameOverEvent>(OnGameOver);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe<GamePauseEvent>(OnGamePaused);
        EventBus.Unsubscribe<GameOverEvent>(OnGameOver);
    }

    void hide()
    {
        var cg = GetComponent<CanvasGroup>();
        cg.alpha = 0;              // 全子 UI 透明
        cg.interactable = false;   // 不可交互
        cg.blocksRaycasts = false; // 不挡射线
    }

    void show()
    {
        var cg = GetComponent<CanvasGroup>();
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    void OnGamePaused(GamePauseEvent e)
    {
        if (e.pause)
        {
            show();
        }
        else
        {
            hide();
        }
    }

    void OnGameOver(GameOverEvent e)
    {
        gameObject.SetActive(false);
    }
}
