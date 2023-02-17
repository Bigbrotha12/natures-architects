using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Scorer : MonoBehaviour
{
    [SerializeField] TMP_Text totalScoreText;
    [SerializeField] TMP_Text grassScoreText;
    [SerializeField] TMP_Text mountainScoreText;
    [SerializeField] TMP_Text forestScoreText;
    [SerializeField] TMP_Text waterScoreText;
    int totalScore = 0;
    Dictionary<TerrainTypes, int> terrainScores = new Dictionary<TerrainTypes, int>();
    
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
            Debug.Log("Scoring tile type: " + tile.tileType.ToString());
            if(tile.tileType == scoringTile.tileType)
            {
                score += 1;
            }
        }
        
        UpdateScore(scoringTile.tileType, score);
        DisplayScore(scoringTile.tileType);
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
                grassScoreText.text = terrainScores[type].ToString();
                break;
            case TerrainTypes.WATER:
                grassScoreText.text = terrainScores[type].ToString();
                break;
            case TerrainTypes.MOUNTAIN:
                grassScoreText.text = terrainScores[type].ToString();
                break;
            case TerrainTypes.FIRE:
                grassScoreText.text = terrainScores[type].ToString();
                break;
            case TerrainTypes.SNOW:
                grassScoreText.text = terrainScores[type].ToString();
                break;
        }
    }
}
