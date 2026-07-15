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
    [SerializeField] Transform buttomLayoutGroup;
    private List<Image> cachedHearts = new List<Image>();

    void Start()
    {
        pauseButton?.onClick.AddListener(() => EventBus.Emit(new GamePauseEvent(!GameManager.Instance.isGamePausing)));

        // 初始化时缓存所有Heart Image
        foreach (Transform child in buttomLayoutGroup)
        {
            if (child.CompareTag("Heart") && child.TryGetComponent<Image>(out var img))
            {
                cachedHearts.Add(img);
            }
        }
        // 按Hierarchy顺序排序
        cachedHearts.Sort((a, b) => a.transform.GetSiblingIndex().CompareTo(b.transform.GetSiblingIndex()));
        UpdateHearts(ConfigManager.Instance.StatsConfigSO.playerHP);
    }

    void OnEnable()
    {
        EventBus.Subscribe<PlayerHitEvent>(OnPlayerHit);
        EventBus.Subscribe<PlayerHealEvent>(OnPlayerHeal);
        EventBus.Subscribe<ScoreChangedEvent>(OnScoreChanged);
    }

    void Update()
    {
        //gameText.text = "GameSpeed: " + Convert.ToString(GameStateManager.Instance.GameSpeed) + " GameState : " + Convert.ToString(GameStateManager.Instance.gameState);
        if (Input.GetKeyDown(KeyCode.Escape)){
            EventBus.Emit(new GamePauseEvent(!GameManager.Instance.isGamePausing));
        }
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
        UpdateHearts(e.value);
    }
    void OnPlayerHeal(PlayerHealEvent e)
    {
        UpdateHearts(e.value);
    }
    void OnScoreChanged(ScoreChangedEvent e)
    {
        if (scoreText != null)
        {

            scoreText.text = "Socre:" + Convert.ToString(e.value);
        }
    }
    public void UpdateHearts(int health)
    {
        for (int i = 0; i < cachedHearts.Count; i++)
        {
            cachedHearts[i].enabled = i < Mathf.Clamp(health, 0, cachedHearts.Count);
        }
    }
}
