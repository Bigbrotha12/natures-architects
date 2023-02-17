using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class MapGrid : MonoBehaviour
{
    public Tilemap tilemap;
    public GameLevelSO gameLevel;
    public TerrainTile[,] Map;
    [SerializeField] Scorer ScoreBoard;
    
    void Start()
    {
        GenerateRandomTiles();
        SetTargets();
    }

    void GenerateRandomTiles()
    {
        Map = new TerrainTile[gameLevel.columns, gameLevel.rows];
        for (int i = 0; i < gameLevel.columns; i++)
        {
            for (int j = 0; j < gameLevel.rows; j++)
            {
                CreateTile(i, j, gameLevel.starterTile);
            }
        }
    }

    void SetTargets()
    {
        ScoreBoard.SetTargetScore(TerrainTypes.GRASS, gameLevel.grassTargetScore);
        ScoreBoard.SetTargetScore(TerrainTypes.FOREST, gameLevel.forestTargetScore);
        ScoreBoard.SetTargetScore(TerrainTypes.WATER, gameLevel.waterTargetScore);
        ScoreBoard.SetTargetScore(TerrainTypes.MOUNTAIN, gameLevel.mountainTargetScore);
        ScoreBoard.SetTargetScore(TerrainTypes.FIRE, gameLevel.fireTargetScore);
        ScoreBoard.SetTargetScore(TerrainTypes.SNOW, gameLevel.snowTargetScore);
    }

    public void CreateTile(int positionX, int positionY, TerrainTile type) 
    {
        if(Map is null) 
        {
            Debug.LogError("Map Grid not instantiated.");
            return;
        }

        Vector3Int position = new Vector3Int(positionX, positionY, 0);
        if(CheckPositionIsOnMapGrid(position))
        {
            Map[position.x, position.y] = type;
            tilemap.SetTile(position, type.tileSprite);
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

    public bool CheckTileEmpty(Vector3Int position)
    {
        if(CheckPositionIsOnMapGrid(position))
        {
            return Map[position.x, position.y].tileType == TerrainTypes.NONE;
        }
        return false;
    }

    public void ScoreTile(Vector3Int position)
    {
        if(!CheckPositionIsOnMapGrid(position)) { return; }

        TerrainTile tile = Map[position.x, position.y];
        Vector3Int[] possiblePositions = {
            new Vector3Int(position.x + 1, position.y, 0),
            new Vector3Int(position.x - 1, position.y, 0),
            new Vector3Int(position.x, position.y + 1, 0),
            new Vector3Int(position.x, position.y - 1, 0)
        };
        List<TerrainTile> validAdjacentTiles = new List<TerrainTile>();
        
        foreach(Vector3Int positionTile in possiblePositions)
        {
            if(CheckPositionIsOnMapGrid(positionTile))
            {
                validAdjacentTiles.Add(Map[positionTile.x, positionTile.y]);
            }
        }

        ScoreBoard.ScoreTile(tile, validAdjacentTiles.ToArray());
    }

    public void ScoreMap()
    {
        for (int i = 0; i < gameLevel.columns; i++)
        {
            for (int j = 0; j < gameLevel.rows; j++)
            {
                ScoreTile(new Vector3Int(i, j, 0));
            }
        }
    }
}
