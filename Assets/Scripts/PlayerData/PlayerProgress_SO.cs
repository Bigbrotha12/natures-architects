using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player progress", fileName = "PlayerProgress")]
public class PlayerProgress_SO : ScriptableObject, IPlayerProgressData
{
    [SerializeField] string playerName;
    [SerializeField] Dictionary<int, ILevelProgressData> levels = new Dictionary<int, ILevelProgressData>();

    public string Name
    {
        get { return playerName; }
        set { playerName = value; }
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
        if (levels.TryAdd(levelID, levelProgress)) return;

        int currentHighScore = GetLevelProgressData(levelID).HighScore;
        if (levelProgress.HighScore >= currentHighScore)
        {
            levels[levelID] = levelProgress;
        }
    }

    public ILevelProgressData GetLevelProgressData(int levelID)
    {
        ILevelProgressData levelProgress;
        if (levels.TryGetValue(levelID, out levelProgress))
        {
            return levelProgress;
        }
        return null;
    }
}

public class LevelProgress :  ILevelProgressData
{
    int starsAwarded;
    int highScore;
    bool completed;
    bool available;
    Sprite mapImage;

    public int StarsAwarded { get => starsAwarded; set => starsAwarded = value; }
    public int HighScore { get => highScore; set => highScore = value; }
    public bool Completed { get => completed; set => completed = value; }
    public bool Available { get => available; set => available = value; }
    public Sprite MapImage { get => mapImage; set => mapImage = value; }
}