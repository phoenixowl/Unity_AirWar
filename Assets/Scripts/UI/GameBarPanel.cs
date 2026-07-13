using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameBarPanel : MonoBehaviour
{
    [SerializeField] Text healthText;
    [SerializeField] Text scoreText;
    [SerializeField] Text gameText;
    [SerializeField] Slider gameProgressSlider;
    [SerializeField] Button pauseButton;

    void Start()
    {
        pauseButton?.onClick.AddListener(() => EventBus.Emit(new GamePauseEvent(!GameManager.Instance.isGamePausing)));
    }

    void OnEnable()
    {
        EventBus.Subscribe<PlayerHitEvent>(OnPlayerHit);
        EventBus.Subscribe<PlayerHealEvent>(OnPlayerHeal);
        EventBus.Subscribe<ScoreChangedEvent>(OnScoreChanged);
    }

    void Update()
    {
        gameText.text = "GameSpeed: " + Convert.ToString(GameStateManager.Instance.GameSpeed) + " GameState : " + Convert.ToString(GameStateManager.Instance.gameState);
        gameProgressSlider.value = GameStateManager.Instance.gameState / ConfigManager.Instance.StatsConfigSO.expectedGameTime;
    }

    void OnDisable()
    {
        EventBus.Unsubscribe<PlayerHitEvent>(OnPlayerHit);
        EventBus.Unsubscribe<PlayerHealEvent>(OnPlayerHeal);
        EventBus.Unsubscribe<ScoreChangedEvent>(OnScoreChanged);
    }


    void OnPlayerHit(PlayerHitEvent e)
    {
        if (healthText != null)
        {
            healthText.text = "  生命值：";
            for (int i = 0; i < e.value; i++)
            {
                healthText.text += "█";
            }
        }
    }
    void OnPlayerHeal(PlayerHealEvent e)
    {
        if (healthText != null)
        {
            healthText.text = "  生命值：";
            for (int i = 0; i < e.value; i++)
            {
                healthText.text += "█";
            }
        }
    }
    void OnScoreChanged(ScoreChangedEvent e)
    {
        if (scoreText != null)
        {

            scoreText.text = "Socre:" + Convert.ToString(e.value);
        }
    }
}
