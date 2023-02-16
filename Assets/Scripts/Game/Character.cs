using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] CharacterSO characterSO;

    SpriteRenderer spriteRenderer;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = characterSO.defaultSprite;
    }

    public TerrainTile TerrainTile
    {
        get { return characterSO.terrainTile; }
    }

    public void ChangeCharacter(CharacterSO newCharacter)
    {
        characterSO = newCharacter;
        spriteRenderer.sprite = characterSO.defaultSprite;
    }
}
