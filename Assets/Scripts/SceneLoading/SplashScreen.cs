using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class SplashLogo
{
    public Sprite sprite;
    public float fadeInTime = 0.5f;
    public float displayDuration = 1.0f;
    public float fadeOutTime = 0.5f;
}

public class SplashScreen : MonoBehaviour
{
    [SerializeField] SplashLogo[] logos;

    [SerializeField] GameObject splashCanvas;
    [SerializeField] GameObject logoObject;

    Image logoImage;
    CanvasGroup logoCanvasGroup;

    void Awake()
    {
        logoImage = logoObject.GetComponent<Image>();
        logoCanvasGroup = logoObject.GetComponent<CanvasGroup>();
        logoCanvasGroup.alpha = 0;
    }

    public IEnumerator PlaySequence()
    {
        splashCanvas.SetActive(true);
        foreach (SplashLogo logo in logos)
        {
            logoImage.sprite = logo.sprite;
            yield return ShowLogo(logo);
        }
        yield return FindObjectOfType<Fader>().FadeOut(1);
        splashCanvas.SetActive(false);
    }

    IEnumerator ShowLogo(SplashLogo logo)
    {
        yield return FadeInLogo(logo.fadeInTime);
        yield return new WaitForSeconds(logo.displayDuration);
        yield return FadeOutLogo(logo.fadeOutTime);
    }

    IEnumerator FadeInLogo(float duration)
    {
        yield return CanvasFaderUtils.FadeWithLerp(logoCanvasGroup, 0, 1, duration);
    }

    IEnumerator FadeOutLogo(float duration)
    {
        yield return CanvasFaderUtils.FadeWithLerp(logoCanvasGroup, 1, 0, duration);
    }
}

public static class CanvasFaderUtils
{
    public static IEnumerator FadeWithLerp(CanvasGroup canvasToFade, float startValue, float endValue, float duration)
    {
        canvasToFade.alpha = startValue;
        float timeElapsed = 0;
        while (timeElapsed < duration)
        {
            canvasToFade.alpha = Mathf.Lerp(startValue, endValue, timeElapsed / duration);
            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        canvasToFade.alpha = endValue;
    }
}