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

    public static event Action PlayerMove;
    public static void CallPlayerMove()
    {
        PlayerMove?.Invoke();
    }

    public static event Action PlayerMoveBlocked;
    public static void CallPlayerMoveBlocked()
    {
        PlayerMoveBlocked?.Invoke();
    }

    public static event Action PlaceTerrain;
    public static void CallPlaceTerrain()
    {
        PlaceTerrain?.Invoke();
    }
}