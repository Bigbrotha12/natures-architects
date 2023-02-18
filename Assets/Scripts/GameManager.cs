using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] SceneLoader sceneLoader;
    [SerializeField] SplashScreen splashScreen;

    Coroutine splashCoroutine;

    protected override void Awake()
    {
        base.Awake();

        EventBroker.StartGame += LoadGame;
        EventBroker.QuitGame += QuitGame;
        EventBroker.ReturnToTitleScreen += ReturnToStartMenu;
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
    }

    void LoadGame()
    {
        StartCoroutine(sceneLoader.LoadScene(SceneIndex.GAME_SCENE, SceneIndex.TITLE_SCENE));
    }

    void ReturnToStartMenu()
    {
        StartCoroutine(sceneLoader.LoadScene(SceneIndex.TITLE_SCENE, SceneIndex.GAME_SCENE));
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
