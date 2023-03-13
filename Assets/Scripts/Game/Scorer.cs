using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Scorer : MonoBehaviour
{
    [System.Serializable]
    private class ScoreUI
    {
        public GameObject UIObject;
        public TMP_Text ActualScore;
        public TMP_Text TargetScore;
        public Image ScoreBar;
        public Image ScoreCheck;
    }

    [SerializeField] ScoreUI grassScore;
    [SerializeField] ScoreUI forestScore;
    [SerializeField] ScoreUI mountainScore;
    [SerializeField] ScoreUI waterScore;
    [SerializeField] ScoreUI snowScore;
    [SerializeField] ScoreUI fireScore;

    [SerializeField] TMP_Text totalScoreText;
    [SerializeField] TMP_Text totalTargetText;
    [SerializeField] Sprite Checkmark;
    [SerializeField] Sprite Empty;
    [SerializeField] FloatingTextManager floatText;
    int totalScore = 0;
    int totalScoreTarget = 0;
    Dictionary<TerrainTypes, int> terrainScores = new Dictionary<TerrainTypes, int>();
    Dictionary<TerrainTypes, (int, int, int)> terrainTarget = new Dictionary<TerrainTypes, (int, int, int)>();
    Dictionary<TerrainTypes, ScoreUI> UIElement;

    void Awake()
    {
        UIElement = new Dictionary<TerrainTypes, ScoreUI>()
        {
            {TerrainTypes.Grass, grassScore},
            {TerrainTypes.Forest, forestScore},
            {TerrainTypes.Mountain, mountainScore},
            {TerrainTypes.Water, waterScore},
            {TerrainTypes.Snow, snowScore},
            {TerrainTypes.Fire, fireScore}
        };
    }

    public void ResetScores()
    {
        terrainScores = new Dictionary<TerrainTypes, int>();
        totalScoreTarget = 0;
        totalScore = 0;

        foreach(TerrainTypes terrain in UIElement.Keys)
        {
            DisplayScore(terrain);
        }
    }
    
    public Medals CheckWinCondition() 
    {
        Medals achieved = Medals.GOLD;
        foreach (KeyValuePair<TerrainTypes, (int, int, int)> target in terrainTarget)
        {
            if(target.Value.Item1 == 0) continue;
            if(!terrainScores.ContainsKey(target.Key)) 
            { 
                achieved = Medals.NONE;
                return achieved; 
            }  
            
            if(terrainScores[target.Key] >= target.Value.Item3 && achieved == Medals.GOLD) { continue; }
            if(terrainScores[target.Key] >= target.Value.Item2 && achieved >= Medals.SILVER) 
            { 
                achieved = Medals.SILVER;
                continue;
            }
            if(terrainScores[target.Key] >= target.Value.Item1 && achieved >= Medals.BRONZE) 
            { 
                achieved = Medals.BRONZE;
                continue;
            }
            achieved = Medals.NONE;
            return achieved;
        }
        return achieved;
    }

    public void SetTargetScore(TerrainTypes type, (int, int, int) targets)
    {
        terrainTarget[type] = (targets.Item1, targets.Item2, targets.Item3);
        totalScoreTarget += targets.Item1;

        ScoreUI scoreBoard = UIElement[type];
        scoreBoard.TargetScore.text = targets.Item1.ToString();
        DisplayScore(type);

        totalTargetText.text = totalScoreTarget.ToString();
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
            int tileScore = scoringTile.GetAdjacentTileValue(tile.tileType);
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
    
        floatText.Show((score > 0 ? "+" : "") + score.ToString() + " points", score >= 0);
    }

    void DisplayScore(TerrainTypes type)
    {
        if(!UIElement.ContainsKey(type))
        {
            Debug.Log("Invalid tile type.");
            return;
        }

        int score = terrainScores.ContainsKey(type) ? terrainScores[type] : 0;
        (int, int, int) target = terrainTarget.ContainsKey(type) ? terrainTarget[type] : (score, score, score);
        ScoreUI scoreBoard = UIElement[type];

        if(target.Item1 == 0) 
        {
            scoreBoard.UIObject.SetActive(false);
            return;
        }

        scoreBoard.UIObject.SetActive(true);
        scoreBoard.ActualScore.text = score.ToString();
        scoreBoard.ScoreBar.fillAmount = (float) score / (target.Item1 == 0 ? 1f : (float) target.Item1);
        if(score >= target.Item1) { scoreBoard.ScoreCheck.sprite = Checkmark; }
        else { scoreBoard.ScoreCheck.sprite = Empty; }

        totalScoreText.text = totalScore.ToString();
    }

}
