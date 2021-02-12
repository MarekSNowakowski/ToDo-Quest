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
    [SerializeField]
    TranslationManager translationManager;
    /// <summary>
    /// Only for factory of categories
    /// </summary>
    [SerializeField]
    CategoryDetails categoryDetails;

    public CategoryLabel LoadCategory(Category category)
    {
        GameObject ob = Instantiate(labelCategory);
        ob.transform.SetParent(transform);
        CategoryLabel categoryLabel = ob.GetComponent<CategoryLabel>();
        categoryLabel.Initialize(category, categoryDetails);

        return categoryLabel;
    }

    public CategoryLabel LoadOthersCategoryLabel()
    {
        GameObject ob = Instantiate(labelCategory);
        ob.transform.SetParent(transform);
        CategoryLabel categoryLabel = ob.GetComponent<CategoryLabel>();
        categoryLabel.Initialize(translationManager.GetStaticString(2),emptyBookmarkIcon);

        return categoryLabel;
    }

    /// <summary>
    /// Used to create DateLabel, contains instructions on how to create Labels by date
    /// </summary>
    public DateLabel LoadDate(DateTime date)
    {
        string title = "", dateText = "", id = "";
        CultureInfo cultureInfo = translationManager.GetCultureInfo();

        if (date == default || date >= DateTime.Today.AddDays(7))
        {
            title = translationManager.GetStaticString(2);
            dateText = "";
            id = "Other";
        }
        else if(date < DateTime.Today)
        {
            title = translationManager.GetStaticString(1);
            dateText = "";
            id = "Overdue";
        }
        else
        {
            id = date.ToString();
            for(int i = 0; i < 7; i++)
            {
                if (date == DateTime.Today.AddDays(i))
                {
                    if(i==0)
                    {
                        title = translationManager.GetStaticString(24) + " ( " + date.ToString("dddd", cultureInfo) + " )";
                    }
                    else if(i==1)
                    {
                        title = translationManager.GetStaticString(25) + " ( " + date.ToString("dddd", cultureInfo) + " )";
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
        dateLabel.Initialize(dateText,title,id);

        return dateLabel;
    }
}
