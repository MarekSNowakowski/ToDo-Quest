﻿using System.Collections.Generic;
using UnityEngine;
using System;
using TMPro;
using System.Globalization;

public class Calendar : MonoBehaviour
{
    /// <summary>
    /// All the days in the month. After we make our first calendar we store these days in this list so we do not have to recreate them every time.
    /// </summary>
    private List<Day> days = new List<Day>();

    /// <summary>
    /// Setup in editor since there will always be six weeks. 
    /// Try to figure out why it must be six weeks even though at most there are only 31 days in a month
    /// </summary>
    public Transform[] weeks;

    /// <summary>
    /// This is the text object that displays the current month and year
    /// </summary>
    public TextMeshProUGUI MonthAndYear;

    /// <summary>
    /// this currDate is the date our Calendar is currently on. The year and month are based on the calendar, 
    /// while the day itself is almost always just 1
    /// If you have some option to select a day in the calendar, you would want the change this objects day value to the last selected day
    /// </summary>
    public DateTime currDate = DateTime.Now;

    /// <summary>
    /// Here set colors representing state of the day
    /// </summary>
    public Color activeDayColor;
    public Color nonActiveDayColor;
    public Color currentDayColor;
    public Color selectedColor;

    /// <summary>
    /// In awake we set the Calendar to the current date
    /// </summary>
    private void OnEnable()
    {
        UpdateCalendar(DateTime.Now.Year, DateTime.Now.Month);
    }

    /// <summary>
    /// Anytime the Calendar is changed we call this to make sure we have the right days for the right month/year
    /// </summary>
    void UpdateCalendar(int year, int month)
    {
        DateTime temp = new DateTime(year, month, 1);
        currDate = temp;
        CultureInfo cultureInfo = new CultureInfo("en-US");
        MonthAndYear.text = temp.Year.ToString() + "\n" + temp.ToString("MMMM", cultureInfo);
        int startDay = GetMonthStartDay(year,month) - 1;
        int endDay = GetTotalNumberOfDays(year, month);
        //Activate last week because it can be inactive
        weeks[weeks.Length - 1].gameObject.SetActive(true);


        ///Create the days
        ///This only happens for our first Update Calendar when we have no Day objects therefore we must create them

        if (days.Count == 0)
        {
            for (int w = 0; w < 6; w++)
            {
                for (int i = 0; i < 7; i++)
                {
                    int currDay = (w * 7) + i;

                    Day newDay = weeks[w].GetChild(i).gameObject.AddComponent<Day>();
                    
                    if (currDay < startDay || currDay - startDay >= endDay)
                    {
                        newDay.InitializeDay(DateTime.Now, nonActiveDayColor, false, this);
                    }
                    else
                    {
                        Debug.Log(currDay - startDay);
                        DateTime date = new DateTime(year, month, currDay - startDay + 1);
                        newDay.InitializeDay(date, activeDayColor, true, this);
                    }
                    days.Add(newDay);
                }
            }
        }
        ///loop through days
        ///Since we already have the days objects, we can just update them rather than creating new ones
        else
        {
            for(int i = 0; i < 42; i++)
            {
                if(i < startDay || i - startDay >= endDay)
                {
                    days[i].UpdateActivnes(false, nonActiveDayColor);
                }
                else
                {
                    days[i].UpdateActivnes(true, activeDayColor);
                }

                days[i].UpdateDay(i - startDay + 1);
            }
        }

        ///This just checks if today is on our calendar. If so, we highlight it in green
        if(DateTime.Now.Year == year && DateTime.Now.Month == month)
        {
            days[(DateTime.Now.Day - 1) + startDay].UpdateActivnes(true, currentDayColor);
        }

        CheckTheLastWeek();
    }

    /// <summary>
    /// Turns off last row if the month can be displayed in 5 rows
    /// </summary>
    void CheckTheLastWeek()
    {
        bool turnOff = true;
        for(int i = days.Count - 7 ; i < days.Count ; i++)
        {
            if (days[i].active)
            {
                turnOff = false;
                break;
            }
        }
        if(turnOff)
        {
            weeks[weeks.Length - 1].gameObject.SetActive(false);
        }
    }

    /// <summary>
    /// This returns which day of the week the month is starting on
    /// </summary>
    int GetMonthStartDay(int year, int month)
    {
        DateTime temp = new DateTime(year, month, 1);

        //DayOfWeek Sunday == 0, Saturday == 6 etc.
        return (int)temp.DayOfWeek;
    }

    /// <summary>
    /// Gets the number of days in the given month.
    /// </summary>
    int GetTotalNumberOfDays(int year, int month)
    {
        return DateTime.DaysInMonth(year, month);
    }

    /// <summary>
    /// This either adds or subtracts one month from our currDate.
    /// The arrows will use this function to switch to past or future months
    /// </summary>
    public void SwitchMonth(int direction)
    {
        if(direction < 0)
        {
            currDate = currDate.AddMonths(-1);
        }
        else
        {
            currDate = currDate.AddMonths(1);
        }

        UpdateCalendar(currDate.Year, currDate.Month);
    }

    public Color GetSelectedColor()
    {
        return selectedColor;
    }

    public void ChooseDate()
    {

    }

    public void SubmitDate()
    {

    }
}
