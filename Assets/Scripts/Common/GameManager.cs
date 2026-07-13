using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    bool _isGamePausing;
    public bool isGamePausing
    {
        get
        {
            return _isGamePausing;
        }
        set
        {
            _isGamePausing = value;
            if (isGamePausing)
            {
                Time.timeScale = 0;
            }
            else
            {
                Time.timeScale = 1.0f;
            }
        }
    }
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
        isGamePausing = false;
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

    void OnGamePaused(GamePauseEvent e)
    {
        isGamePausing = e.pause;
    }

    void OnGameOver(GameOverEvent e)
    {
        isGamePausing = true;
    }
}
