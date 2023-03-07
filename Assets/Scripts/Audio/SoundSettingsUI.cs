using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;

public class SoundSettingsUI : MonoBehaviour
{
    [SerializeField] GameObject volumePanelPrefab;
    [SerializeField] Transform volumePanelsParentTransform;

    AudioSettings audioSettings;
    List<GameObject> volumePanels;

    bool initialised;

    void Awake()
    {
        audioSettings = FindObjectOfType<AudioSettings>();
        volumePanels = new List<GameObject>();
    }

    void OnEnable()
    {
        if (initialised) return;
        if (audioSettings == null) return;
        SetupVolumeControls();
    }

    void SetupVolumeControls()
    {
        foreach (AudioGroupSettings audioGroup in audioSettings.AudioGroups)
        {
            GameObject volumePanel = Instantiate(volumePanelPrefab, volumePanelsParentTransform);
            volumePanels.Add(volumePanel);
            volumePanel.GetComponentInChildren<TextMeshProUGUI>().text = audioGroup.displayName;
            Slider slider = volumePanel.GetComponentInChildren<Slider>();
            audioGroup.SetupSlider(slider);
        }
        initialised = true;
    }

    public void SetToDefaultVolume()
    {
        foreach (AudioGroupSettings audioGroup in audioSettings.AudioGroups)
        {
            audioGroup.SetToDefaultVolume();
            audioGroup.volumeSlider.value = audioGroup.defaultVolume;
        }
    }

    public void ExitButtonPressed()
    {
        EventBroker.CallExitSoundMenu();
    }
}
