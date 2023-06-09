﻿using System.Collections;
using UnityEngine;

public interface IPlayerProgressData
{
    public string Name { get; set; }
    public int LevelsCompleted { get; }
    public ILevelProgressData GetLevelProgressData(int levelID);
    public void AddLevelProgressData(int levelID, ILevelProgressData levelProgress);
}

public interface ILevelProgressData
{
    public int StarsAwarded { get; set; }
    public int HighScore { get; set; }
    public bool Completed { get; set; }
    public bool Available { get; set; }
}