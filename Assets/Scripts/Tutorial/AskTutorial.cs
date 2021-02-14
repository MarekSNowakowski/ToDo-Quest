using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AskTutorial : MonoBehaviour
{
    [SerializeField]
    GameObject tutorialAskingPanel;
    [SerializeField]
    SettingsManager settingsManager;

    private void Start()
    {
        if(settingsManager.IsFirstRun())
        {
            settingsManager.SetFirstRun();
            tutorialAskingPanel.SetActive(true);
        }
    }
}
