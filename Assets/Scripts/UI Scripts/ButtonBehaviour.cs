using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBehaviour : MonoBehaviour
{
    [SerializeField] Color normalColor;
    [SerializeField] Color highlightedColor;
    [SerializeField] Color normalTextColor;
    [SerializeField] Color highlightedTextColor;
    [SerializeField] AudioClip highlightedSound;

    AudioSource audioSource;

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    void OnEnable()
    {
        SetNormal();
    }

    public void HighlightMe()
    {
        GetComponent<Image>().color = highlightedColor;
        GetComponentInChildren<TextMeshProUGUI>().color = highlightedTextColor;
        if (highlightedSound != null)
        {
            audioSource.clip = highlightedSound;
            audioSource.Play();
        }
    }

    public void SetNormal()
    {
        GetComponent<Image>().color = normalColor;
        GetComponentInChildren<TextMeshProUGUI>().color = normalTextColor;
    }
}
