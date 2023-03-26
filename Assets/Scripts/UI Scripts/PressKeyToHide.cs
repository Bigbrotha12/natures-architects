using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PressKeyToHide : MonoBehaviour
{
    public UnityEvent OnKeyPress;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space) || Input.GetMouseButtonDown(0))
        {
            gameObject.SetActive(false);
            OnKeyPress?.Invoke();
        }
    }

    public void StartGame()
    {
        if (GameManager.Instance != null) GameManager.Instance.ChangeGameState(GameState.RUNNING);
    }

    public void EndGame()
    {
        EventBroker.CallGameCompleted();
    }
}
