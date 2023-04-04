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
    bool paused = false;

    bool isDead;
    bool canBuild = true;

    Coroutine cooldownRoutine;

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
        if (!GameStateCheck()) return;

        if (paused) return;
        if (actionCounter <= 0) return;

        if (!canBuild) return;

        if (Input.GetAxis("Fire1") > 0.5f)
        {
            canBuild = false;
            canMove = false;
            PlaceTileAttempt();
            return;
        }

        if (!canMove) return;
        canMove = false;

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

    public void PauseControls(bool value)
    {
        paused = value;
    }

    public void ShowCharacter(bool show)
    {
        character.ShowCharacter(show);
    }

    public void SetCharacter(CharacterSO newCharacter, int moves)
    {
        character.ChangeCharacter(newCharacter);
        SetActionCounter(moves);
        StartCoroutine(CheckCharacterSetupComplete());
    }

    bool GameStateCheck()
    {
        if (GameManager.Instance == null) return true;
        return GameManager.Instance.CurrentState == GameState.RUNNING;
    }

    IEnumerator CheckCharacterSetupComplete()
    {
        while(!character.IsReady)
        {
            yield return new WaitForEndOfFrame();
        }
        OnCharacterSetupComplete();
    }

    void OnCharacterSetupComplete()
    {
        canMove = true;
        paused = false;
    }

    void SetActionCounter(int moves)
    {
        actionCounter = moves;
        actionCounterText.text = actionCounter.ToString();
    }

    void PlaceTileAttempt()
    {
        if (player is null )
        {
            Debug.Log("player reference not set.");
            StartInputCooldown();
            return;
        }

        if (mapGrid is null)
        {
            Debug.Log("Map grid reference not set.");
            StartInputCooldown();
            return;
        }

        if (character is null)
        {
            Debug.Log("Character reference not set.");
            StartInputCooldown();
            return;
        }

        if (character.TerrainTile is null)
        {
            Debug.Log("Terrain tile reference not set.");
            StartInputCooldown();
            return;
        }

        Vector3Int position = new Vector3Int((int)player.position.x, (int)player.position.y, 0);
        TerrainTypes targetTerrain = mapGrid.GetTileType(position);

        if (CanPlaceTile(targetTerrain))
        {
            PlaceTile(position);
        }
        else
        {
            EventBroker.CallPlayerMoveBlocked();
           
        }
        StartInputCooldown();
    }

    bool CanPlaceTile(TerrainTypes targetTerrain)
    {
        // Only allow tile placement over "Empty" or "Fire" tiles, except for Fire which can place on top of any except other Fire.
        if (character.TerrainTile.tileType != TerrainTypes.Fire && (targetTerrain == TerrainTypes.None || targetTerrain == TerrainTypes.Fire))
            return true;
        if (character.TerrainTile.tileType == TerrainTypes.Fire && targetTerrain != TerrainTypes.Fire)
            return true;

        return false;
    }

    void PlaceTile(Vector3Int position)
    {
        mapGrid.CreateTile(position.x, position.y, character.TerrainTile);
        mapGrid.ScoreTile(position);
        EventBroker.CallPlaceTerrain();

        if (placingTileUsesAction)
        {
            IncrementActionCount();
        }
    }

    void Move(Vector3 direction)
    {
        if (player is null)
        {
            Debug.Log("Character transform not set.");
            return;
        }

        Vector3Int newPosition = new Vector3Int((int)player.position.x + (int)direction.x, (int)player.position.y + (int)direction.y, (int)player.position.z);

        if (CanMoveToTile(newPosition))
        {
            mover.movementCompletedEvent += OnMovementCompleted;
            mover.MoveToLocation(player, newPosition);
            EventBroker.CallPlayerMove();
        }
        else
        {
            EventBroker.CallPlayerMoveBlocked();
            StartInputCooldown();
        }
    }

    bool CanMoveToTile(Vector3Int newPosition)
    {
        if (!mapGrid.CheckPositionIsOnMapGrid(newPosition)) return false;
        return mapGrid.CheckTileIsWalkable(newPosition);
    }

    void OnMovementCompleted()
    {
        mover.movementCompletedEvent -= OnMovementCompleted;
        IncrementActionCount();

        if (isDead)
        {
            CharacterDeath();
            return;
        }
        StartInputCooldown();
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
            actionCounterText.text = "0";
            isDead = true;
        }
    }

    void CharacterDeath()
    {
        isDead = false;
        EventBroker.CallCharacterDeath();
    }

    void StartInputCooldown()
    {
        if (cooldownRoutine is not null) StopCoroutine(cooldownRoutine);
        canMove = false;
        canBuild = false;
        cooldownRoutine = StartCoroutine(InputCooldownRoutine(inputDelay));
    }

    IEnumerator InputCooldownRoutine(float duration) 
    {
        yield return new WaitForSeconds(duration);
        canMove = true;
        canBuild = true;
    }
}