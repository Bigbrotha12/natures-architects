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
    bool isReady;

    public CharacterSO CharacterSO { get { return characterSO; } }
    public bool IsReady { get { return isReady; } }
   
    public TerrainTile TerrainTile
    {
        get { return characterSO.terrainTile; }
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    public void ShowCharacter(bool show)
    {
        spriteRenderer.enabled = show;
    }

    public void ChangeCharacter(CharacterSO newCharacter)
    {
        isReady = false;
        characterSO = newCharacter;
        EventBroker.CallCharacterChange(newCharacter);
        SetupCharacter();
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
        SpawnEffects();
    }

    void SpawnEffects()
    {
        animator.enabled = true;
        ShowCharacter(true);
        animator.SetTrigger("Spawn");
        EventBroker.CallSpawnCharacter();
    }

    public void SpawnAnimationComplete()
    {
        print("Spawn complete");
        isReady = true;
    }
}
