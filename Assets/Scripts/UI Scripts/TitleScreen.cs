using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TitleScreen : MonoBehaviour
{
    [SerializeField] Sprite gameCompletedBackgroundImage;
    [SerializeField] string startAgainButtonText;

    [SerializeField] Image backgroundImage;
    [SerializeField] GameObject originalTitleText;
    [SerializeField] GameObject startAgainTitleText;
    [SerializeField] Button startButton;

    void OnEnable()
    {
        if (GameManager.Instance is not null && GameManager.Instance.GameCompleted)
        {
            SetupNewBeginning();
        }
    }

    private void SetupNewBeginning()
    {
        backgroundImage.sprite = gameCompletedBackgroundImage;
        originalTitleText.SetActive(false);
        startAgainTitleText.SetActive(true);
        startButton.GetComponentInChildren<TextMeshProUGUI>().text = startAgainButtonText;
    }
}
