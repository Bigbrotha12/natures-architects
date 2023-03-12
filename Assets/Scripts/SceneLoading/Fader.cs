using System.Collections;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Fader : MonoBehaviour
{
    CanvasGroup canvasGroup;
    Coroutine currentActiveFade;

    public bool FaderIsActive { get { return canvasGroup.alpha != 0; } }

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void FadeOutInstant()
    {
        canvasGroup.alpha = 1;
    }

    public void FadeInInstant()
    {
        canvasGroup.alpha = 0;
    }

    public Coroutine FadeOut(float time)
    {
        return Fade(1, time);
    }

    public Coroutine FadeIn(float time)
    {
        return Fade(0, time);
    }

    public Coroutine Fade(float alphaTarget, float time)
    {
        if (currentActiveFade != null)
        {
            StopCoroutine(currentActiveFade);
        }
        return currentActiveFade = StartCoroutine(FadeRoutine(alphaTarget, time));
    }

    IEnumerator FadeRoutine(float targetAlpha, float time)
    {
        while (!Mathf.Approximately(canvasGroup.alpha, targetAlpha))
        {
            canvasGroup.alpha = Mathf.MoveTowards(canvasGroup.alpha, targetAlpha, Time.deltaTime / time);
            yield return new WaitForEndOfFrame();
        }
    }
}