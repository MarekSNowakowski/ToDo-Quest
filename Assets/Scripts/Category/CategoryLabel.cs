using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryLabel : Label
{
    [SerializeField]
    Image categoryIcon;
    [SerializeField]
    TMPro.TextMeshProUGUI categoryName;
    CategoryDetails categoryDetails;
    Category category;
    [SerializeField]
    Color inactiveColor;


    public void Initialize(Category category, CategoryDetails categoryDetails)
    {
        categoryIcon.color = category.GetColor();
        categoryName.text = category.GetName();
        questsInside = 0;
        labelID = category.GetID();
        this.category = category;
        this.categoryDetails = categoryDetails;
    }

    public void Initialize(string text, Sprite sprite)
    {
        categoryName.text = text;
        categoryIcon.sprite = sprite;
        questsInside = 0;
        labelID = "No category";
    }

    public void OnCategoryPress()
    {
        if(category!=null && categoryDetails != null)
        {
            categoryDetails.ShowCategoryDetails(category);
        }
    }

    public override void QuestAdded()
    {
        TurnActive();
        base.QuestAdded();
    }

    public void TurnInactive()
    {
        categoryName.color = inactiveColor;
    }

    public void TurnActive()
    {
        categoryName.color = Color.white;
    }

    public void EditLabel(string name, Color color)
    {
        categoryName.text = name;
        categoryIcon.color = color;
    }


    //public string GetName()
    //{
    //    return categoryName.text;
    //}
}
