using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
 

[CreateAssetMenu(menuName = "TerrainTile")]
public class TerrainTile : TileBase
{
    public TerrainTypes tileType;
    public Tile tileSprite;
    public bool walkable;
    public int adjacentGrassValue;
    public int adjacentWaterValue;
    public int adjacentMountainValue;
    public int adjacentForestValue;
    public int adjacentSnowValue;
    public int adjacentFireValue;

    // Tile Animation
    public float AnimationSpeed = 1f;
    public float AnimationStartTime = 0f;

    // Tile Rule
    [System.Serializable]
    class AnimatedTileRule {
        public List<TerrainTypes> targetTerrains;
        public Sprite[] Base;
        public Sprite[] UpOnly, LeftOnly, DownOnly, RightOnly;
        public Sprite[] UpLeft, LeftDown, DownRight, RightUp, LeftRight, UpDown;
        public Sprite[] UpLeftDown, LeftDownRight, DownRightUp, RightUpLeft;
        public Sprite[] AllSides;

        public Sprite[] GetRuleSprite(bool Up, bool Left, bool Down, bool Right) {
            if(Up) {
                if(Left) {
                    if(Right) {
                        if(Down) {
                            return AllSides;
                        } else {
                            return RightUpLeft;
                        }
                    } else {
                        if(Down) {
                            return UpLeftDown;
                        } else {
                            return UpLeft;
                        }
                    }
                } else {
                    if(Right) {
                        if(Down) {
                            return DownRightUp;
                        } else {
                            return RightUp;
                        }
                    } else {
                        if(Down) {
                            return UpDown;
                        } else {
                            return UpOnly;
                        }
                    }
                }
            } else {
                if(Left) {
                    if(Right) {
                        if(Down) {
                            return LeftDownRight;
                        } else {
                            return LeftRight;
                        }
                    } else {
                        if(Down) {
                            return LeftDown;
                        } else {
                            return LeftOnly;
                        }
                    }
                } else {
                    if(Right) {
                        if(Down) {
                            return DownRight;
                        } else {
                            return RightOnly;
                        }
                    } else {
                        if(Down) {
                            return DownOnly;
                        } else {
                            return Base;
                        }
                    }
                }
            }
            
        }
    }
    [SerializeField] AnimatedTileRule tileRule;
    Sprite[] currentAnimatedTile;

    public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
    {
        if(currentAnimatedTile is not null) {
            tileAnimationData.animatedSprites = currentAnimatedTile;
        } else {
            (bool Up, bool Left, bool Down, bool Right) = GetTileRuleLocations(position, tilemap, tileRule.targetTerrains);
            tileAnimationData.animatedSprites = tileRule.GetRuleSprite(Up, Left, Down, Right);
        }
        tileAnimationData.animationSpeed = AnimationSpeed;
        tileAnimationData.animationStartTime = AnimationStartTime;
        return true;
    }

    public int GetAdjacentTileValue(TerrainTypes type)
    {
        switch (type)
        {
            case TerrainTypes.Grass:
                return adjacentGrassValue;
            case TerrainTypes.Forest:
                return adjacentForestValue;
            case TerrainTypes.Water:
                return adjacentWaterValue;
            case TerrainTypes.Mountain:
                return adjacentMountainValue;
            case TerrainTypes.Fire:
                return adjacentFireValue;
            case TerrainTypes.Snow:
                return adjacentSnowValue;
            default:
                return 0;
        }
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        if(tileRule is not null && tileRule.targetTerrains.Count > 0) 
        {
            (bool Up, bool Left, bool Down, bool Right) = GetTileRuleLocations(position, tilemap, tileRule.targetTerrains);
            Sprite[] ruledTile = tileRule.GetRuleSprite(Up, Left, Down, Right);
            currentAnimatedTile = ruledTile;
            tileData.sprite = ruledTile.Length > 0 ? ruledTile[0] : tileSprite.sprite;

        } else
        {
            tileSprite.GetTileData(position, tilemap, ref tileData);
        }
    }

    (bool, bool, bool, bool) GetTileRuleLocations(Vector3Int position, ITilemap tilemap, List<TerrainTypes> targetTiles) 
    {
        TerrainTile terrainUp = tilemap.GetTile<TerrainTile>(new Vector3Int(position.x, position.y + 1, 0));
        TerrainTile terrainLeft = tilemap.GetTile<TerrainTile>(new Vector3Int(position.x - 1, position.y, 0));
        TerrainTile terrainDown = tilemap.GetTile<TerrainTile>(new Vector3Int(position.x, position.y - 1, 0));
        TerrainTile terrainRight = tilemap.GetTile<TerrainTile>(new Vector3Int(position.x + 1, position.y, 0));
        bool Up = terrainUp is not null ? targetTiles.Contains(terrainUp.tileType) : false;
        bool Left = terrainLeft is not null ? targetTiles.Contains(terrainLeft.tileType) : false;
        bool Down = terrainDown is not null ? targetTiles.Contains(terrainDown.tileType) : false;
        bool Right = terrainRight is not null ? targetTiles.Contains(terrainRight.tileType) : false;
        return (Up, Left, Down, Right);
    }

    public List<string> GetScoringKeyText() 
    {
        List<string> scoringText = new List<string>();
        if(adjacentGrassValue != 0)
        {
            string key = adjacentGrassValue > 0 ? "+ " : "";
            key += adjacentGrassValue.ToString() + " GRASS."; 
            scoringText.Add(key);
        }

        if(adjacentWaterValue != 0)
        {
            string key = adjacentWaterValue > 0 ? "+ " : "";
            key += adjacentWaterValue.ToString() + " WATER."; 
            scoringText.Add(key);
        }
        
        if(adjacentMountainValue != 0)
        {
            string key = adjacentMountainValue > 0 ? "+ " : "";
            key += adjacentMountainValue.ToString() + " MOUNTAIN."; 
            scoringText.Add(key);
        }

        if(adjacentForestValue != 0)
        {
            string key = adjacentForestValue > 0 ? "+ " : "";
            key += adjacentForestValue.ToString() + " FOREST."; 
            scoringText.Add(key);
        }

        if(adjacentSnowValue != 0)
        {
            string key = adjacentSnowValue > 0 ? "+ " : "";
            key += adjacentSnowValue.ToString() + " SNOW."; 
            scoringText.Add(key);
        }

        if(adjacentFireValue != 0)
        {
            string key = adjacentFireValue > 0 ? "+ " : "";
            key += adjacentFireValue.ToString() + " FIRE."; 
            scoringText.Add(key);
        }
        return scoringText;
    }

    public List<(TerrainTypes, int)> GetScoringKey()
    {
        List<(TerrainTypes, int)> scoring = new List<(TerrainTypes, int)>();
        if(adjacentGrassValue != 0) scoring.Add((TerrainTypes.Grass, adjacentGrassValue));
        if(adjacentWaterValue != 0) scoring.Add((TerrainTypes.Water, adjacentWaterValue));
        if(adjacentMountainValue != 0) scoring.Add((TerrainTypes.Mountain, adjacentMountainValue));
        if(adjacentForestValue != 0) scoring.Add((TerrainTypes.Forest, adjacentForestValue));
        if(adjacentSnowValue != 0) scoring.Add((TerrainTypes.Snow, adjacentSnowValue));
        if(adjacentFireValue != 0) scoring.Add((TerrainTypes.Fire, adjacentFireValue));
        return scoring;
    }
}
