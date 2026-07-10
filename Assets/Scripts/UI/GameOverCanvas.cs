using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameOverCanvas : MonoBehaviour
{

    [SerializeField] Text overHintText;
    [SerializeField] Text btnText;
    void OnEnable()
    {
        EventBus.Subscribe<GameOverEvent>(OnGameOver);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe<GameOverEvent>(OnGameOver);
    }

    void OnGameOver(GameOverEvent e)
    {
        gameObject.GetComponent<Canvas>().enabled = true;
        if (e.won)
        {
            overHintText.text = "恭喜获胜";
            btnText.text = "再来一局";

        }
    }
}
