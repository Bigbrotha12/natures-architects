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
   
    public TerrainTile TerrainTile
    {
        get { return characterSO.terrainTile; }
    }

    void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        animator = GetComponent<Animator>();
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
        animator.SetTrigger("Spawn");
    }

    public IEnumerator DeathAnimation()
    {
        // TODO: 
        print("Death animation");
        yield return new WaitForSecondsRealtime(0.2f);
        yield break;
    }
}
