using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class BacktoLobbyBtn : MonoBehaviour
{
    public void BackToTitle()
    {
        SceneManager.LoadScene("StartMenuScene");
    }
}
