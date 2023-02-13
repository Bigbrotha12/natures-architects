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
    }

    void Start()
    {
        splashCoroutine = StartCoroutine(ShowSplashScreen());
    }

    IEnumerator ShowSplashScreen()
    {
        yield return splashScreen.PlaySequence();

        StartCoroutine(sceneLoader.LoadScene(SceneIndexes.TITLE_SCENE, SceneIndexes.NONE));
    }

    void OnDisable()
    {
        EventBroker.StartGame -= LoadGame;
    }

    void LoadGame()
    {
        StartCoroutine(sceneLoader.LoadScene(SceneIndexes.GAME_SCENE, SceneIndexes.TITLE_SCENE));
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
