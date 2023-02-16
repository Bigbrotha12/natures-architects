using UnityEngine;

[CreateAssetMenu(menuName = "CharacterSO")]
public class CharacterSO : ScriptableObject
{
    public Sprite defaultSprite;
    public AnimatorOverrideController animatorOverride;
    public TerrainTile terrainTile;
}