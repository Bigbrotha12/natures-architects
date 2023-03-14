using System.Collections;
using UnityEngine;

public interface IPlayerProgressData
{
    public string Name { get; set; }
    public ILevelProgressData GetLevelProgress(int levelID);
}

public interface ILevelProgressData
{
    public int StarsAwarded { get; set; }
    public int HighScore { get; set; }
    public bool Completed { get; set; }
    public bool Available { get; set; }
    public Sprite MapImage { get; set; }
}