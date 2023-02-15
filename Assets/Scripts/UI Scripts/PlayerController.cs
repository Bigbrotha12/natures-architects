using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using TMPro;

public class PlayerController : MonoBehaviour
{
    public Transform playerCharacter;
    public bool canMove = true;
    public float inputDelay = 0.2f;
    public Tile tile;
    public Tilemap map;
    public int actionCounter = 12;
    public TMP_Text actionCounterText;
    // Start is called before the first frame update
    void Start()
    {
        playerCharacter = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) && canMove)
        {
            canMove = false;
            PlaceTile();
            StartCoroutine("InputCooldown");
        }

        if(Input.GetKeyDown(KeyCode.UpArrow) && canMove)
        {
            canMove = false;
            MoveUp();
            StartCoroutine("InputCooldown");
        }

        if(Input.GetKeyDown(KeyCode.DownArrow) && canMove)
        {
            canMove = false;
            MoveDown();
            StartCoroutine("InputCooldown");
        }

        if(Input.GetKeyDown(KeyCode.LeftArrow) && canMove)
        {
            canMove = false;
            MoveLeft();
            StartCoroutine("InputCooldown");
        }

        if(Input.GetKeyDown(KeyCode.RightArrow) && canMove)
        {
            canMove = false;
            MoveRight();
            StartCoroutine("InputCooldown");
        }
    }

    void MoveUp() 
    {
        if(playerCharacter is null) 
        {
            Debug.Log("Character transform not set.");
            return;
        }
        playerCharacter.position = new Vector3Int((int)playerCharacter.position.x, (int)playerCharacter.position.y + 1, 0);
    }

    void MoveDown() 
    {
        if(playerCharacter is null) 
        {
            Debug.Log("Character transform not set.");
            return;
        }
        playerCharacter.position = new Vector3Int((int)playerCharacter.position.x, (int)playerCharacter.position.y - 1, 0);
    }

    void MoveLeft() 
    {
        if(playerCharacter is null) 
        {
            Debug.Log("Character transform not set.");
            return;
        }
        playerCharacter.position = new Vector3Int((int)playerCharacter.position.x - 1, (int)playerCharacter.position.y, 0);
    }

    void MoveRight() 
    {
        if(playerCharacter is null) 
        {
            Debug.Log("Character transform not set.");
            return;
        }
        playerCharacter.position = new Vector3Int((int)playerCharacter.position.x + 1, (int)playerCharacter.position.y, 0);
    }

    void PlaceTile()
    {
        if(playerCharacter is null || map is null || tile is null) 
        {
            Debug.Log("Character, tilemap, or tile reference not set.");
            return;
        }
        Vector3Int position = new Vector3Int((int)playerCharacter.position.x, (int)playerCharacter.position.y, 0);
        map.SetTile(position, tile);
    }

    IEnumerator InputCooldown() 
    {
        if (actionCounter > 0)
        {
            actionCounter -= 1;
            actionCounterText.text = actionCounter.ToString();
        } else {
            actionCounterText.text = "Dead";
        }
        yield return new WaitForSeconds(inputDelay);
        canMove = true;
    }
}
