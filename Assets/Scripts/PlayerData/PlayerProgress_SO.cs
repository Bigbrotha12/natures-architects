using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
[CreateAssetMenu(menuName = "Player progress", fileName = "PlayerProgress")]
public class PlayerProgress_SO : ScriptableObject, IPlayerProgressData
{
    [SerializeField] string playerName;
    [SerializeField] Dictionary<int, LevelProgress_SO> levels = new Dictionary<int, LevelProgress_SO>();

    public string Name
    {
        get { return playerName; }
        set { playerName = value; }
    }

    public Dictionary<int, LevelProgress_SO> Levels
    {
        get { return levels; }
    }

    public int LevelsCompleted
    {
        get { return CountLevelsCompleted(); }
    }

    private int CountLevelsCompleted()
    {
        int count = 0;
        foreach (var item in levels)
        {
            if (item.Value.Completed) count++;
        }
        return count;
    }

    public void AddLevelProgressData(int levelID, ILevelProgressData levelProgress)
    {
        if (levels.TryAdd(levelID, levelProgress as LevelProgress_SO)) return;

        int currentHighScore = GetLevelProgressData(levelID).HighScore;
        if (levelProgress.HighScore >= currentHighScore)
        {
            levels[levelID] = levelProgress as LevelProgress_SO;
        }
    }

    public ILevelProgressData GetLevelProgressData(int levelID)
    {
        LevelProgress_SO levelProgress;
        if (levels.TryGetValue(levelID, out levelProgress))
        {
            return levelProgress;
        }
        return null;
    }
}

[System.Serializable]
public class LevelProgress_SO :  ILevelProgressData
{
    int starsAwarded;
    int highScore;
    bool completed;
    bool available;

    public int StarsAwarded { get => starsAwarded; set => starsAwarded = value; }
    public int HighScore { get => highScore; set => highScore = value; }
    public bool Completed { get => completed; set => completed = value; }
    public bool Available { get => available; set => available = value; }
}

[System.Serializable]
public class LevelProgress
{
    public int starsAwarded;
    public int highScore;
    public bool completed;
    public bool available;
}

[System.Serializable]
public class PlayerProgress
{
    public string playerName;
    public Dictionary<int, LevelProgress> levels = new Dictionary<int, LevelProgress>();
}