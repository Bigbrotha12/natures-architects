using System;
using UnityEngine;

public class EventBroker
{
    public static event Action StartGame;
    public static void CallStartGame()
    {
        StartGame?.Invoke();
    }

    public static event Action QuitGame;
    public static void CallQuitGame()
    {
        QuitGame?.Invoke();
    }

    public static event Action PlayerMove;
    public static void CallPlayerMove()
    {
        PlayerMove?.Invoke();
    }

    public static event Action PlayerMoveBlocked;
    public static void CallPlayerMoveBlocked()
    {
        PlayerMoveBlocked?.Invoke();
    }

    public static event Action PlaceTerrain;
    public static void CallPlaceTerrain()
    {
        PlaceTerrain?.Invoke();
    }

    public static event Action CharacterDeath;
    public static void CallCharacterDeath()
    {
        CharacterDeath?.Invoke();
    }

    public static event Action GameOver;
    public static void CallGameOver()
    {
        GameOver?.Invoke();
    }

    public static event Action<Medals> LevelCompleted;
    public static void CallLevelCompleted(Medals result)
    {
        LevelCompleted?.Invoke(result);
    }


    public static event Action ReturnToTitleScreen;
    public static void CallReturnToTitleScreen()
    {
        ReturnToTitleScreen?.Invoke();
    }

    
    public static event Action RestartLevel;
    public static void CallRestartLevel()
    {
        RestartLevel?.Invoke();
    }

    public static event Action RestartGame;
    public static void CallRestartGame()
    {
        RestartGame?.Invoke();
    }

    public static event Action LoadNextLevel;
    public static void CallLoadNextLevel()
    {
        LoadNextLevel?.Invoke();
    }
    
    public static event Action GameCompleted;
    public static void CallGameCompleted()
    {
        GameCompleted?.Invoke();
    }

    public static event Action ExitSoundMenu;
    public static void CallExitSoundMenu()
    {
        ExitSoundMenu?.Invoke();
    }

    public static event Action<CharacterSO> CharacterChange;
    public static void CallCharacterChange(CharacterSO newCharacter)
    {
        CharacterChange?.Invoke(newCharacter);
    }
}