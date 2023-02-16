using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class MapGrid : MonoBehaviour
{
    public Tile tile;
    public Tilemap tilemap;
    public TerrainTile[,] Map;
    public int height, width, columns, rows;
    public TerrainTile[] initialTerrains;
    public TMP_Text totalScore;
    public TMP_Text grassScore;
    public TMP_Text mountainScore;
    public TMP_Text forestScore;
    public TMP_Text waterScore;
    public Dictionary<TerrainTypes, int> terrainScores = new Dictionary<TerrainTypes, int>();

    void Start()
    {
        // Create random map tiles
        Map = new TerrainTile[columns, rows];
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                int terrainIndex = Random.Range(0, initialTerrains.Length);
                CreateTile(i, j, initialTerrains[terrainIndex]);
            }
        }
        ScoreMap();
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
        // check position is within grid
        if(position.x < 0 || position.x >= columns || position.y < 0 || position.y >= rows) 
        {
            Debug.LogError("Invalid Map position.");   
            return false;
        }
        return true;
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

        int score = Scorer.ScoreTile(tile, validAdjacentTiles.ToArray());
        if(!terrainScores.ContainsKey(tile.tileType))
        {
            terrainScores[tile.tileType] = score;
        } else 
        {
            terrainScores[tile.tileType] += score;
        }
        

        Debug.Log(terrainScores[tile.tileType]);
    }

    public void ScoreMap()
    {
        for (int i = 0; i < columns; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                ScoreTile(new Vector3Int(i, j, 0));
            }
        }
    }
}
