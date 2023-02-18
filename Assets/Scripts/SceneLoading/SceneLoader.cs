using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] Fader fader;
    [SerializeField] float fadeTime = 1.5f;
    [SerializeField] float minLoadTime = 1;
    [SerializeField] LoadingScreen loadingScreen;

    List<AsyncOperation> scenesLoading = new List<AsyncOperation>();

    public IEnumerator LoadScene(SceneIndex newScene, SceneIndex oldScene)
    {
        List<SceneIndex> scenesToLoad = new List<SceneIndex>();
        scenesToLoad.Add(newScene);

        List<SceneIndex> scenesToUnload = new List<SceneIndex>();
        if (oldScene != SceneIndex.NONE)
        {
            scenesToUnload.Add(oldScene);
        }

        yield return LoadScenes(scenesToLoad, scenesToUnload);
    }


    public IEnumerator LoadScenes(List<SceneIndex> newScenes, List<SceneIndex> oldScenes)
    {
        yield return fader.FadeOut(fadeTime);
        loadingScreen.SetActive(true);
        yield return fader.FadeIn(fadeTime);

        float startTime = Time.time;

        foreach (SceneIndex oldScene in oldScenes)
        {
            print("Unloading scene:" + oldScene);
            scenesLoading.Add(SceneManager.UnloadSceneAsync((int)oldScene));

        }

        foreach (SceneIndex newScene in newScenes)
        {
            scenesLoading.Add(SceneManager.LoadSceneAsync((int)newScene, LoadSceneMode.Additive));
        }

        float totalLoadProgress;

        foreach (AsyncOperation sceneLoading in scenesLoading)
        {
            while (!sceneLoading.isDone)
            {
                totalLoadProgress = 0;
                foreach (AsyncOperation operation in scenesLoading)
                {
                    totalLoadProgress += operation.progress;
                }

                totalLoadProgress = totalLoadProgress / scenesLoading.Count;
                loadingScreen.SetProgressBar(totalLoadProgress);

                yield return null;
            }
        }

        loadingScreen.SetProgressBar(1);

        float loadTime = Time.time - startTime;
        if (loadTime < minLoadTime)
        {
            yield return new WaitForSeconds(minLoadTime - loadTime);
        }

        yield return fader.FadeOut(fadeTime);
        loadingScreen.SetActive(false);
        yield return fader.FadeIn(fadeTime);
    }
}