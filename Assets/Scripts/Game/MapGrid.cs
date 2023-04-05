using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGrid : MonoBehaviour
{
    public GameLevelSO gameLevel;
    Tilemap mutableMap;
    [SerializeField] TMP_Text storyText;
    [SerializeField] Scorer ScoreBoard;
    public TerrainTile[] TerrainIndex;
    
    public void SetLevel(GameLevelSO levelSO)
    {
        gameLevel = levelSO;
        GenerateTileMap();
        SetTargets();
    }

    void SetTargets()
    {
        ScoreBoard.SetTargetScore(TerrainTypes.Grass, (gameLevel.levelTargets.GrassTargets[0], gameLevel.levelTargets.GrassTargets[1], gameLevel.levelTargets.GrassTargets[2]));
        ScoreBoard.SetTargetScore(TerrainTypes.Forest, (gameLevel.levelTargets.ForestTargets[0], gameLevel.levelTargets.ForestTargets[1], gameLevel.levelTargets.ForestTargets[2]));
        ScoreBoard.SetTargetScore(TerrainTypes.Water, (gameLevel.levelTargets.WaterTargets[0], gameLevel.levelTargets.WaterTargets[1], gameLevel.levelTargets.WaterTargets[2]));
        ScoreBoard.SetTargetScore(TerrainTypes.Mountain, (gameLevel.levelTargets.MountainTargets[0], gameLevel.levelTargets.MountainTargets[1], gameLevel.levelTargets.MountainTargets[2]));
        ScoreBoard.SetTargetScore(TerrainTypes.Fire, (gameLevel.levelTargets.FireTargets[0], gameLevel.levelTargets.FireTargets[1], gameLevel.levelTargets.FireTargets[2]));
        ScoreBoard.SetTargetScore(TerrainTypes.Snow, (gameLevel.levelTargets.SnowTargets[0], gameLevel.levelTargets.SnowTargets[1], gameLevel.levelTargets.SnowTargets[2]));
    }

    public void CreateTile(int positionX, int positionY, TerrainTile type) 
    {
        if(gameLevel.map is null) 
        {
            Debug.LogError("Map Grid not instantiated.");
            return;
        }

        Vector3Int position = new Vector3Int(positionX, positionY, 0);
        if(CheckPositionIsOnMapGrid(position))
        {
            mutableMap.SetTile(position, type);
        }
    }

    public bool CheckPositionIsOnMapGrid(Vector3Int position)
    {
        if(position.x < 0 || position.x >= gameLevel.columns || position.y < 0 || position.y >= gameLevel.rows) 
        {
            return false;
        }
        return true;
    }

    public bool CheckTileIsWalkable(Vector3Int position)
    {
        if (mutableMap.GetTile<TerrainTile>(position) is null) return false;
        if (!mutableMap.GetTile<TerrainTile>(position).walkable) return false;
        if (mutableMap.GetTile<TerrainTile>(position).tileType == TerrainTypes.Blocked) return false;
        return true;
    }

    public TerrainTypes GetTileType(Vector3Int position)
    {
        if(CheckPositionIsOnMapGrid(position))
        {
            return mutableMap.GetTile<TerrainTile>(position).tileType;
        }
        return TerrainTypes.Blocked;
    }

    public void ScoreTile(Vector3Int position)
    {
        if(!CheckPositionIsOnMapGrid(position)) { return; }

        TerrainTile tile = mutableMap.GetTile<TerrainTile>(position);
        Vector3Int[] possiblePositions = {
            new Vector3Int(position.x + 1, position.y, 0),
            new Vector3Int(position.x - 1, position.y, 0),
            new Vector3Int(position.x, position.y + 1, 0),
            new Vector3Int(position.x, position.y - 1, 0)
        };
        List<TerrainTile> validAdjacentTiles = new List<TerrainTile>();
        
        foreach(Vector3Int adjacentPosition in possiblePositions)
        {
            if(CheckPositionIsOnMapGrid(adjacentPosition))
            {
                TerrainTile adjacentTile = mutableMap.GetTile<TerrainTile>(adjacentPosition);
                validAdjacentTiles.Add(adjacentTile);
            }
        }

        ScoreBoard.ScoreTile(tile, validAdjacentTiles.ToArray());
    }

    public void GenerateTileMap()
    {
        // Create new mutable tilemap
        Tilemap levelTilemap = gameLevel.map.GetComponent<Tilemap>();
        mutableMap = transform.Find("MutableMap").GetComponent<Tilemap>();
        mutableMap.ClearAllTiles();
        for (int i = levelTilemap.cellBounds.xMin; i < levelTilemap.cellBounds.xMax; i++)
        {
            for(int j = levelTilemap.cellBounds.yMin; j < levelTilemap.cellBounds.yMax; j++)
            {
                Vector3Int position = new Vector3Int(i, j, 0);
                mutableMap.SetTile(position, levelTilemap.GetTile(position));
            }
        }
    }
}
