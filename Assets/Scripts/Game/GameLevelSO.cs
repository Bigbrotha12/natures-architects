using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "Game Level", fileName = "Levels")]
public class GameLevelSO : ScriptableObject
{
    public int levelID;
    [System.Serializable]
    public class ScoreTargets
    {
        public int[] GrassTargets = new int[3];
        public int[] ForestTargets = new int[3];
        public int[] MountainTargets = new int[3];
        public int[] WaterTargets = new int[3];
        public int[] SnowTargets = new int[3];
        public int[] FireTargets = new int[3];
    }

    public string StoryText;
    public FlavorTexts FlavorTexts;
    public bool ShowTutorialText;
    public AudioClip music;
    
    [Header("Grid")]
    public GameObject map;
    public TerrainTile starterTile;
    public int rows;
    public int columns;
    

    [Header("Player info")]
    public Vector3Int StartCoordinates;
    public CharacterUses[] AvailableCharacters;
    public Vector3 StartPosition { get {
        return StartCoordinates;
    }}

    [Header("Target Scores")]
    public ScoreTargets levelTargets;
}

[System.Serializable]
public class CharacterUses
{
    public CharacterSO CharacterSO;
    public int Uses; 
}

[System.Serializable]
public struct FlavorTexts
{
    public string IntroText;
    public string SuccessText;
    public string FailText;
    public string TutorialText;
}