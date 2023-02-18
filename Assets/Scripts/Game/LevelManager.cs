using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameLevelSO[] levels;
    [SerializeField] int currentLevelIndex;
    [SerializeField] MapGrid mapGrid;
    [SerializeField] AudioClip defaultMusic;

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
        currentCharacterID = 0;
        player.transform.position = CurrentLevelSO.StartPosition;
        SetupCurrentCharacter();
        mapGrid.SetLevel(CurrentLevelSO);

        PlayLevelMusic();
    }

    void SetupCurrentCharacter()
    {
        player.SetCharacter(CurrentLevelSO.AvailableCharacters[currentCharacterID].CharacterSO, CurrentLevelSO.AvailableCharacters[currentCharacterID].Uses);

        uiController.SetNextCharacterSprites(currentCharacterID, CurrentLevelSO);
    }

    void PlayLevelMusic()
    {
        audioSource.clip = defaultMusic;
        if (CurrentLevelSO.music != null)
        {
            audioSource.clip = CurrentLevelSO.music;
        }
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
        if (scorer.CheckWinCondition())
        {
            EventBroker.CallLevelCompleted();
        }
        else
        {
            EventBroker.CallGameOver();
        }
    }

    void LoadNextLevel()
    {
        currentLevelIndex++;
        if (currentLevelIndex >= levels.Length)
        {
            NoMoreLevels();
        }

        InitializeLevel();
    }

    void RestartLevel()
    {
        InitializeLevel();
    }

    void NoMoreLevels()
    {
        print("No more levels");
        // Todo
    }
}