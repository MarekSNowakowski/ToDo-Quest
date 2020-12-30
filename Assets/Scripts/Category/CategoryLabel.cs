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


    public void Initialize(Category category)
    {
        categoryIcon.color = category.GetColor();
        categoryName.text = category.GetName();
        questsInside = 0;
        labelID = category.GetID();
    }

    public void Initialize(string text, Sprite sprite)
    {
        categoryName.text = text;
        categoryIcon.sprite = sprite;
        questsInside = 0;
        labelID = "No category";
    }

    //public string GetName()
    //{
    //    return categoryName.text;
    //}
}
