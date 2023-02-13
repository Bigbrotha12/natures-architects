using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    [SerializeField] Sprite defaultBackground;

    [SerializeField] GameObject loadingCanvas;
    [SerializeField] Image backgroundImage;
    [SerializeField] TextMeshProUGUI loadingText;
    [SerializeField] Image progressBarMask;

    void Awake()
    {
        if (defaultBackground != null)
        {
            SetBackgroundImage(defaultBackground);
        }
    }

    public void SetActive(bool active)
    {
        loadingCanvas.SetActive(active);
    }

    public void SetBackgroundImage(Sprite sprite)
    {
        backgroundImage.sprite = sprite;
    }

    public void SetText(string text)
    {
        loadingText.text = text;
    }

    public void SetProgressBar(float progressDecimal)
    {
        if (progressDecimal <= 1 && progressDecimal >= 0)
        {
            progressBarMask.fillAmount = progressDecimal;
        }
    }
}