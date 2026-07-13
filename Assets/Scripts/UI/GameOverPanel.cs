using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverPanel : MonoBehaviour
{

    [SerializeField] Text overHintText;
    [SerializeField] Text scoreText;
    [SerializeField] Text btnText;
    [SerializeField] Button backtoLobbyButton;
    [SerializeField] Button retryButton;
    bool isWon = false;

    void Start()
    {
        backtoLobbyButton?.onClick.AddListener(onBacktoLobbyBtnClicked);
        retryButton?.onClick.AddListener(onRetryButtonClicked);
    }


    void OnEnable()
    {
        EventBus.Subscribe<GameOverEvent>(OnGameOver);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe<GameOverEvent>(OnGameOver);
    }

    void hide()
    {
        var cg = GetComponent<CanvasGroup>();
        cg.alpha = 0;
        cg.interactable = false;
        cg.blocksRaycasts = false;
    }

    void show()
    {
        var cg = GetComponent<CanvasGroup>();
        cg.alpha = 1;
        cg.interactable = true;
        cg.blocksRaycasts = true;
    }

    void OnGameOver(GameOverEvent e)
    {
        show();
        isWon = e.won;
        scoreText.text = "分数:" + Convert.ToString(ScoreManager.Instance.Score);
        if (e.won)
        {
            overHintText.text = "恭喜获胜";
            btnText.text = "再来一局";

        }
    }

    void onBacktoLobbyBtnClicked()
    {
        LeaderboardManager.SubmitScore(ScoreManager.Instance.Score, isWon);
        SceneManager.LoadScene("StartMenuScene");
    }

    void onRetryButtonClicked()
    {
        LeaderboardManager.SubmitScore(ScoreManager.Instance.Score, isWon);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
