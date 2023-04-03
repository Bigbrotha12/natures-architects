using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public GameLevelSO[] levels;
    [SerializeField] int currentLevelIndex;
    [SerializeField] MapGrid mapGrid;
    [SerializeField] AudioClip defaultMusic;
    [SerializeField] AudioClip gameOverMusic;
    [SerializeField] AudioClip levelSelectMusic;
    [SerializeField] PlayerController player;
    [SerializeField] GameObject levelSelectMenu;
    [SerializeField] GameObject storyEndText;

    Scorer scorer;
    UIController uiController;
    int currentCharacterID = 0;
    AudioSource audioSource;

    public GameLevelSO CurrentLevelSO
    {
        get { return levels[currentLevelIndex]; }
    }

    void Awake()
    {
        scorer = FindObjectOfType<Scorer>();
        uiController = FindObjectOfType<UIController>();
        audioSource = GetComponent<AudioSource>();

        EventBroker.CharacterDeath += OnCharacterDeath;
        EventBroker.LoadNextLevel += LoadNextLevel;
        EventBroker.RestartLevel += RestartLevel;
    }

    void OnDisable()
    {
        EventBroker.CharacterDeath -= OnCharacterDeath;
        EventBroker.LoadNextLevel -= LoadNextLevel;
        EventBroker.RestartLevel -= RestartLevel;
        uiController.IntroPanelDismissedEvent -= OnIntroPanelDismissed;
    }

    void Start()
    {
        ShowLevelSelectMenu(true);
    }

    public void ShowLevelSelectMenu(bool value)
    {
        levelSelectMenu.SetActive(value);
        if (value)
        {
            PlayMusic(levelSelectMusic);
        }
    }

    public bool SetLevelIndex(int newLevelIndex)
    {
        if (newLevelIndex >= levels.Length) return false;

        currentLevelIndex = newLevelIndex;
        return true;
    }

    public void InitializeLevel(bool showIntro)
    {
        Debug.Log("Index result: " + currentLevelIndex.ToString());
        scorer.ResetScores();
        player.ShowCharacter(false);
        currentCharacterID = 0;
        player.transform.position = CurrentLevelSO.StartPosition;

        mapGrid.SetLevel(CurrentLevelSO);
        SetLevelText(CurrentLevelSO);
        uiController.SetLevelInformation(CurrentLevelSO);

        PlayLevelMusic();
        if (showIntro)
        {
            uiController.ShowIntroPanel();
        }
        SetupCurrentCharacter(showIntro);
    }

    void SetupCurrentCharacter(bool showIntro)
    {
        CharacterSO currentCharacter = CurrentLevelSO.AvailableCharacters[currentCharacterID].CharacterSO;
        if (currentCharacter != null)
        {
            uiController.SetNextCharacterSprites(currentCharacterID, CurrentLevelSO);
            uiController.DisplayScoringKey(currentCharacter);
            if (currentCharacter.terrainTile != null)
            {
                uiController.SetCurrentTileType(currentCharacter.terrainTile.name);
            }
            else
            {
                uiController.SetCurrentTileType("");
            }

            if (showIntro)
            {
                uiController.IntroPanelDismissedEvent += OnIntroPanelDismissed;
                return;
            }
            player.SetCharacter(currentCharacter, CurrentLevelSO.AvailableCharacters[currentCharacterID].Uses);
        }
    }

    void OnIntroPanelDismissed()
    {
        player.SetCharacter(CurrentLevelSO.AvailableCharacters[currentCharacterID].CharacterSO, CurrentLevelSO.AvailableCharacters[currentCharacterID].Uses);
        uiController.IntroPanelDismissedEvent -= OnIntroPanelDismissed;
        ShowTutorialPanel();
    }

    void ShowTutorialPanel()
    {
        if (CurrentLevelSO.ShowTutorialText)
        {
            uiController.ShowTutorialPanel(CurrentLevelSO.FlavorTexts.TutorialText);
        }
    }

    void SetLevelText(GameLevelSO levelSO)
    {
        uiController.SetLevelText(currentLevelIndex + 1, levelSO.FlavorTexts);
    }

    void PlayLevelMusic()
    {
        AudioClip clip = defaultMusic;
        if (CurrentLevelSO.music != null)
        {
            clip = CurrentLevelSO.music;
        }
        PlayMusic(clip);
    }

    void PlayMusic(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    void OnCharacterDeath()
    {
        currentCharacterID++;
        if (currentCharacterID >= CurrentLevelSO.AvailableCharacters.Length)
        {
            LevelEnd();
            return;
        }

        SetupCurrentCharacter(false);
    }

    void LevelEnd()
    {
        PlayGameOverMusic();
        Medals result = scorer.CheckWinCondition();
        LevelProgress_SO levelProgress = new LevelProgress_SO { StarsAwarded = (int)result, 
                                                            Available = true, 
                                                            Completed = result != Medals.NONE, 
                                                            HighScore = scorer.GetTotalScore() };
        if (SaveData.Instance != null)
        {
            SaveData.Instance.LevelPlayed(levels[currentLevelIndex].levelID, levelProgress); 
        }
        if (result != Medals.NONE)
        {
            EventBroker.CallLevelCompleted(result);
        }
        else
        {
            EventBroker.CallGameOver();
        }
    }

    void PlayGameOverMusic()
    {
        PlayMusic(gameOverMusic);
    }

    void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex >= levels.Length)
        {
            NoMoreLevels();
            return;
        }

        InitializeLevel(true);
    }

    void RestartLevel()
    {
        InitializeLevel(false);
    }

    void NoMoreLevels()
    {
        storyEndText.SetActive(true);
        if (GameManager.Instance != null)
        {
            GameManager.Instance.ChangeGameState(GameState.ENDGAME);
        }
    }
}