using UnityEngine;

[CreateAssetMenu(menuName = "Game Level", fileName = "Levels")]
public class GameLevelSO : ScriptableObject
{
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
    public int grassTargetScore;
    public int forestTargetScore;
    public int mountainTargetScore;
    public int waterTargetScore;
    public int fireTargetScore;
    public int snowTargetScore;
}

[System.Serializable]
public class CharacterUses
{
    public CharacterSO CharacterSO;
    public int Uses; 
}