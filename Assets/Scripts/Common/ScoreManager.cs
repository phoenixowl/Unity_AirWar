using System.Collections;
using System.Collections.Generic;
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

}
