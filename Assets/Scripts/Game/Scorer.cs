using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scorer : MonoBehaviour
{
    [Header("Actual")]
    [SerializeField] TMP_Text totalScoreText;
    [SerializeField] TMP_Text grassScoreText;
    [SerializeField] TMP_Text mountainScoreText;
    [SerializeField] TMP_Text forestScoreText;
    [SerializeField] TMP_Text waterScoreText;
    [SerializeField] TMP_Text snowScoreText;
    [SerializeField] TMP_Text fireScoreText;

    [Header("Targets")]
    [SerializeField] TMP_Text totalTargetText;
    [SerializeField] TMP_Text grassTargetText;
    [SerializeField] TMP_Text mountainTargetText;
    [SerializeField] TMP_Text forestTargetText;
    [SerializeField] TMP_Text waterTargetText;
    [SerializeField] TMP_Text snowTargetText;
    [SerializeField] TMP_Text fireTargetText;

    int totalScore = 0;
    int totalScoreTarget = 0;
    Dictionary<TerrainTypes, int> terrainScores = new Dictionary<TerrainTypes, int>();
    Dictionary<TerrainTypes, int> terrainTarget = new Dictionary<TerrainTypes, int>();

    public void ResetScores()
    {
        // TODO: reset all scores to zero
    }
    
    public void ScoreTile(TerrainTile scoringTile, TerrainTile[] adjacentTiles)
    {
        if(scoringTile is null) 
        {
            Debug.LogError("Scoring Tile cannot be null.");
            return;
        }
        
        // Each tile score +1 for adjacent tiles of same type.
        int score = 0;
        foreach(TerrainTile tile in adjacentTiles)
        {
            if(tile.tileType == scoringTile.tileType)
            {
                score += scoringTile.GetAdjacentTileValue(tile.tileType);
            }
        }
        
        UpdateScore(scoringTile.tileType, score);
        DisplayScore(scoringTile.tileType);
    }

    public int GetTerrainScore(TerrainTypes terrain)
    {
        return terrainScores.ContainsKey(terrain) ? terrainScores[terrain] : 0;
    }

    public int GetTotalScore()
    {
        return totalScore;
    }

    void UpdateScore(TerrainTypes tileType, int score)
    {
        if(!terrainScores.ContainsKey(tileType))
        {
            terrainScores[tileType] = score;
        } else 
        {
            terrainScores[tileType] += score;
        }
        totalScore += score;
    }

    void DisplayScore(TerrainTypes type)
    {
        switch(type)
        {
            case TerrainTypes.NONE:
                break;
            case TerrainTypes.GRASS:
                grassScoreText.text = terrainScores[type].ToString();
                break;
            case TerrainTypes.FOREST:
                forestScoreText.text = terrainScores[type].ToString();
                break;
            case TerrainTypes.WATER:
                waterScoreText.text = terrainScores[type].ToString();
                break;
            case TerrainTypes.MOUNTAIN:
                mountainScoreText.text = terrainScores[type].ToString();
                break;
            case TerrainTypes.FIRE:
                fireScoreText.text = terrainScores[type].ToString();
                break;
            case TerrainTypes.SNOW:
                snowScoreText.text = terrainScores[type].ToString();
                break;
        }
        totalScoreText.text = totalScore.ToString();
    }

    public void SetTargetScore(TerrainTypes type, int target)
    {
        terrainTarget[type] = target;
        totalScoreTarget += target;

        switch(type)
        {
            case TerrainTypes.NONE:
                break;
            case TerrainTypes.GRASS:
                grassTargetText.text = target.ToString();
                break;
            case TerrainTypes.FOREST:
                forestTargetText.text = target.ToString();
                break;
            case TerrainTypes.WATER:
                waterTargetText.text = target.ToString();
                break;
            case TerrainTypes.MOUNTAIN:
                mountainTargetText.text = target.ToString();
                break;
            case TerrainTypes.FIRE:
                fireTargetText.text = target.ToString();
                break;
            case TerrainTypes.SNOW:
                snowTargetText.text = target.ToString();
                break;
        }
        totalTargetText.text = totalScoreTarget.ToString();
    }

    public bool CheckWinCondition() 
    {
        foreach (KeyValuePair<TerrainTypes, int> score in terrainScores)
        {
            if(terrainTarget.ContainsKey(score.Key))
            {
                if(score.Value < terrainTarget[score.Key])
                {
                    Debug.Log(
                        "You failed to meet score target for " + score.Key.ToString() + 
                        " terrain. Achieved: " + score.Value.ToString() +
                        ". Required: " + terrainTarget[score.Key].ToString() + ".");
                    return false;
                }
            }
        }
        return true;
    }
}
