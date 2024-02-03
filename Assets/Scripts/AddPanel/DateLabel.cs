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
    DateTime date;


    public void Initialize(DateTime date, string dateText, string title, string iD)
    {
        this.date = date;
        this.dateText.text = dateText;
        titleText.text = title;
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

    public DateTime GetDate()
    {
        return date;
    }
}

