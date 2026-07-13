using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class StartMenuPanel : MonoBehaviour
{

    [SerializeField] Button StartGameButton;
    [SerializeField] Button SettingsButton;
    [SerializeField] Button LeaderBoardButton;

    void Start()
    {
        if(StartGameButton && SettingsButton && LeaderBoardButton)
        {
            StartGameButton.onClick.AddListener(() => SceneManager.LoadScene("GameScene"));
            SettingsButton.onClick.AddListener(() => SceneManager.LoadScene("SettingsScene"));
            LeaderBoardButton.onClick.AddListener(() => SceneManager.LoadScene("LeaderBoardScene"));
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
