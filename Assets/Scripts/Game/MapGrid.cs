using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public enum TerrainTypes {
    NONE,
    GRASS,
    FOREST,
    WATER,
    MOUNTAIN,
    FIRE,
    SNOW
}

public class MapGrid : MonoBehaviour
{
    public Tile tile;
    public Tilemap tilemap;
    public int[,] Map;
    public int height, width, columns, rows;

    void Start()
    {
        Map = new int[rows, columns];
        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < columns; j++)
            {
                //CreateTile(i, j, TerrainTypes.GRASS);
            }
        }
    }

    void CreateTile(int positionX, int positionY, TerrainTypes type) 
    {
        if(Map is null) 
        {
            Debug.LogError("Map Grid not instantiated.");
            return;
        }

        Vector3Int position = new Vector3Int(positionX, positionY, 0);
        tilemap.SetTile(position, tile);
    }

    public bool CheckPositionIsOnMapGrid(Vector3Int position)
    {
        // TODO
        return true;
    }
}
