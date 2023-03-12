using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundFX : MonoBehaviour
{
    [SerializeField] AudioClip defaultMoveSFX;
    [SerializeField] AudioClip defaultMoveBlockedSFX;
    [SerializeField] AudioClip defaultPlaceTerrainSFX;
    [SerializeField] AudioClip defaultSpawnSound;

    IPlayerSFXs soundFXs;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        EventBroker.CharacterChange += OnCharacterChange;

        EventBroker.PlayerMove += PlayMoveSFX;
        EventBroker.PlayerMoveBlocked += PlayMoveBlockedSFX;
        EventBroker.PlaceTerrain += PlayPlaceTerrainSFX;
        EventBroker.SpawnCharacter += PlaySpawnSFX;
    }

    void OnDisable()
    {
        EventBroker.CharacterChange -= OnCharacterChange;

        EventBroker.PlayerMove -= PlayMoveSFX;
        EventBroker.PlayerMoveBlocked -= PlayMoveBlockedSFX;
        EventBroker.PlaceTerrain -= PlayPlaceTerrainSFX;
        EventBroker.SpawnCharacter -= PlaySpawnSFX;
    }

    private void OnCharacterChange(CharacterSO newCharacter)
    {
        soundFXs = newCharacter;
    }

    void PlayMoveSFX()
    {
        SetRandomPitch(0.95f, 1.0f);
        PlayOneOrOther(soundFXs.MoveSFX, defaultMoveSFX);
    }

    void PlayMoveBlockedSFX()
    {
        audioSource.pitch = 1.0f;
        PlayOneOrOther(soundFXs.MoveBlockedSFX, defaultMoveBlockedSFX);
    }

    void PlayPlaceTerrainSFX()
    {
        SetRandomPitch(0.95f, 1.0f);
        PlayOneOrOther(soundFXs.PlaceTerrainSFX, defaultPlaceTerrainSFX);
    }

    void PlaySpawnSFX()
    {
        audioSource.pitch = 1.0f;
        PlayOneOrOther(soundFXs.SpawnSFX, defaultSpawnSound);
    }

    void PlayOneOrOther(AudioClip mainClip, AudioClip altClip)
    {
        audioSource.PlayOneShot(mainClip != null ? mainClip : altClip);
    }

    void SetRandomPitch(float min, float max)
    {
        audioSource.pitch = UnityEngine.Random.Range(min, max);
    }
}
