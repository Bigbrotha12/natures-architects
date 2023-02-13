using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class StartMenuController : MonoBehaviour
{
    [SerializeField] Color buttonDefaultTextColor;
    [SerializeField] Color buttonHighlightTextColor;
    [SerializeField] AudioClip buttonHighlightSFX;

    [SerializeField] Button startButton;
    [SerializeField] Button settingsButton;
    [SerializeField] Button quitButton;

    List<Button> buttons = new List<Button>();
    AudioSource audioSource;

    void Awake()
    {
        buttons.Add(startButton);
        buttons.Add(settingsButton);
        buttons.Add(quitButton);

        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        AddOnClickListeners();
    }

    void OnDisable()
    {
        RemoveAllButtonListeners();
    }

    void AddOnClickListeners()
    {
        startButton.onClick.AddListener(StartButtonPressed);
        settingsButton.onClick.AddListener(SettingsButtonPressed);
        quitButton.onClick.AddListener(QuitButtonPressed);
    }

    void RemoveAllButtonListeners()
    {
        startButton.onClick.RemoveAllListeners();
        settingsButton.onClick.RemoveAllListeners();
        quitButton.onClick.RemoveAllListeners();
    }

    public void OnButtonHover(Button button)
    {
        button.GetComponentInChildren<TextMeshProUGUI>().color = buttonHighlightTextColor;
        audioSource.clip = buttonHighlightSFX;
        audioSource.Play();
    }

    public void OnButtonHoverExit(Button button)
    {
        button.GetComponentInChildren<TextMeshProUGUI>().color = buttonDefaultTextColor;
    }

    void StartButtonPressed()
    {
        EventBroker.CallStartGame();
    }

    void SettingsButtonPressed()
    {
        print("TODO: Settings button");
    }

    void QuitButtonPressed()
    {
        EventBroker.CallQuitGame();
    }
}
