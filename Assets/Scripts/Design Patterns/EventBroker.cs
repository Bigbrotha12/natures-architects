using System;
using UnityEngine;

public class EventBroker
{
    public static event Action StartGame;
    public static void CallStartGame()
    {
        StartGame?.Invoke();
    }

    public static event Action QuitGame;
    public static void CallQuitGame()
    {
        QuitGame?.Invoke();
    }
}