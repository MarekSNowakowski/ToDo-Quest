using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoryLabel : MonoBehaviour
{
    [SerializeField]
    Image categoryIcon;
    [SerializeField]
    TMPro.TextMeshProUGUI categoryName;

    string categoryID;
    int questsInside;

    public void Initialize(Category category)
    {
        categoryIcon.color = category.GetColor();
        categoryName.text = category.GetName();
        questsInside = 0;
        categoryID = category.GetID();
    }

    public void Initialize(string text, Sprite sprite)
    {
        categoryName.text = text;
        categoryIcon.sprite = sprite;
        categoryID = "No category";
    }

    public string GetName()
    {
        return categoryName.text;
    }

    public void QuestAdded()
    {
        questsInside++;
    }

    public void QuestRemoved()
    {
        questsInside--;
    }

    public int GetNumberOfQuestsInside()
    {
        return questsInside;
    }

    public string GetID()
    {
        return categoryID;
    }
}
