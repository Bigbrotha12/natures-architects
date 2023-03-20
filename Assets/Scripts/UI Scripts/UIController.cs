using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    [Header("Message Board")]
    [SerializeField] string defaultMessageText;
    [SerializeField] TextMeshProUGUI levelMessageText;

    [Header("Intro Panel")]
    [SerializeField] string defaultIntroText;
    [SerializeField] TextMeshProUGUI levelIntroText;
    [SerializeField] TextMeshProUGUI levelIntroTitle;
    [SerializeField] GameObject LevelIntroPanel;

    [Header("Tutorial Panel")]
    [SerializeField] GameObject tutorialPanel;
    [SerializeField] TextMeshProUGUI tutorialTextObject;

    [Header("Level End Panels")]
    [SerializeField] GameObject gameOverPanel;
    [SerializeField] string defaultGameOverText;
    [SerializeField] TextMeshProUGUI gameOverText;
    [SerializeField] GameObject successPanel;
    [SerializeField] string defaultSuccessText;
    [SerializeField] TextMeshProUGUI successText;
    [SerializeField] Image bronzeStar;
    [SerializeField] Image silverStar;
    [SerializeField] Image goldStar;
    [SerializeField] Sprite filledStar;
    [SerializeField] Sprite emptyStar;

    [Header("Next Character Images")]
    [SerializeField] Image[] nextImages;
    [SerializeField] GameObject LevelInfoPanel;
    [SerializeField] GameObject AnimalInfoPrefab;
    [SerializeField] Transform AnimalInfoContainer;

    [Header("Scoring Key")]
    [SerializeField] TextMeshProUGUI currentTileText;
    [SerializeField] Transform ScoringKeyContainer;
    [SerializeField] GameObject ScoringKeyPrefab;

    public event Action IntroPanelDismissedEvent;

    void Awake()
    {
        gameOverPanel.SetActive(false);
        successPanel.SetActive(false);

        EventBroker.GameOver += OnGameOver;
        EventBroker.LevelCompleted += OnLevelCompleted;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Tab))
        {
            LevelInfoPanel.SetActive(!LevelInfoPanel.activeSelf);
        }
    }

    void OnDisable()
    {
        EventBroker.GameOver -= OnGameOver;
        EventBroker.LevelCompleted -= OnLevelCompleted;
    }

    public void SetLevelText(int levelNumber, FlavorTexts flavorTexts)
    {
        SetDefaultText();
        levelIntroTitle.text = "Level " + levelNumber;
        if (flavorTexts.IntroText != "default")
        {
            levelIntroText.text = flavorTexts.IntroText;
            levelMessageText.text = flavorTexts.IntroText;
        }
        if (flavorTexts.SuccessText != "default")
            successText.text = flavorTexts.SuccessText;
        if (flavorTexts.FailText != "default")
            gameOverText.text = flavorTexts.IntroText;
    }

    public void ShowIntroPanel()
    {
        LevelIntroPanel.SetActive(true);
    }

    public void DismissIntroPanel()
    {
        LevelIntroPanel.SetActive(false);
        IntroPanelDismissedEvent?.Invoke();
    }

    public void ShowTutorialPanel(string text)
    {
        tutorialTextObject.text = text;
        tutorialPanel.SetActive(true);
    }

    private void SetDefaultText()
    {
        levelMessageText.text = defaultMessageText;
        gameOverText.text = defaultGameOverText;
        successText.text = defaultSuccessText;
    }

    public void SetLevelInformation(GameLevelSO currentLevel)
    {
        foreach(Transform container in AnimalInfoContainer)
        {
            Destroy(container.gameObject);
        }

        foreach (CharacterUses character in currentLevel.AvailableCharacters)
        {
            GameObject animal = GameObject.Instantiate(AnimalInfoPrefab, AnimalInfoContainer);
            animal.transform.Find("ImageBorder").Find("AnimalSprite").GetComponent<Image>().sprite = character.CharacterSO.defaultSprite;
            animal.transform.Find("TileTypeText").GetComponent<TMP_Text>().text = character.CharacterSO.terrainTile.tileType.ToString();
            animal.transform.Find("Uses").Find("UsesText").GetComponent<TMP_Text>().text = character.Uses.ToString();
        }
    }

    public void OnGameOver()
    {
        ShowGameOverPanel(true);
        tutorialPanel.SetActive(false);
    }

    public void OnLevelCompleted(Medals achieved)
    {
        ShowSuccessPanel(true, achieved);
        tutorialPanel.SetActive(false);
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

    void ShowSuccessPanel(bool show, Medals achieved)
    {
        successPanel.SetActive(show);
        switch(achieved) 
        {
            case Medals.BRONZE:
                bronzeStar.sprite = filledStar;
                silverStar.sprite = emptyStar;
                goldStar.sprite = emptyStar;
                break;
            case Medals.SILVER:
                bronzeStar.sprite = filledStar;
                silverStar.sprite = filledStar;
                goldStar.sprite = emptyStar;
                break;
            case Medals.GOLD:
                bronzeStar.sprite = filledStar;
                silverStar.sprite = filledStar;
                goldStar.sprite = filledStar;
                break;
            default:
                bronzeStar.sprite = emptyStar;
                silverStar.sprite = emptyStar;
                goldStar.sprite = emptyStar;
                break;
        }
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
        ShowSuccessPanel(false, Medals.NONE);
    }

    public void SetCurrentTileType(string terrainType)
    {
        currentTileText.text = terrainType;
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
