using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectHandler: MonoBehaviour {
    [SerializeField] Sprite filledStar;
    [SerializeField] Sprite emptyStar;
    [SerializeField] Sprite levelAvailable;
    [SerializeField] Color levelCompletedColor;
    [SerializeField] GameObject creaturePrefab;
    [SerializeField] GameObject levelPrefab;
    [SerializeField] LevelManager levelManager;
    [SerializeField] IPlayerProgressData playerSavedData;

    class progressTestData : ILevelProgressData
    {
        public bool available;
        public bool completed;
        public int score;
        public int stars;
        public bool Available(){return available;}
        public bool Completed(){return completed;}
        public int HighScore(){return score;}
        public Sprite MapImage()
        {
            throw new System.NotImplementedException();
        }
        public int StarsAwarded(){return stars;}
    }
    class playerTestData : IPlayerProgressData
    {
        public ILevelProgressData GetLevelProgress(int levelID)
        {
            return new progressTestData()
            {
                available = levelID < 5,
                completed = levelID < 3,
                score = levelID * 1000,
                stars = levelID % 3
            };
        }
    }

    void OnEnable() 
    {
        // Mock Test
        #region Test
        playerSavedData = new playerTestData(); 
        #endregion

        // Not sure why this line is needed
        //EventBroker.ReturnToTitleScreen += () => gameObject.SetActive(true);
        transform
            .Find("Border")
            .Find("LevelSelect")
            .Find("ExitButton")
            .GetComponent<Button>()
            .onClick
            .AddListener(() => EventBroker.CallReturnToTitleScreen());

        ClearLevelScore();
        ClearCreatureQueue();
        ClearScoreTargets();
        DisplayChapterLevels(levelManager.levels);
    }

    void OnDisable() 
    {
        ClearLevels();
        transform
            .Find("Border")
            .Find("LevelSelect")
            .Find("ExitButton")
            .GetComponent<Button>()
            .onClick
            .RemoveAllListeners();
        transform
            .Find("Border")
            .Find("LevelSelect")
            .Find("StartButton")
            .GetComponent<Button>()
            .onClick
            .RemoveAllListeners();
    }

    void UpdateLevelInfoPanels(GameLevelSO level)
    {
        ClearLevelScore();
        DisplayLevelScore(level);
        ClearCreatureQueue();
        DisplayCreatureQueue(level);
        DisplayScoreTargets(level);
    }

    void DisplayLevelScore(GameLevelSO level)
    {
        ILevelProgressData progress = playerSavedData.GetLevelProgress(level.levelID);
        Image bronzeStar = transform
            .Find("Border")
            .Find("LevelInfo")
            .Find("PlayerBestScore")
            .Find("StarBronze")
            .GetComponent<Image>();
        Image silverStar = transform
            .Find("Border")
            .Find("LevelInfo")
            .Find("PlayerBestScore")
            .Find("StarSilver")
            .GetComponent<Image>();
        Image goldStar = transform
            .Find("Border")
            .Find("LevelInfo")
            .Find("PlayerBestScore")
            .Find("StarGold")
            .GetComponent<Image>();
        TMP_Text highScore = transform
            .Find("Border")
            .Find("LevelInfo")
            .Find("PlayerBestScore")
            .Find("ScoreTitle")
            .Find("ScoreText")
            .GetComponent<TMP_Text>();
        
        highScore.text = progress.HighScore().ToString();
        switch (progress.StarsAwarded())
        {
            case 1:
                bronzeStar.sprite = filledStar;
                silverStar.sprite = emptyStar;
                goldStar.sprite = emptyStar;
                break;
            case 2:
                bronzeStar.sprite = filledStar;
                silverStar.sprite = filledStar;
                goldStar.sprite = emptyStar;
                break;
            case 3:
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

    void ClearLevelScore()
    {
        transform
            .Find("Border")
            .Find("LevelInfo")
            .Find("PlayerBestScore")
            .Find("StarBronze")
            .GetComponent<Image>()
            .sprite = emptyStar;
        transform
            .Find("Border")
            .Find("LevelInfo")
            .Find("PlayerBestScore")
            .Find("StarSilver")
            .GetComponent<Image>()
            .sprite = emptyStar;
        transform
            .Find("Border")
            .Find("LevelInfo")
            .Find("PlayerBestScore")
            .Find("StarGold")
            .GetComponent<Image>()
            .sprite = emptyStar;
        transform
            .Find("Border")
            .Find("LevelInfo")
            .Find("PlayerBestScore")
            .Find("ScoreTitle")
            .Find("ScoreText")
            .GetComponent<TMP_Text>()
            .text = "0";
    }

    void DisplayCreatureQueue(GameLevelSO level)
    {
        Transform container = transform
            .Find("Border")
            .Find("LevelInfo")
            .Find("AnimalQueue")
            .Find("AnimalContainer");

        CharacterUses[] creatures = level.AvailableCharacters;
        foreach (CharacterUses creature in creatures)
        {
            GameObject item = GameObject.Instantiate(creaturePrefab, container);
            item
                .transform
                .Find("ImageBorder")
                .Find("AnimalSprite")
                .GetComponent<Image>()
                .sprite = creature.CharacterSO.defaultSprite;
            item
                .transform
                .Find("TileTypeText")
                .GetComponent<TMP_Text>()
                .text = creature.CharacterSO.terrainTile.tileType.ToString();
            item
                .transform
                .Find("Uses")
                .Find("UsesText")
                .GetComponent<TMP_Text>()
                .text = creature.Uses.ToString();
        }
    }

    void ClearCreatureQueue()
    {
        Transform container = transform
            .Find("Border")
            .Find("LevelInfo")
            .Find("AnimalQueue")
            .Find("AnimalContainer");

        foreach (Transform item in container)
        {
            Destroy(item.gameObject);            
        }
    }

    void DisplayScoreTargets(GameLevelSO level)
    {
        GameLevelSO.ScoreTargets targets = level.levelTargets;
        Transform targetContainer = transform
            .Find("Border")
            .Find("LevelInfo")
            .Find("LevelTargetScore");
        
        // Grass target
        targetContainer
            .Find("TargetGrass")
            .Find("Target")
            .GetComponent<TMP_Text>()
            .text = targets.GrassTargets[0].ToString();

        // Forest target
        targetContainer
            .Find("TargetForest")
            .Find("Target")
            .GetComponent<TMP_Text>()
            .text = targets.ForestTargets[0].ToString();

        // Water target
        targetContainer
            .Find("TargetWater")
            .Find("Target")
            .GetComponent<TMP_Text>()
            .text = targets.WaterTargets[0].ToString();

        // Mountain target
        targetContainer
            .Find("TargetMountain")
            .Find("Target")
            .GetComponent<TMP_Text>()
            .text = targets.MountainTargets[0].ToString();

        // Fire target
        targetContainer
            .Find("TargetFire")
            .Find("Target")
            .GetComponent<TMP_Text>()
            .text = targets.FireTargets[0].ToString();

        // Snow target
        targetContainer
            .Find("TargetSnow")
            .Find("Target")
            .GetComponent<TMP_Text>()
            .text = targets.SnowTargets[0].ToString();
    }

    void ClearScoreTargets()
    {
        Transform targetContainer = transform
            .Find("Border")
            .Find("LevelInfo")
            .Find("LevelTargetScore");
        
        // Grass target
        targetContainer
            .Find("TargetGrass")
            .Find("Target")
            .GetComponent<TMP_Text>()
            .text = "0";

        // Forest target
        targetContainer
            .Find("TargetForest")
            .Find("Target")
            .GetComponent<TMP_Text>()
            .text = "0";

        // Water target
        targetContainer
            .Find("TargetWater")
            .Find("Target")
            .GetComponent<TMP_Text>()
            .text = "0";

        // Mountain target
        targetContainer
            .Find("TargetMountain")
            .Find("Target")
            .GetComponent<TMP_Text>()
            .text = "0";

        // Fire target
        targetContainer
            .Find("TargetFire")
            .Find("Target")
            .GetComponent<TMP_Text>()
            .text = "0";

        // Snow target
        targetContainer
            .Find("TargetSnow")
            .Find("Target")
            .GetComponent<TMP_Text>()
            .text = "0";
    }

    // stretch goal
    void DisplayChapterLevels(GameLevelSO[] levels)
    {
        Transform container = transform
            .Find("Border")
            .Find("LevelSelect")
            .Find("LevelContainer");
        Button startButton = transform
            .Find("Border")
            .Find("LevelSelect")
            .Find("StartButton")
            .GetComponent<Button>();
        
        foreach (GameLevelSO level in levels)
        {
            ILevelProgressData progressData = playerSavedData.GetLevelProgress(level.levelID);
            
            GameObject item = GameObject.Instantiate(levelPrefab, container);
            item
                .transform
                .Find("Border")
                .Find("LevelText")
                .GetComponent<TMP_Text>()
                .text = level.levelID.ToString();

            if(progressData.Available())
            {
                item.transform
                    .Find("Border")
                    .GetComponent<Image>()
                    .sprite = levelAvailable;
                item.transform
                    .Find("Border")
                    .Find("LevelText")
                    .GetComponent<TMP_Text>()
                    .color = Color.white;
                item.GetComponent<Button>().onClick.AddListener(() => 
                {
                    UpdateLevelInfoPanels(level);
                    levelManager.SetLevelIndex(level.levelID);
                    startButton.onClick.RemoveAllListeners();
                    startButton.onClick.AddListener(() => 
                    {
                        levelManager.InitializeLevel();
                        gameObject.SetActive(false);
                    });
                }); 
            }
            
            if(progressData.Completed())
            {
                item.GetComponent<Image>().color = Color.green;
            }
        }
    }

    void ClearLevels()
    {
        Transform container = transform
            .Find("Border")
            .Find("LevelSelect")
            .Find("LevelContainer");
        
        foreach (Transform item in container)
        {
            Destroy(item.gameObject);
        }
    }
}