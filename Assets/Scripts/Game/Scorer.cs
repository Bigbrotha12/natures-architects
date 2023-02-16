using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scorer : MonoBehaviour
{
    public static int ScoreTile(TerrainTile scoringTile, TerrainTile[] adjacentTiles)
    {
        if(scoringTile is null) 
        {
            Debug.LogError("Scoring Tile cannot be null.");
            return 0;
        }
        // Each tile score +1 for adjacent tiles of same type.
        int score = 0;
        foreach(TerrainTile tile in adjacentTiles)
        {
            Debug.Log("Scoring tile type: " + tile.tileType.ToString());
            if(tile.tileType == scoringTile.tileType)
            {
                score += 1;
            }
        }
        return score;
    }
}
