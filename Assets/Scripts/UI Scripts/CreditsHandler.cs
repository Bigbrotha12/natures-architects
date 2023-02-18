using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreditsHandler : MonoBehaviour
{
    [SerializeField] GameObject CreditsPanel;
    public void DisplayCredits()
    {
        if(CreditsPanel is not null)
        {
            CreditsPanel.SetActive(true);
        }
    }

    public void CloseCredits()
    {
        if(CreditsPanel is not null)
        {
            CreditsPanel.SetActive(false);
        }
    }
}
