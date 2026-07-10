using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseButton : MonoBehaviour
{
    
    public void ButtonPressed()
    {
        EventBus.Emit(new GamePauseEvent(!GameManager.Instance.isGamePausing));
    }
}
