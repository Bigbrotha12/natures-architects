using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
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

    [Header("Progress Bar")]
    [SerializeField] Image grassTargetBar;
    [SerializeField] Image mountainTargetBar;
    [SerializeField] Image forestTargetBar;
    [SerializeField] Image waterTargetBar;
    [SerializeField] Image snowTargetBar;
    [SerializeField] Image fireTargetBar;

    [Header("Checkmark")]
    [SerializeField] Sprite Checkmark;
    [SerializeField] Sprite Empty;
    [SerializeField] Image grassTargetCheck;
    [SerializeField] Image mountainTargetCheck;
    [SerializeField] Image forestTargetCheck;
    [SerializeField] Image waterTargetCheck;
    [SerializeField] Image snowTargetCheck;
    [SerializeField] Image fireTargetCheck;

    [Header("Object")]
    [SerializeField] GameObject grassTargetObject;
    [SerializeField] GameObject mountainTargetObject;
    [SerializeField] GameObject forestTargetObject;
    [SerializeField] GameObject waterTargetObject;
    [SerializeField] GameObject snowTargetObject;
    [SerializeField] GameObject fireTargetObject;

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
    
        floatText.Show((score > 0 ? "+" : "") + score.ToString() + " points", score >= 0);
    }

    void DisplayScore(TerrainTypes type)
    {
        int score, target;
        switch(type)
        {
            case TerrainTypes.NONE:
                break;
            case TerrainTypes.GRASS:
                grassTargetObject.SetActive(true);
                score = terrainScores.ContainsKey(type) ? terrainScores[type] : 0;
                target = terrainTarget.ContainsKey(type) ? terrainTarget[type] : score;
                grassScoreText.text = score.ToString();
                grassTargetBar.fillAmount = (float) score / (target == 0 ? 1f : (float) target);
                if(score >= target) { grassTargetCheck.sprite = Checkmark; }
                else { grassTargetCheck.sprite = Empty; }
                if(target == 0) grassTargetObject.SetActive(false);
                break;
            case TerrainTypes.FOREST:
            forestTargetObject.SetActive(true);
                score = terrainScores.ContainsKey(type) ? terrainScores[type] : 0;
                target = terrainTarget.ContainsKey(type) ? terrainTarget[type] : score;
                forestScoreText.text = score.ToString();
                forestTargetBar.fillAmount = (float) score / (target == 0 ? 1f : (float) target);
                if(score >= target) { forestTargetCheck.sprite = Checkmark; }
                else { forestTargetCheck.sprite = Empty; }
                if(target == 0) forestTargetObject.SetActive(false);
                break;
            case TerrainTypes.WATER:
            waterTargetObject.SetActive(true);
                score = terrainScores.ContainsKey(type) ? terrainScores[type] : 0;
                target = terrainTarget.ContainsKey(type) ? terrainTarget[type] : score;
                waterScoreText.text = score.ToString();
                waterTargetBar.fillAmount = (float) score / (target == 0 ? 1f : (float) target);
                if(score >= target) { waterTargetCheck.sprite = Checkmark; }
                else { waterTargetCheck.sprite = Empty; }
                if(target == 0) waterTargetObject.SetActive(false);
                break;
            case TerrainTypes.MOUNTAIN:
            mountainTargetObject.SetActive(true);
                score = terrainScores.ContainsKey(type) ? terrainScores[type] : 0;
                target = terrainTarget.ContainsKey(type) ? terrainTarget[type] : score;
                mountainScoreText.text = score.ToString();
                mountainTargetBar.fillAmount = (float) score / (target == 0 ? 1f : (float) target);
                if(score >= target) { mountainTargetCheck.sprite = Checkmark; }
                else { mountainTargetCheck.sprite = Empty; }
                if(target == 0) mountainTargetObject.SetActive(false);
                break;
            case TerrainTypes.FIRE:
            fireTargetObject.SetActive(true);
                score = terrainScores.ContainsKey(type) ? terrainScores[type] : 0;
                target = terrainTarget.ContainsKey(type) ? terrainTarget[type] : score;
                fireScoreText.text = score.ToString();
                fireTargetBar.fillAmount = (float) score / (target == 0 ? 1f : (float) target);
                if(score >= target) { fireTargetCheck.sprite = Checkmark; }
                else { fireTargetCheck.sprite = Empty; }
                if(target == 0) fireTargetObject.SetActive(false);
                break;
            case TerrainTypes.SNOW:
            snowTargetObject.SetActive(true);
                score = terrainScores.ContainsKey(type) ? terrainScores[type] : 0;
                target = terrainTarget.ContainsKey(type) ? terrainTarget[type] : score;
                snowScoreText.text = score.ToString();
                snowTargetBar.fillAmount = (float) score / (target == 0 ? 1f : (float) target);
                if(score >= target) { snowTargetCheck.sprite = Checkmark; }
                else { snowTargetCheck.sprite = Empty; }
                if(target == 0) snowTargetObject.SetActive(false);
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
                DisplayScore(TerrainTypes.GRASS);
                break;
            case TerrainTypes.FOREST:
                forestTargetText.text = target.ToString();
                DisplayScore(TerrainTypes.FOREST);
                break;
            case TerrainTypes.WATER:
                waterTargetText.text = target.ToString();
                DisplayScore(TerrainTypes.WATER);
                break;
            case TerrainTypes.MOUNTAIN:
                mountainTargetText.text = target.ToString();
                DisplayScore(TerrainTypes.MOUNTAIN);
                break;
            case TerrainTypes.FIRE:
                fireTargetText.text = target.ToString();
                DisplayScore(TerrainTypes.FIRE);
                break;
            case TerrainTypes.SNOW:
                snowTargetText.text = target.ToString();
                DisplayScore(TerrainTypes.SNOW);
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
