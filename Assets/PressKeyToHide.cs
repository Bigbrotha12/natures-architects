using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PressKeyToHide : MonoBehaviour
{
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
            if (GameManager.Instance != null) GameManager.Instance.ChangeGameState(GameState.RUNNING);
        }
    }
}
