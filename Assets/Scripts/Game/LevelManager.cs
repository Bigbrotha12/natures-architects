using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    [SerializeField] GameLevelSO currentLevelSO;
    PlayerController player;
    [SerializeField] MapGrid mapGrid;
    int currentCharacterID = 0;
    AudioSource audioSource;

    public GameLevelSO CurrentLevelSO
    {
        get { return currentLevelSO; }
    }

    void Awake()
    {
        player = FindObjectOfType<PlayerController>();
        audioSource = GetComponent<AudioSource>();
        EventBroker.CharacterDeath += OnCharacterDeath;
    }

    void OnDisable()
    {
        EventBroker.CharacterDeath -= OnCharacterDeath;
    }

    void Start()
    {
        InitializeLevel();
    }

    void InitializeLevel()
    {
        player.transform.position = CurrentLevelSO.StartPosition;
        SetupCurrentCharacter();
        mapGrid.SetLevel(CurrentLevelSO);

        PlayLevelMusic();
    }

    void SetupCurrentCharacter()
    {
        player.SetCharacter(CurrentLevelSO.AvailableCharacters[currentCharacterID].CharacterSO, CurrentLevelSO.AvailableCharacters[currentCharacterID].Uses);
    }

    void PlayLevelMusic()
    {
        audioSource.clip = currentLevelSO.music;
        audioSource.Play();
    }

    void OnCharacterDeath()
    {
        currentCharacterID++;
        if (currentCharacterID >= currentLevelSO.AvailableCharacters.Length)
        {
            LevelEnd();
            return;
        }

        SetupCurrentCharacter();
    }

    void LevelEnd()
    {
        // TODO: display win/lose screen
    }
}