using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreenCanvas : MonoBehaviour
{
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
       gameObject.GetComponent<Canvas>().enabled = e.pause;
    }

    void OnGameOver(GameOverEvent e)
    {
        gameObject.SetActive(false);
    }
}
