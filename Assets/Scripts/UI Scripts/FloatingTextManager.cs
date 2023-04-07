using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Floating text manager taken from an rpg tutorial

public class FloatingTextManager : MonoBehaviour
{
    public GameObject textContainer;
    public GameObject textPrefab;
    [SerializeField] Transform playerTransform;
    [SerializeField] int fontSize = 60;
    [SerializeField] Color positiveColor;
    [SerializeField] Color negativeColor;
    [SerializeField] float duration;
    [SerializeField] float speed;

    private List<FloatingText> floatingTexts = new List<FloatingText>();

    private void Update()
    {
        foreach (FloatingText txt in floatingTexts)
            txt.UpdateFloatingText();
    }

    public void Show(string msg, bool positive)
    {
        Show(msg, fontSize, positive ? positiveColor : negativeColor, playerTransform.position + Vector3.up, duration, Vector3.up * speed);
    }

    public void Show(string msg, int fontSize, Color color, Vector3 position, float duration, Vector3 motion)
    {
        FloatingText floatingText = GetFloatingText();

        floatingText.txt.text = msg;
        floatingText.txt.fontSize = fontSize;
        floatingText.txt.color = color;

        floatingText.go.transform.position = position; // Camera.main.WorldToScreenPoint(position); // Transfer world space to screen space to use in ui
        floatingText.motion = motion;
        floatingText.duration = duration;

        floatingText.Show();
    }

    private FloatingText GetFloatingText()
    {
        FloatingText txt = floatingTexts.Find(t => !t.active);

        if (txt == null)
        {
            txt = new FloatingText();
            txt.go = Instantiate(textPrefab);
            txt.go.transform.SetParent(textContainer.transform);
            txt.txt = txt.go.GetComponent<TextMeshProUGUI>();

            floatingTexts.Add(txt);
        }

        return txt;
    }
}