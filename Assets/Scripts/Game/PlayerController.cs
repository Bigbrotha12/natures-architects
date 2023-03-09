using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using System;

public class PlayerController : MonoBehaviour
{
    public Transform player;
    public bool canMove = true;
    public float inputDelay = 0.5f;
    public int actionCounter = 12;
    public TMP_Text actionCounterText;

    [SerializeField] MapGrid mapGrid;

    [Header("For testing")]
    [SerializeField] bool placingTileUsesAction = true;
    
    Character character;
    Mover mover;

    bool isDead;

    void Awake()
    {
        if (player == null)
        {
            player = transform;
        }
        mover = GetComponent<Mover>();
        character = GetComponentInChildren<Character>();
    }

    void OnDisable()
    {
        mover.movementCompletedEvent -= OnMovementCompleted;
    }


    void Update()
    {
        if (actionCounter <= 0) return;

        if (!canMove) return;
        canMove = false;

        if (Input.GetAxis("Fire1") > 0.5f)
        {
            PlaceTile();
            return;
        }

        float vertical = Input.GetAxis("Vertical");
        float horizontal = Input.GetAxis("Horizontal");
        
        if (vertical > 0.9f)
        {
            Move(Vector3.up);
            return;
        }

        if (vertical < -0.9f)
        {
            Move(Vector3.down);
            return;
        }

        if (horizontal < -0.9f)
        {
            Move(Vector3.left);
            return;
        }

        if (horizontal > 0.9f)
        {
            Move(Vector3.right);
            return;
        }

        canMove = true;
    }

    public void SetCharacter(CharacterSO newCharacter, int moves)
    {
        character.ChangeCharacter(newCharacter);
        SetActionCounter(moves);
        canMove = true;
    }

    void SetActionCounter(int moves)
    {
        actionCounter = moves;
        actionCounterText.text = actionCounter.ToString();
    }

    void PlaceTile()
    {
        if (player is null || mapGrid is null || character.TerrainTile is null)
        {
            Debug.Log("Character, tilemap, or tile reference not set.");
            return;
        }

        Vector3Int position = new Vector3Int((int)player.position.x, (int)player.position.y, 0);
        // Only allow tile placement over "Empty" or "Fire" tiles, except for Fire which can place on top of any except other Fire.
        TerrainTypes targetTerrain = mapGrid.CheckTile(position);
        if (character.TerrainTile.tileType != TerrainTypes.FIRE && (targetTerrain == TerrainTypes.NONE || targetTerrain == TerrainTypes.FIRE))
        {
            mapGrid.CreateTile(position.x, position.y, character.TerrainTile);
            mapGrid.ScoreTile(position);
            EventBroker.CallPlaceTerrain();

            if (placingTileUsesAction)
            {
                IncrementActionCount();
            }
        } 
        else if(character.TerrainTile.tileType == TerrainTypes.FIRE && targetTerrain != TerrainTypes.FIRE)
        {
            mapGrid.CreateTile(position.x, position.y, character.TerrainTile);
            mapGrid.ScoreTile(position);
            EventBroker.CallPlaceTerrain();

            if (placingTileUsesAction)
            {
                IncrementActionCount();
            }
        }
        else
        {
            EventBroker.CallPlayerMoveBlocked();
           
        }
        StartCoroutine("InputCooldown");
    }

    void Move(Vector3 direction)
    {
        if (player is null)
        {
            Debug.Log("Character transform not set.");
            return;
        }

        Vector3Int newPosition = new Vector3Int((int)player.position.x + (int)direction.x, (int)player.position.y + (int)direction.y, (int)player.position.z);

        if (mapGrid.CheckPositionIsOnMapGrid(newPosition))
        {
            mover.movementCompletedEvent += OnMovementCompleted;
            mover.MoveToLocation(player, newPosition);
            EventBroker.CallPlayerMove();
            IncrementActionCount();
        }
        else
        {
            EventBroker.CallPlayerMoveBlocked();
            StartCoroutine(InputCooldown());
        }
    }

    void OnMovementCompleted()
    {
        if (isDead)
        {
            CharacterDeath();
        }
        StartCoroutine(InputCooldown());
        mover.movementCompletedEvent -= OnMovementCompleted;
    }

    void IncrementActionCount()
    {
        actionCounter -= 1;
        if (actionCounter > 0)
        {
            actionCounterText.text = actionCounter.ToString();
        }
        else
        {
            actionCounterText.text = "Dead";
            isDead = true;
        }
    }

    void CharacterDeath()
    {
        isDead = false;
        EventBroker.CallCharacterDeath();
    }

    IEnumerator InputCooldown() 
    {
        yield return new WaitForSeconds(inputDelay);
        canMove = true;
    }
}