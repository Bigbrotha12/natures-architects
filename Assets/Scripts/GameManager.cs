using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    PREGAME,
    RUNNING,
    PAUSE,
    ENDGAME
}

public class GameManager : Singleton<GameManager>
{
    [SerializeField] SceneLoader sceneLoader;
    [SerializeField] SplashScreen splashScreen;

    bool gameCompleted = false;
    GameState currentState;
    Coroutine splashCoroutine;

    public bool GameCompleted { get { return gameCompleted; } }
    public GameState CurrentState { get { return currentState; } }

    protected override void Awake()
    {
        base.Awake();

        EventBroker.StartGame += LoadGame;
        EventBroker.QuitGame += QuitGame;
        EventBroker.ReturnToTitleScreen += ReturnToStartMenu;
        EventBroker.GameCompleted += OnGameCompleted;
        EventBroker.RestartGame += RestartGame;
        currentState = GameState.PREGAME;
    }

    void Start()
    {
        splashCoroutine = StartCoroutine(ShowSplashScreen());
    }

    IEnumerator ShowSplashScreen()
    {
        yield return splashScreen.PlaySequence();

        StartCoroutine(sceneLoader.LoadScene(SceneIndex.TITLE_SCENE, SceneIndex.NONE));
    }

    void OnDisable()
    {
        EventBroker.StartGame -= LoadGame;
        EventBroker.QuitGame -= QuitGame;
        EventBroker.ReturnToTitleScreen -= ReturnToStartMenu;
        EventBroker.GameCompleted -= OnGameCompleted;
        EventBroker.RestartGame -= RestartGame;
    }

    void LoadGame()
    {
        StartCoroutine(LoadGameRoutine(SceneIndex.TITLE_SCENE));
    }

    IEnumerator LoadGameRoutine(SceneIndex previousScene)
    {
        yield return StartCoroutine(sceneLoader.LoadScene(SceneIndex.GAME_SCENE, previousScene));
        currentState = GameState.RUNNING;
    }

    void RestartGame()
    {
        StartCoroutine(LoadGameRoutine(SceneIndex.END_SCENE));
    }

    void ReturnToStartMenu()
    {
        StartCoroutine(sceneLoader.LoadScene(SceneIndex.TITLE_SCENE, SceneIndex.GAME_SCENE));
        currentState = GameState.PREGAME;
    }

    void OnGameCompleted()
    {
        gameCompleted = true;
        StartCoroutine(sceneLoader.LoadScene(SceneIndex.TITLE_SCENE, SceneIndex.GAME_SCENE));
        currentState = GameState.ENDGAME;
    }

    void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}
