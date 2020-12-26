using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Cell or slot in the calendar. All the information each day should now about itself
/// </summary>
public class Day : MonoBehaviour
{
    DateTime date;
    Color dayColor;
    Color nonActiveColor;
    bool active;
    Calendar calendar;
    Button button;

    private void AddButton()
    {
        button = this.gameObject.AddComponent<Button>();
        button.onClick.AddListener(OnDayChoose);
    }

    void TurnOffIfNotActive()
    {
        if(button==null)
        {
            AddButton();
        }
        if(active)
        {
            button.enabled = true;
        }
        else
        {
            button.enabled = false;
        }
    }

    /// <summary>
    /// Constructor of Day
    /// </summary>
    public void InitializeDay(DateTime date, Color dayColor, bool active, Calendar calendar)
    {
        this.date = date;
        UpdateActivnes(active, dayColor);
        UpdateDay(date.Day);
        this.calendar = calendar;
        TurnOffIfNotActive();
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
        Image image = gameObject.GetComponent<Image>();
        if (image.color != calendar.GetSelectedColor())
        {
            Select(image);
        }
        else
        {
            UnSelect(image);
        }

    }

    void Select(Image image)
    {
        calendar.ChooseDate(this);
        image.color = calendar.GetSelectedColor();
    }

    void UnSelect(Image image)
    {
        calendar.UnChooseDate();
        image.color = dayColor;
    }

    public void UnSelect()
    {
        Image image = gameObject.GetComponent<Image>();
        UnSelect(image);
    }

    public bool IsActive()
    {
        return active;
    }

    public DateTime GetDate()
    {
        return date;
    }

    public void ChangeColorToSelected()
    {
        gameObject.GetComponent<Image>().color = calendar.GetSelectedColor();
    }
}