using System;
using System.Globalization;
using UnityEngine;

public class DateLabel : Label
{
    [SerializeField]
    TMPro.TextMeshProUGUI titleText;
    [SerializeField]
    TMPro.TextMeshProUGUI dateText;


    public void Initialize(string date, string title)
    {
        titleText.text = title;
        dateText.text = date;
        questsInside = 0;
        labelID = title;
    }

    public string GetName()
    {
        return dateText.text;
    }
}

