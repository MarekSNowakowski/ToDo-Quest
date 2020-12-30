using System;
using System.Globalization;
using UnityEngine;

/// <summary>
/// Class for creating all types of labels
/// </summary>
public class LabelFactory : MonoBehaviour
{
    [SerializeField]
    GameObject labelCategory;
    [SerializeField]
    GameObject labelDate;
    [SerializeField]
    Sprite emptyBookmarkIcon;
    string emptyLabelText = "Others";

    public CategoryLabel LoadCategory(Category category)
    {
        GameObject ob = Instantiate(labelCategory);
        ob.transform.SetParent(transform);
        CategoryLabel categoryLabel = ob.GetComponent<CategoryLabel>();
        categoryLabel.Initialize(category);

        return categoryLabel;
    }

    public CategoryLabel LoadOthersCategoryLabel()
    {
        GameObject ob = Instantiate(labelCategory);
        ob.transform.SetParent(transform);
        CategoryLabel categoryLabel = ob.GetComponent<CategoryLabel>();
        categoryLabel.Initialize(emptyLabelText,emptyBookmarkIcon);

        return categoryLabel;
    }

    /// <summary>
    /// Used to create DateLabel, contains instructions on how to create Labels by date
    /// </summary>
    public DateLabel LoadDate(DateTime date)
    {
        string title = "", dateText = "";
        CultureInfo cultureInfo = new CultureInfo("en-US");

        if (date == default)
        {
            title = "Other";
            dateText = "";
        }
        else if(date < DateTime.Today)
        {
            title = "Overdue";
            dateText = "";
        }
        else
        {
            for(int i = 0; i < 7; i++)
            {
                if (date == DateTime.Today.AddDays(i))
                {
                    if(i==0)
                    {
                        title = "Today ( " + date.ToString("dddd", cultureInfo) + " )";
                    }
                    else if(i==1)
                    {
                        title = "Tommorow ( " + date.ToString("dddd", cultureInfo) + " )";
                    }
                    else
                    {
                        title = date.ToString("dddd", cultureInfo);
                    }

                    dateText = date.ToString("dd MMMM", cultureInfo);
                }
            }
        }

        GameObject ob = Instantiate(labelDate);
        ob.transform.SetParent(transform);
        DateLabel dateLabel = ob.GetComponent<DateLabel>();
        dateLabel.Initialize(dateText,title);

        return dateLabel;
    }
}
