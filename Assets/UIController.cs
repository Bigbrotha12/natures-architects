using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Message Board")]
    [SerializeField] string defaultMessageText;
    [SerializeField] TextMeshProUGUI levelMessageText;

    [Header("Level End Panels")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] string defaultGameOverText;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] GameObject successPanel;
    [SerializeField] string defaultSuccessText;
    [SerializeField] TextMeshProUGUI successText;

    [Header("Next Character Images")]
    [SerializeField] Image[] nextImages;

    [Header("Scoring Key")]
    [SerializeField] Transform ScoringKeyContainer;
    [SerializeField] GameObject ScoringKeyPrefab;

    void Awake()
    {
        gameOverPanel.SetActive(false);
        successPanel.SetActive(false);

        EventBroker.GameOver += OnGameOver;
        EventBroker.LevelCompleted += OnLevelCompleted;
    }

    void OnDisable()
    {
        EventBroker.GameOver -= OnGameOver;
        EventBroker.LevelCompleted -= OnLevelCompleted;
    }

    public void SetLevelText(string[] messages)
    {
        SetDefaultText();
        if (messages.Length > 0)
            levelMessageText.text = messages[0];
        if (messages.Length > 1)
            successText.text = messages[1];
        if (messages.Length > 2)
            gameOverText.text = messages[2];
    }

    private void SetDefaultText()
    {
        levelMessageText.text = defaultMessageText;
        gameOverText.text = defaultGameOverText;
        successText.text = defaultSuccessText;
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

    public void DisplayScoringKey(CharacterSO currentCharacter)
    {
        foreach (Transform key in ScoringKeyContainer)
        {
            Destroy(key.gameObject);
        }

        foreach (string key in currentCharacter.terrainTile.GetScoringKeyText())
        {
            GameObject scoringKey = GameObject.Instantiate(ScoringKeyPrefab, ScoringKeyContainer);
            scoringKey.GetComponent<TMP_Text>().text = key;
        }
    }
}
