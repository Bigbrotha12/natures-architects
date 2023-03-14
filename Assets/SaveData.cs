using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaveData : Singleton<SaveData>
{
    public PlayerProgress_SO PlayerProgressData;

    public void LevelPlayed(int levelID, ILevelProgressData levelProgressData)
    {
        PlayerProgressData.AddLevelProgressData(levelID, levelProgressData);
    }
}
