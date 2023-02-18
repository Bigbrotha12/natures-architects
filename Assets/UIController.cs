using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject successPanel;


    void Awake()
    {
        gameOverPanel.SetActive(false);
        successPanel.SetActive(false);

        EventBroker.GameOver += OnGameOver;
        EventBroker.LevelCompleted += OnLevelCompleted;
    }

    public void OnGameOver()
    {
        ShowGameOverPanel(true);
    }

    public void OnLevelCompleted()
    {
        ShowSuccessPanel(true);
    }

    void ShowGameOverPanel(bool show)
    {
        gameOverPanel.SetActive(show);
    }

    void ShowSuccessPanel(bool show)
    {
        successPanel.SetActive(show);
    }

    public void QuitToStart()
    {
        EventBroker.CallReturnToTitleScreen();
    }

    public void RestartLevel()
    {
        EventBroker.CallRestartLevel();
    }

    public void NextLevel()
    {
        EventBroker.CallLoadNextLevel();
    }
}
