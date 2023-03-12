using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "TerrainTile")]
public class TerrainTile : TileBase
{
    public TerrainTypes tileType;
    public Tile tileSprite;
    public int adjacentGrassValue;
    public int adjacentWaterValue;
    public int adjacentMountainValue;
    public int adjacentForestValue;
    public int adjacentSnowValue;
    public int adjacentFireValue;

    public int GetAdjacentTileValue(TerrainTypes type)
    {
        switch (type)
        {
            case TerrainTypes.GRASS:
                return adjacentGrassValue;
            case TerrainTypes.FOREST:
                return adjacentForestValue;
            case TerrainTypes.WATER:
                return adjacentWaterValue;
            case TerrainTypes.MOUNTAIN:
                return adjacentMountainValue;
            case TerrainTypes.FIRE:
                return adjacentFireValue;
            case TerrainTypes.SNOW:
                return adjacentSnowValue;
            default:
                return 0;
        }
    }

    public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
    {
        tileSprite.GetTileData(position, tilemap, ref tileData);
    }

    public List<string> GetScoringKeyText() 
    {
        List<string> scoringText = new List<string>();
        if(adjacentGrassValue != 0)
        {
            string key = adjacentGrassValue > 0 ? "+ " : "";
            key += adjacentGrassValue.ToString() + " for adjacent GRASS."; 
            scoringText.Add(key);
        }

        if(adjacentWaterValue != 0)
        {
            string key = adjacentWaterValue > 0 ? "+ " : "";
            key += adjacentWaterValue.ToString() + " for adjacent WATER."; 
            scoringText.Add(key);
        }
        
        if(adjacentMountainValue != 0)
        {
            string key = adjacentMountainValue > 0 ? "+ " : "";
            key += adjacentMountainValue.ToString() + " for adj. MOUNTAIN."; 
            scoringText.Add(key);
        }

        if(adjacentForestValue != 0)
        {
            string key = adjacentForestValue > 0 ? "+ " : "";
            key += adjacentForestValue.ToString() + " for adjacent FOREST."; 
            scoringText.Add(key);
        }

        if(adjacentSnowValue != 0)
        {
            string key = adjacentSnowValue > 0 ? "+ " : "";
            key += adjacentSnowValue.ToString() + " for adjacent SNOW."; 
            scoringText.Add(key);
        }

        if(adjacentFireValue != 0)
        {
            string key = adjacentFireValue > 0 ? "+ " : "";
            key += adjacentFireValue.ToString() + " for adjacent FIRE."; 
            scoringText.Add(key);
        }
        return scoringText;
    }
}
