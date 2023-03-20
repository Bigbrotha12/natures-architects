using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class LevelInfoHandler : MonoBehaviour 
{
    [SerializeField] GameObject creaturePrefab;
    [SerializeField] GameObject scoringKeyPrefab;
    [SerializeField] GameObject scoringItemPrefab;
    [SerializeField] LevelManager levelManager;
    [SerializeField] MapGrid map;

    TMP_Text storyText;

    void Awake()
    {
        storyText = transform
            .Find("Border")
            .Find("StoryBoard")
            .Find("Border")
            .Find("StoryText")
            .GetComponent<TMP_Text>();
    }

    void OnEnable()
    {
        ClearCreatureQueue();
        ClearScoringKey();
        DisplayStoryText(map.gameLevel);
        DisplayCreatureQueue(map.gameLevel);
        DisplayScoringKey();
    }

    void DisplayStoryText(GameLevelSO level)
    {
        storyText.text = level.FlavorTexts.IntroText;
    }

    void ClearStoryText()
    {
        storyText.text = "";
    }

    void DisplayCreatureQueue(GameLevelSO level)
    {
        Transform container = transform
            .Find("Border")
            .Find("AnimalBoard")
            .Find("Border");
        CharacterUses[] characters = level.AvailableCharacters;
        foreach (CharacterUses character in characters)
        {
            GameObject item = GameObject.Instantiate(creaturePrefab, container);
            item
                .transform
                .Find("ImageBorder")
                .Find("AnimalSprite")
                .GetComponent<Image>()
                .sprite = character.CharacterSO.defaultSprite;
            item
                .transform
                .Find("Uses")
                .Find("UsesText")
                .GetComponent<TMP_Text>()
                .text = character.Uses.ToString();
        }
    }

    void ClearCreatureQueue()
    {
        Transform container = transform
            .Find("Border")
            .Find("AnimalBoard")
            .Find("Border");

        foreach (Transform item in container)
        {
            Destroy(item.gameObject);
        }
    }

    void DisplayScoringKey()
    {
        Transform container = transform
            .Find("Border")
            .Find("FullScoringKey")
            .Find("Border");

        foreach(TerrainTile availableTiles in map.TerrainIndex)
        {
            if (availableTiles.tileType == TerrainTypes.None || availableTiles.tileType == TerrainTypes.Blocked)
            {
                continue;
            }
            List<(TerrainTypes, int)> terrainScores = availableTiles.GetScoringKey();
            GameObject item = GameObject.Instantiate(scoringKeyPrefab, container);

            item.transform
                .Find("ScoringTileText")
                .GetComponent<TMP_Text>()
                .text = availableTiles.tileType.ToString();
            Transform keyContainer = item.transform.Find("KeyContainer");
            foreach ((TerrainTypes terrain, int score) in terrainScores)
            {
                string prefix = score > 0 ? "+ " : "- ";
                Color textColor = score > 0 ? new Color(0.1f, 0.76f, 0.1f, 1.0f) : new Color(0.76f, 0.1f, 0.1f);
                GameObject scoreObject = GameObject.Instantiate(scoringItemPrefab, keyContainer);
                scoreObject.GetComponent<TMP_Text>().text = prefix + Mathf.Abs(score).ToString() + " " + terrain.ToString();
                scoreObject.GetComponent<TMP_Text>().color = textColor;
            }
        }
    }

    void ClearScoringKey()
    {
        Transform container = transform
            .Find("Border")
            .Find("FullScoringKey")
            .Find("Border");

        foreach (Transform item in container)
        {
            Destroy(item.gameObject);
        }
    }
}