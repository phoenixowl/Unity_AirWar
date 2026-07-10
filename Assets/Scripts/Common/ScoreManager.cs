using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    int _score;
    public int Score {
        get { return _score; }
        set{
            _score = value;
            EventBus.Emit(new ScoreChangedEvent(_score));
        }
    }

    public static ScoreManager Instance { get; private set; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    void OnEnable()
    {
        EventBus.Subscribe<EnemyDiedEvent>(OnEnemyDied);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe<EnemyDiedEvent>(OnEnemyDied);
    }

    void OnEnemyDied(EnemyDiedEvent e)
    {
        Score += e.score;
    }

}
