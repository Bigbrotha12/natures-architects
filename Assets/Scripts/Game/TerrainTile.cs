using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(menuName = "TerrainTile")]
public class TerrainTile : ScriptableObject
{
    public TerrainTypes tileType;
    public Tile tileSprite;
    public int value;
}
