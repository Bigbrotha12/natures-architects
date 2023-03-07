using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundFX : MonoBehaviour
{
    [SerializeField] AudioClip defaultMoveSFX;
    [SerializeField] AudioClip defaultMoveBlockedSFX;
    [SerializeField] AudioClip defaultPlaceTerrainSFX;

    IPlayerSFXs soundFXs;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        EventBroker.CharacterChange += OnCharacterChange;

        EventBroker.PlayerMove += PlayMoveSFX;
        EventBroker.PlayerMoveBlocked += PlayMoveBlockedSFX;
        EventBroker.PlaceTerrain += PlayPlaceTerrainSFX;
    }

    void OnDisable()
    {
        EventBroker.CharacterChange -= OnCharacterChange;

        EventBroker.PlayerMove -= PlayMoveSFX;
        EventBroker.PlayerMoveBlocked -= PlayMoveBlockedSFX;
        EventBroker.PlaceTerrain -= PlayPlaceTerrainSFX;
    }

    private void OnCharacterChange(CharacterSO newCharacter)
    {
        soundFXs = newCharacter;
    }

    void PlayMoveSFX()
    {
        PlayOneOrOther(soundFXs.MoveSFX, defaultMoveSFX);
    }

    void PlayMoveBlockedSFX()
    {
        PlayOneOrOther(soundFXs.MoveBlockedSFX, defaultMoveBlockedSFX);
    }

    void PlayPlaceTerrainSFX()
    {
        PlayOneOrOther(soundFXs.PlaceTerrainSFX, defaultPlaceTerrainSFX);
    }

    void PlayOneOrOther(AudioClip mainClip, AudioClip altClip)
    {
        audioSource.pitch = UnityEngine.Random.Range(0.95f, 1.0f);
        audioSource.PlayOneShot(mainClip != null ? mainClip : altClip);
    }
}
