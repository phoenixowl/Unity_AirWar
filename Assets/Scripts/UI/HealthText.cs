using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;

public class HealthText : MonoBehaviour
{
    void OnEnable()
    {
        EventBus.Subscribe<PlayerHitEvent>(OnPlayerHit);
    }

    void OnDisable()
    {
        EventBus.Unsubscribe<PlayerHitEvent>(OnPlayerHit);
    }

    void OnPlayerHit(PlayerHitEvent e)
    {
        UnityEngine.UI.Text text = GetComponent<UnityEngine.UI.Text>();
        if (text != null)
        {
            text.text = "  ÉúÃüÖµ£º";
            for (int i = 0; i < e.value; i++)
            {
                text.text += "¨€";
            }
        }
    }
}
