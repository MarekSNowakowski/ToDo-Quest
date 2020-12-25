using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Cell or slot in the calendar. All the information each day should now about itself
/// </summary>
public class Day : MonoBehaviour
{
    public DateTime date;
    public Color dayColor;
    public Color nonActiveColor;
    public bool active;
    public Calendar calendar;

    /// <summary>
    /// Constructor of Day
    /// </summary>
    public void InitializeDay(DateTime date, Color dayColor, bool active, Calendar calendar)
    {
        this.date = date;
        UpdateActivnes(active, dayColor);
        UpdateDay(date.Day);
        this.calendar = calendar;
    }

    /// <summary>
    /// Call this when updating the color so that both the dayColor is updated, as well as the visual color on the screen
    /// </summary>
    public void UpdateActivnes(bool active, Color newColor)
    {
        this.active = active;
        gameObject.GetComponent<Image>().color = newColor;
        dayColor = newColor;
    }

    /// <summary>
    /// When updating the day we decide whether we should show the dayNum based on the color of the day
    /// This means the color should always be updated before the day is updated
    /// </summary>
    public void UpdateDay(int newDayNum)
    {
        int dayNum = newDayNum;
        if (active)
        {
            gameObject.GetComponentInChildren<TextMeshProUGUI>().text = (dayNum).ToString();
        }
        else
        {
            gameObject.GetComponentInChildren<TextMeshProUGUI>().text = "";
        }
    }

    public void OnDayChoose()
    {
        calendar.ChooseDate();
        gameObject.GetComponent<Image>().color = calendar.GetSelectedColor();
    }
}