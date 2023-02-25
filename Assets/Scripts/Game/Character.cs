using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] CharacterSO characterSO;

    SpriteRenderer spriteRenderer;
    Animator animator;

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        SetupCharacter(characterSO);
    }

    public TerrainTile TerrainTile
    {
        get { return characterSO.terrainTile; }
    }

    public void ChangeCharacter(CharacterSO newCharacter)
    {
        characterSO = newCharacter;
        SetupCharacter(newCharacter);
    }

    void SetupCharacter(CharacterSO characterSO)
    {
        if (characterSO.defaultSprite != null)
        {
            spriteRenderer.sprite = characterSO.defaultSprite;
        }
        if (characterSO.animatorOverride != null)
        {
            animator.runtimeAnimatorController = characterSO.animatorOverride;
        }
    }
}
