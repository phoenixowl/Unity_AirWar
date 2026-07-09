using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ScoreText : MonoBehaviour
{
    void OnEnable()
    {
        EventBus.Subscribe<ScoreChangedEvent>(OnScoreChanged);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe<ScoreChangedEvent>(OnScoreChanged);
    }

    void OnScoreChanged(ScoreChangedEvent e)
    {
        TextMeshProUGUI textMeshPro = GetComponent<TextMeshProUGUI>();
        if (textMeshPro != null)
        {

            textMeshPro.text = "Socre:" + Convert.ToString(e.value);
        }
    }
}
