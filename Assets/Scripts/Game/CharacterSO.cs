using UnityEngine;

[CreateAssetMenu(menuName = "CharacterSO")]
public class CharacterSO : ScriptableObject, IPlayerSFXs
{
    public Sprite defaultSprite;
    public AnimatorOverrideController animatorOverride;
    public TerrainTile terrainTile;

    [SerializeField] AudioClip moveSFX;
    [SerializeField] AudioClip moveBlockedSFX;
    [SerializeField] AudioClip placeTerrainSFX;
    [SerializeField] AudioClip spawnSFX;

    public AudioClip MoveSFX { get { return moveSFX; } }
    public AudioClip MoveBlockedSFX { get { return moveBlockedSFX; } }
    public AudioClip PlaceTerrainSFX { get { return placeTerrainSFX; } }
    public AudioClip SpawnSFX { get { return spawnSFX; } }
}

public interface IPlayerSFXs
{
    public AudioClip MoveSFX { get; }
    public AudioClip MoveBlockedSFX { get; }
    public AudioClip PlaceTerrainSFX { get; }
    public AudioClip SpawnSFX { get; }
}