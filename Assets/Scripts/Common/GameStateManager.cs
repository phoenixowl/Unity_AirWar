using System;
using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class GameStateManager : MonoBehaviour
{
    public static GameStateManager Instance { get; private set; }

   public float gameState = 0;

    private float _gameSpeed;
    public float GameSpeed {
        get
        {
            return _gameSpeed;
        }
        set
        {
            _gameSpeed = Mathf.Clamp(value, 0.1f, 2f);
        } }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

    }

    private void Start()
    {
        GameSpeed = 1.0f;
    }

    private void Update()
    {
        gameState += GameSpeed * Time.deltaTime;
        if(gameState > ConfigManager.Instance.StatsConfigSO.expectedGameTime)
        {
            EventBus.Emit(new GameOverEvent(true));
        }
    }
    void OnEnable()
    {
        EventBus.Subscribe<EnemyDiedEvent>(OnEnemyDied);
        EventBus.Subscribe<PlayerHitEvent>(OnPlayerHit);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe<EnemyDiedEvent>(OnEnemyDied);
        EventBus.Unsubscribe<PlayerHitEvent>(OnPlayerHit);
    }

    void OnEnemyDied(EnemyDiedEvent e)
    {
        GameSpeed += e.score / 5000.0f;
    }
    void OnPlayerHit(PlayerHitEvent e)
    {
        GameSpeed -= 0.1f;
    }
}