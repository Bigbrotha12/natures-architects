using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

[System.Serializable]
public class AudioGroupSettings
{
    public string audioGroupName;
    public string displayName;
    public Slider volumeSlider;
    public float defaultVolume;
    public float currentVolume;
    AudioMixer audioMixer;

    public void InitialiseVolume(AudioMixer audioMixer)
    {
        this.audioMixer = audioMixer;
        currentVolume = defaultVolume;

        if (PlayerPrefs.HasKey(audioGroupName))
        {
            currentVolume = PlayerPrefs.GetFloat(audioGroupName, currentVolume);
        }

        SetVolume(currentVolume);
    }

    public void SetVolume(float newVolume)
    {
        currentVolume = newVolume;
        PlayerPrefs.SetFloat(audioGroupName, newVolume);
        // Mixer volume uses a logarithmic scale but we want the slider to be linear so we need a conversion
        var dbVolume = Mathf.Log10(newVolume) * 20;
        if (newVolume == 0.0f)
            dbVolume = -80.0f;
        audioMixer.SetFloat(audioGroupName, dbVolume);
    }

    public void SetupSlider(Slider slider)
    {
        volumeSlider = slider;
        volumeSlider.onValueChanged.AddListener(SetVolume);
        volumeSlider.value = currentVolume;
    }

    public void SetToDefaultVolume()
    {
        SetVolume(defaultVolume);
        volumeSlider.value = defaultVolume;
    }

    public void RemoveListeners()
    {
        if(volumeSlider != null)
        {
            volumeSlider.onValueChanged.RemoveAllListeners();
        }
    }
}

public class AudioSettings : MonoBehaviour
{
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] AudioGroupSettings[] audioGroups;

    public AudioGroupSettings[] AudioGroups
    {
        get { return audioGroups; }
    }

    private void Start()
    {
        InitialiseSounds();
    }

    void OnDisable()
    {
        foreach (AudioGroupSettings audioGroup in audioGroups)
        {
            audioGroup.RemoveListeners();
        }
    }

    private void InitialiseSounds()
    {
        foreach (AudioGroupSettings audioGroup in audioGroups)
        {
            audioGroup.InitialiseVolume(audioMixer);
        }
    }

    public void SetToDefaultVolume()
    {
        foreach (AudioGroupSettings audioGroup in audioGroups)
        {
            audioGroup.SetToDefaultVolume();
        }
    }
}
