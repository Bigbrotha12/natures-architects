using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundFX : MonoBehaviour
{
    [SerializeField] AudioClip moveSFX;
    [SerializeField] AudioClip moveBlockedSFX;
    [SerializeField] AudioClip placeTerrainSFX;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();

        EventBroker.PlayerMove += PlayMoveSFX;
        EventBroker.PlayerMoveBlocked += PlayMoveBlockedSFX;
        EventBroker.PlaceTerrain += PlayPlaceTerrainSFX;
    }
    void OnDisable()
    {
        EventBroker.PlayerMove -= PlayMoveSFX;
        EventBroker.PlayerMoveBlocked -= PlayMoveBlockedSFX;
        EventBroker.PlaceTerrain -= PlayPlaceTerrainSFX;
    }

    void PlayMoveSFX()
    {
        audioSource.PlayOneShot(moveSFX);
    }

    void PlayMoveBlockedSFX()
    {
        audioSource.PlayOneShot(moveBlockedSFX);
    }

    void PlayPlaceTerrainSFX()
    {
        audioSource.PlayOneShot(placeTerrainSFX);
    }
}
