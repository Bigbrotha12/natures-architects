using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Player progress", fileName = "PlayerProgress")]
public class PlayerProgress_SO : ScriptableObject, IPlayerProgressData
{
    [SerializeField] string playerName;

    public ILevelProgressData GetLevelProgress(int levelID)
    {
        throw new System.NotImplementedException();
    }

    public string Name
    {
        get { return playerName; }
        set { playerName = value; }
    }
}
