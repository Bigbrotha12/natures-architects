using System;
using UnityEngine;

public class SaveData : Singleton<SaveData>, ISaveable
{
    [SerializeField] string saveFileName;
    public PlayerProgress_SO PlayerProgressData;
    SavingSystem saveSystem;

    protected override void Awake()
    {
        base.Awake();

        saveSystem = FindObjectOfType<SavingSystem>();
        saveSystem.Load(saveFileName);
    }

    public object CaptureState()
    {
        // TODO: serialise the playerprogressSO

        PlayerProgress playerProgress = ConvertFromPlayerProgressSO(PlayerProgressData);
        return playerProgress;
    }

    public void LevelPlayed(int levelID, ILevelProgressData levelProgressData)
    {
        PlayerProgressData.AddLevelProgressData(levelID, levelProgressData);
        saveSystem.Save(saveFileName);
    }

    public void RestoreState(object state)
    {
        // Deserialise player progress
        SaveToPlayerProgressSO(state as PlayerProgress);
    }

    PlayerProgress ConvertFromPlayerProgressSO(PlayerProgress_SO progressSO)
    {
        PlayerProgress progress = new PlayerProgress();
        progress.playerName = progressSO.Name;

        foreach (var level in progressSO.Levels)
        {
            progress.levels.Add(level.Key, ConvertFromLevelProgressSO(level.Value));
        }

        return progress;
    }

    private LevelProgress ConvertFromLevelProgressSO(LevelProgress_SO progressSO)
    {
        LevelProgress progress = new LevelProgress();
        progress.available = progressSO.Available;
        progress.completed = progressSO.Completed;
        progress.highScore = progressSO.HighScore;
        progress.starsAwarded = progressSO.StarsAwarded;

        return progress;
    }

    void SaveToPlayerProgressSO(PlayerProgress progress)
    {
      
        PlayerProgressData.Name = progress.playerName;

        foreach (var level in progress.levels)
        {
            PlayerProgressData.AddLevelProgressData(level.Key, ConvertToLevelProgressSO(level.Value));
        }
    }

    private LevelProgress_SO ConvertToLevelProgressSO(LevelProgress progress)
    {
        LevelProgress_SO so = new LevelProgress_SO();
        so.Available = progress.available;
        so.Completed = progress.completed;
        so.HighScore = progress.highScore;
        so.StarsAwarded = progress.starsAwarded;

        return so;
    }
}
