using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleSceneButton : MonoBehaviour
{
    Button gameStartButton;
    Button exitButton;

    private void Awake()
    {
        gameStartButton = transform.GetChild(0).GetComponent<Button>();
        exitButton = transform.GetChild(1).GetComponent<Button>();
        gameStartButton.onClick.AddListener(() => OnGameStartClick());
        exitButton.onClick.AddListener(() => OnGameExitClick());
    }

    private void OnGameStartClick()
    {
        SceneManager.LoadScene("StartScene");
    }
    private void OnGameExitClick()
    {
        Application.Quit();
    }

}
