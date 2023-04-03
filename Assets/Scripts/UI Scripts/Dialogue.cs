using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Dialogue : MonoBehaviour
{
    public TextMeshProUGUI textComponentTMPRO;
    public Text textComponent;
    public string[] lines;
    public float delayBetweenLetters;
    public float delayBetweenLines;
    public bool autoNextLine = true;

    public int index;

    void OnEnable()
    {
        ToggleTextBox(true);
        WriteText(string.Empty);
        StartDialogue();
    }

    void OnDisable()
    {
        WriteText(string.Empty);
        ToggleTextBox(false);
    }

    void StartDialogue()
    {
        index = 0;
        StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        string lineText = "";
        foreach (char c in lines[index].ToCharArray())
        {
            lineText += c;
            WriteText(lineText);
            yield return new WaitForSecondsRealtime(delayBetweenLetters);
        }
        if (autoNextLine)
        {
            yield return new WaitForSecondsRealtime(delayBetweenLines);
            NextLine();
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            WriteText(string.Empty);
            StartCoroutine(TypeLine());
        }
        else

        {
            index = 0;
            WriteText(string.Empty);
        }
    }

    private void ToggleTextBox(bool newState)
    {
        if (textComponentTMPRO != null)
        {
            textComponentTMPRO.gameObject.SetActive(newState);
        }
        else if (textComponent != null)
        {
            textComponent.gameObject.SetActive(newState);
        }
    }

    private void WriteText(string str)
    {
        if (textComponentTMPRO != null)
        {
            textComponentTMPRO.text = str;
        }
        else if (textComponent != null)
        {
            textComponent.text = str;
        }
    }
}


