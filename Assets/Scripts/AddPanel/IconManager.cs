using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IconManager : MonoBehaviour
{
    [Header("Category")]
    [SerializeField]
    Sprite bookmarFilledIcon;
    [SerializeField]
    Image categoryIcon;
    Sprite bookmarkEmpty;

    [Header("Date")]
    [SerializeField]
    Sprite dateFilledIcon;
    [SerializeField]
    Image dateIcon;
    Sprite dateEmptyIcon;

    [Header("Comment")]
    [SerializeField]
    Sprite commentFilledIcon;
    [SerializeField]
    Image commentIcon;
    Sprite commentEmptyIcon;

    [Header("Weight")]
    [SerializeField]
    Image weightIcon;

    private void Start()
    {
        bookmarkEmpty = categoryIcon.sprite;
        dateEmptyIcon = dateIcon.sprite;
        commentEmptyIcon = commentIcon.sprite;
    }

    public void SetCategoryIconColor(Color color)
    {
        categoryIcon.sprite = bookmarFilledIcon;
        categoryIcon.color = color;
    }

    public void FillDateIcon()
    {
        dateIcon.sprite = dateFilledIcon;
    }

    public void FillCommentIcon()
    {
        commentIcon.sprite = commentFilledIcon;
    }

    public void SetWeightColor(Color color)
    {
        weightIcon.color = color;
    }

    public void ClearDateIcon()
    {
        dateIcon.sprite = dateEmptyIcon;
    }

    public void ClearCategoryIcon()
    {
        categoryIcon.sprite = bookmarkEmpty;
        categoryIcon.color = Color.white;
    }

    public void ClearCommentIcon()
    {
        commentIcon.sprite = commentEmptyIcon;
    }

    public void ClearWeightIcon()
    {
        weightIcon.color = Color.white;
    }

    public void Clear()
    {
        ClearDateIcon();
        ClearCategoryIcon();
        ClearCommentIcon();
        ClearWeightIcon();
    }

    public bool IsWeightIconWhite()
    {
        return weightIcon.color == Color.white;
    }
}
