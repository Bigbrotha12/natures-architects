using UnityEngine;

[CreateAssetMenu(menuName = "Game Level", fileName = "Levels")]
public class GameLevelSO : ScriptableObject
{
    public TerrainTile starterTile;
    public int rows;
    public int columns;
    public string[] flavorTexts;
    public int grassTargetScore;
    public int forestTargetScore;
    public int mountainTargetScore;
    public int waterTargetScore;
    public int fireTargetScore;
    public int snowTargetScore;
}