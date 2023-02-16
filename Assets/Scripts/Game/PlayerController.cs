using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;
using System;

public class PlayerController : MonoBehaviour
{
    public Transform playerCharacter;
    public bool canMove = true;
    public float inputDelay = 0.2f;
    public Tile tile;
    public Tilemap map;
    public int actionCounter = 12;
    public TMP_Text actionCounterText;

    [SerializeField] MapGrid mapGrid;

    Mover mover;

    void Awake()
    {
        playerCharacter = transform;
        mover = GetComponent<Mover>();
    }

    void OnDisable()
    {
        mover.movementCompletedEvent -= OnMovementCompleted;
    }


    void Update()
    {
        if (!canMove) return;
        canMove = false;

        if(Input.GetKeyDown(KeyCode.Space))
        {
            PlaceTile();
            return;
        }

        if(Input.GetKeyDown(KeyCode.UpArrow))
        {
            MoveUp();
            return;
        }

        if(Input.GetKeyDown(KeyCode.DownArrow))
        {
            MoveDown();
            return;
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow))
        {
            MoveLeft();
            return;
        }

        if(Input.GetKeyDown(KeyCode.RightArrow))
        {
            MoveRight();
            return;
        }

        canMove = true;
    }

    void PlaceTile()
    {
        if (playerCharacter is null || map is null || tile is null)
        {
            Debug.Log("Character, tilemap, or tile reference not set.");
            return;
        }
        Vector3Int position = new Vector3Int((int)playerCharacter.position.x, (int)playerCharacter.position.y, 0);
        map.SetTile(position, tile);
        EventBroker.CallPlaceTerrain();

        StartCoroutine("InputCooldown");
    }

    void MoveUp() 
    {
        Move(Vector3.up);
    }

    void MoveDown() 
    {
        Move(Vector3.down);
    }

    void MoveLeft() 
    {
        Move(Vector3.left);
    }

    void MoveRight() 
    {
        Move(Vector3.right);
    }

    void Move(Vector3 direction)
    {
        if (playerCharacter is null)
        {
            Debug.Log("Character transform not set.");
            return;
        }

        Vector3Int newPosition = new Vector3Int((int)playerCharacter.position.x + (int)direction.x, (int)playerCharacter.position.y + (int)direction.y, (int)playerCharacter.position.z);

        if (mapGrid.CheckPositionIsOnMapGrid(newPosition))
        {
            mover.movementCompletedEvent += OnMovementCompleted;
            mover.MoveTo(playerCharacter, newPosition);
            EventBroker.CallPlayerMove();
        }
        else
        {
            EventBroker.CallPlayerMoveBlocked();
        }
    }

    void OnMovementCompleted()
    {
        canMove = true;
        mover.movementCompletedEvent -= OnMovementCompleted;
    }

    IEnumerator InputCooldown() 
    {
        if (actionCounter > 0)
        {
            actionCounter -= 1;
            actionCounterText.text = actionCounter.ToString();
            yield return new WaitForSeconds(inputDelay);
            canMove = true;
        } 
        else 
        {
            actionCounterText.text = "Dead";
        }
    }
}