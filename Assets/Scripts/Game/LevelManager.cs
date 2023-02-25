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

    PlayerController player;
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
        player = FindObjectOfType<PlayerController>();
        scorer = FindObjectOfType<Scorer>();
        uiController = FindObjectOfType<UIController>();
        audioSource = GetComponent<AudioSource>();
        currentLevelIndex = 0;

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
        InitializeLevel();
    }

    void InitializeLevel()
    {
        scorer.ResetScores();
        currentCharacterID = 0;
        player.transform.position = CurrentLevelSO.StartPosition;
        SetupCurrentCharacter();
        mapGrid.SetLevel(CurrentLevelSO);
        SetLevelText(CurrentLevelSO);

        PlayLevelMusic();
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
        audioSource.clip = clip;
        audioSource.Play();
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
        if (scorer.CheckWinCondition())
        {
            EventBroker.CallLevelCompleted();
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