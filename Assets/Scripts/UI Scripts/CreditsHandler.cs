using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsHandler : MonoBehaviour
{
    [SerializeField] GameObject CreditsPanel;
    [SerializeField] GameObject menuPanel;
    [SerializeField] GameObject title;
    public void DisplayCredits()
    {
        if(CreditsPanel is not null)
        {
            CreditsPanel.SetActive(true);
            menuPanel.SetActive(false);
            title.SetActive(false);
        }
    }

    public void CloseCredits()
    {
        if(CreditsPanel is not null)
        {
            CreditsPanel.SetActive(false);
            menuPanel.SetActive(true);
            title.SetActive(true);
        }
    }
}
