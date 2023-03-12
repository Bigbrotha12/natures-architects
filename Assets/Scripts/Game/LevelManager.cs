using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameLevelSO[] levels;
    [SerializeField] int currentLevelIndex;
    [SerializeField] MapGrid mapGrid;
    [SerializeField] AudioClip defaultMusic;
    [SerializeField] AudioClip gameOverMusic;
    [SerializeField] AudioClip levelSelectMusic;
    [SerializeField] PlayerController player;
    [SerializeField] GameObject levelSelectMenu;

    int levelsCompleted = 0;

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
        if (PlayerPrefs.HasKey("LevelsCompleted"))
        {
            levelsCompleted = PlayerPrefs.GetInt("LevelsCompleted");
        }
        currentLevelIndex = levelsCompleted;

        EventBroker.CharacterDeath += OnCharacterDeath;
        EventBroker.LoadNextLevel += LoadNextLevel;
        EventBroker.RestartLevel += RestartLevel;
    }

    void OnDisable()
    {
        EventBroker.CharacterDeath -= OnCharacterDeath;
        EventBroker.LoadNextLevel -= LoadNextLevel;
        EventBroker.RestartLevel -= RestartLevel;
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

    public bool SetLevelIndex(int newLevel)
    {
        if (newLevel > levelsCompleted + 1) return false;
        currentLevelIndex = newLevel - 1;
        return true;
    }

    public void InitializeLevel()
    {
        scorer.ResetScores();
        player.ShowCharacter(false);
        currentCharacterID = 0;
        player.transform.position = CurrentLevelSO.StartPosition;

        mapGrid.SetLevel(CurrentLevelSO);
        SetLevelText(CurrentLevelSO);
        uiController.SetLevelInformation(CurrentLevelSO);

        PlayLevelMusic();
        SetupCurrentCharacter();
    }

    void SetupCurrentCharacter()
    {
        CharacterSO currentCharacter = CurrentLevelSO.AvailableCharacters[currentCharacterID].CharacterSO;
        if (currentCharacter != null)
        {
            player.SetCharacter(currentCharacter, CurrentLevelSO.AvailableCharacters[currentCharacterID].Uses);

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
        }
    }

    void SetLevelText(GameLevelSO levelSO)
    {
        uiController.SetLevelText(levelSO.flavorTexts);
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

        SetupCurrentCharacter();
    }

    void LevelEnd()
    {
        PlayGameOverMusic();
        Medals result = scorer.CheckWinCondition();
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

        InitializeLevel();
    }

    void RestartLevel()
    {
        InitializeLevel();
    }

    void NoMoreLevels()
    {
        EventBroker.CallGameCompleted();
    }
}