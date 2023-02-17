using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "TerrainTile")]
public class TerrainTile : ScriptableObject
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
}
