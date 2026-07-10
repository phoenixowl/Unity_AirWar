using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreenCanvas : MonoBehaviour
{
    void OnEnable()
    {
        EventBus.Subscribe<GamePauseEvent>(OnGamePaused);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe<GamePauseEvent>(OnGamePaused);
    }

    void OnGamePaused(GamePauseEvent e)
    {
       gameObject.GetComponent<Canvas>().enabled = e.pause;
    }
}
