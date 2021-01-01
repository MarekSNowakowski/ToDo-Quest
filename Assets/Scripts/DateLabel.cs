using System;
using System.Globalization;
using UnityEngine;

public class DateLabel : Label
{
    [SerializeField]
    TMPro.TextMeshProUGUI titleText;
    [SerializeField]
    TMPro.TextMeshProUGUI dateText;
    [SerializeField]
    Color inactiveColor;


    public void Initialize(string date, string title, string iD)
    {
        titleText.text = title;
        dateText.text = date;
        questsInside = 0;
        labelID = iD;
    }

    public void TurnInactive()
    {
        titleText.color = inactiveColor;
        dateText.color = inactiveColor;
    }

    public string GetName()
    {
        return dateText.text;
    }
}

