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
    [SerializeField] FloatingTextManager floatText;
    int totalScore = 0;
    int totalScoreTarget = 0;
    Dictionary<TerrainTypes, int> terrainScores = new Dictionary<TerrainTypes, int>();
    Dictionary<TerrainTypes, int> terrainTarget = new Dictionary<TerrainTypes, int>();

    public void ResetScores()
    {
        terrainScores = new Dictionary<TerrainTypes, int>();
        totalScoreTarget = 0;
        totalScore = 0;
        DisplayScore(TerrainTypes.GRASS);
        DisplayScore(TerrainTypes.FOREST);
        DisplayScore(TerrainTypes.MOUNTAIN);
        DisplayScore(TerrainTypes.WATER);
        DisplayScore(TerrainTypes.FIRE);
        DisplayScore(TerrainTypes.SNOW);
    }
    
    public void ScoreTile(TerrainTile scoringTile, TerrainTile[] adjacentTiles)
    {
        if(scoringTile is null) 
        {
            Debug.LogError("Scoring Tile cannot be null.");
            return;
        }
        
        int score = 0;
        foreach(TerrainTile tile in adjacentTiles)
        {
            Debug.Log("Adjacent Terrains: " + tile.tileType.ToString());
            int tileScore = scoringTile.GetAdjacentTileValue(tile.tileType);
            Debug.Log("Score: " + tileScore.ToString());
            score += scoringTile.GetAdjacentTileValue(tile.tileType);
            
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
    
        floatText.Show(score.ToString() + " Points.", score >= 0);
    }

    void DisplayScore(TerrainTypes type)
    {
        switch(type)
        {
            case TerrainTypes.NONE:
                break;
            case TerrainTypes.GRASS:
                grassScoreText.text = terrainScores.ContainsKey(type) ? terrainScores[type].ToString() : "0";
                break;
            case TerrainTypes.FOREST:
                forestScoreText.text = terrainScores.ContainsKey(type) ? terrainScores[type].ToString() : "0";
                break;
            case TerrainTypes.WATER:
                waterScoreText.text = terrainScores.ContainsKey(type) ? terrainScores[type].ToString() : "0";
                break;
            case TerrainTypes.MOUNTAIN:
                mountainScoreText.text = terrainScores.ContainsKey(type) ? terrainScores[type].ToString() : "0";
                break;
            case TerrainTypes.FIRE:
                fireScoreText.text = terrainScores.ContainsKey(type) ? terrainScores[type].ToString() : "0";
                break;
            case TerrainTypes.SNOW:
                snowScoreText.text = terrainScores.ContainsKey(type) ? terrainScores[type].ToString() : "0";
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
        foreach (KeyValuePair<TerrainTypes, int> target in terrainTarget)
        {
            if(target.Value == 0) continue;

            if(!terrainScores.ContainsKey(target.Key))
            {
                return false;
                
            } else 
            {
                if(target.Value > terrainScores[target.Key])
                {
                    Debug.Log(
                        "You failed to meet score target for " + target.Key.ToString() + 
                        " terrain. Achieved: " + terrainScores[target.Key].ToString() +
                        ". Required: " + target.Value.ToString() + ".");
                    return false;
                }
            }
        }
        return true;
    }
}
