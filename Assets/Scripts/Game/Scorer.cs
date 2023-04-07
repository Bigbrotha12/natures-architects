using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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

    [Header("Current stars")]
    [SerializeField] Image bronzeStar;
    [SerializeField] Image silverStar;
    [SerializeField] Image goldStar;
    [SerializeField] Sprite filledStar;
    [SerializeField] Sprite emptyStar;

    [SerializeField] TMP_Text totalScoreText;
    [SerializeField] TMP_Text totalTargetText;
    [SerializeField] Sprite Checkmark;
    [SerializeField] Sprite Empty;
    [SerializeField] FloatingTextManager floatText;
    int currentTotalScore = 0;
    int bronzeTotalTarget = 0;
    int silverTotalTarget = 0;
    int goldTotalTarget = 0;
    
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
        bronzeTotalTarget = 0;
        silverTotalTarget = 0;
        goldTotalTarget = 0;
        currentTotalScore = 0;

        foreach(TerrainTypes terrain in UIElement.Keys)
        {
            DisplayScore(terrain);
        }
    }
    
    public Medals CheckWinCondition() 
    {
        if (!CheckAllTerrainsCompleted()) return Medals.NONE;

        if (currentTotalScore >= goldTotalTarget) return Medals.GOLD;
        if (currentTotalScore >= silverTotalTarget) return Medals.SILVER;
        return Medals.BRONZE;
    }

    bool CheckAllTerrainsCompleted()
    {
        foreach (KeyValuePair<TerrainTypes, (int, int, int)> target in terrainTarget)
        {
            if (target.Value.Item1 == 0) continue;
            if (!terrainScores.ContainsKey(target.Key))
            {
                return false;
            }
            if (terrainScores[target.Key] < target.Value.Item1)
            {
                return false;
            }
        }
        return true;
    }

    public void SetTargetScore(TerrainTypes type, (int, int, int) targets)
    {
        terrainTarget[type] = (targets.Item1, targets.Item2, targets.Item3);

        bronzeTotalTarget += targets.Item1;
        silverTotalTarget += targets.Item2;
        goldTotalTarget += targets.Item3;

        ScoreUI scoreBoard = UIElement[type];
        scoreBoard.TargetScore.text = targets.Item1.ToString();
        DisplayScore(type);
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
        return currentTotalScore;
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
        currentTotalScore += score;
    
        floatText.Show((score > 0 ? "+" : "") + score.ToString() + " points", score >= 0);
    }

    void DisplayScore(TerrainTypes type)
    {
        if(!UIElement.ContainsKey(type))
        {
            Debug.LogError("Invalid tile type.");
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

        totalScoreText.text = currentTotalScore.ToString();
        Medals currentMedal = CheckWinCondition();
        if (currentMedal == Medals.GOLD || !CheckAllTerrainsCompleted())
        {
            totalTargetText.text = "";
        }
        else
        {
            totalTargetText.text = "Next star: " + GetNextStarTargetScore(currentMedal);
        }
        ShowCurrentMedals(currentMedal);
    }

    void ShowCurrentMedals(Medals currentMedal)
    {
        switch (currentMedal)
        {
            case Medals.BRONZE:
                bronzeStar.sprite = filledStar;
                silverStar.sprite = emptyStar;
                goldStar.sprite = emptyStar;
                break;
            case Medals.SILVER:
                bronzeStar.sprite = filledStar;
                silverStar.sprite = filledStar;
                goldStar.sprite = emptyStar;
                break;
            case Medals.GOLD:
                bronzeStar.sprite = filledStar;
                silverStar.sprite = filledStar;
                goldStar.sprite = filledStar;
                break;
            default:
                bronzeStar.sprite = emptyStar;
                silverStar.sprite = emptyStar;
                goldStar.sprite = emptyStar;
                break;
        }
    }

    int GetNextStarTargetScore(Medals currentMedal)
    {
        switch (currentMedal)
        {
            case Medals.BRONZE:
                return silverTotalTarget;
            case Medals.SILVER:
                return goldTotalTarget;
            case Medals.GOLD:
                return 0;
            default:
                break;
        }
        return 0;
    }
}
