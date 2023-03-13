using System.Collections;
using UnityEngine;

public interface IPlayerProgressData
{
    public ILevelProgressData GetLevelProgress(int levelID);
}

public interface ILevelProgressData
{
    public int StarsAwarded();
    public int HighScore();
    public bool Completed();
    public bool Available();
    public Sprite MapImage();
}