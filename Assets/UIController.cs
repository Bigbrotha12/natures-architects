using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] GameObject successPanel;

    [Header("Next Character Images")]
    [SerializeField] Image[] nextImages;

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

    void HideGameOverPanels()
    {
        gameOverPanel.SetActive(false);
        successPanel.SetActive(false);
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
        HideGameOverPanels();
    }

    public void RestartLevel()
    {
        EventBroker.CallRestartLevel();
        HideGameOverPanels();
    }

    public void NextLevel()
    {
        EventBroker.CallLoadNextLevel();
        ShowSuccessPanel(false);
    }

    public void SetNextCharacterSprites(int currentCharacterID, GameLevelSO currentLevel)
    {
        int index = currentCharacterID + 1;

        for (int i = 0; i < nextImages.Length; i++, index++)
        {
            if (index < currentLevel.AvailableCharacters.Length)
            {
                nextImages[i].sprite = currentLevel.AvailableCharacters[index].CharacterSO.defaultSprite;
            }
            else
            {
                nextImages[i].sprite = null; 
            }
        }
    }
}
