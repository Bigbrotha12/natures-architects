using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    [SerializeField] CharacterSO characterSO;
    [SerializeField] RuntimeAnimatorController baseAnimatorController;

    SpriteRenderer spriteRenderer;
    Animator animator;

    public CharacterSO CharacterSO { get { return characterSO; } }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        //SetupCharacter();
    }

    public TerrainTile TerrainTile
    {
        get { return characterSO.terrainTile; }
    }

    public void ChangeCharacter(CharacterSO newCharacter)
    {
        characterSO = newCharacter;
        SetupCharacter();
        EventBroker.CallCharacterChange(newCharacter);
    }

    void SetupCharacter()
    {
        if (characterSO.animatorOverride != null)
        {
            animator.runtimeAnimatorController = characterSO.animatorOverride;
        }
        else
        {
            animator.runtimeAnimatorController = baseAnimatorController;
        }
        spriteRenderer.sprite = characterSO.defaultSprite;
    }
}
