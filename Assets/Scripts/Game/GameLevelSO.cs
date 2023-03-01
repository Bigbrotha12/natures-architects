using UnityEngine;

[CreateAssetMenu(menuName = "Game Level", fileName = "Levels")]
public class GameLevelSO : ScriptableObject
{
    [System.Serializable]
    public class ScoreTargets
    {
        public int[] GrassTargets;
        public int[] ForestTargets;
        public int[] MountainTargets;
        public int[] WaterTargets;
        public int[] SnowTargets;
        public int[] FireTargets;
    }

    public string[] flavorTexts;
    public AudioClip music;
    
    [Header("Grid")]
    public TerrainTile starterTile;
    public int rows;
    public int columns;

    [Header("Player info")]
    public Vector3 StartPosition = Vector3.zero;
    public CharacterUses[] AvailableCharacters;

    [Header("Target Scores")]
    public ScoreTargets levelTargets;
}

[System.Serializable]
public class CharacterUses
{
    public CharacterSO CharacterSO;
    public int Uses; 
}