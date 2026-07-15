using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;

public class StartMenuPanel : MonoBehaviour
{

    [SerializeField] Button StartGameButton;
    [SerializeField] Button SettingsButton;
    [SerializeField] Button LeaderBoardButton;
    [SerializeField] Button ExitGameButton;

    void Start()
    {
        if(StartGameButton && SettingsButton && LeaderBoardButton)
        {
            StartGameButton.onClick.AddListener(() => SceneManager.LoadScene("GameScene"));
            SettingsButton.onClick.AddListener(() => SceneManager.LoadScene("SettingsScene"));
            LeaderBoardButton.onClick.AddListener(() => SceneManager.LoadScene("LeaderBoardScene"));
            ExitGameButton.onClick.AddListener(onExitGameButtonclicked);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void onExitGameButtonclicked()
    {
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;   // Editor 停止播放
#else
        Application.Quit();                    // 真机退出进程
#endif
        }
    }
}
