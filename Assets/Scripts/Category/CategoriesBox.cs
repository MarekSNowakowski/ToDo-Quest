﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CategoriesBox : MonoBehaviour
{
    protected RectTransform myRectTransform;

    [SerializeField]
    GameObject background;
    [SerializeField]
    GameObject categoryPrefab;
    [SerializeField]
    RectTransform categoriesContainer;
    List<CategoryObject> categoriesObjects = new List<CategoryObject>();

    protected float categoryHeightRatio = 0.045f;
    protected float categoryHeight;
    protected float categoriesBoxMaxHeight;
    protected float height;

    private void Awake()
    {
        myRectTransform = gameObject.GetComponent<RectTransform>();
        categoryHeight = categoryHeightRatio * Screen.height;
        categoriesBoxMaxHeight = 5 * categoryHeight;
    }

    public virtual void RefreshSize(int numberOfCategories)
    {
        height = (numberOfCategories + 1) * categoryHeight;
    }

    public void OnCategoriesChoosePress()
    {
        if (background.activeInHierarchy)
        {
            CloseCategoriesContainer();
        }
        else
        {
            background.SetActive(true);
            categoriesContainer.gameObject.SetActive(true);
            categoriesContainer.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            if (height > categoriesBoxMaxHeight)
            {
                myRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, categoriesBoxMaxHeight);
            }
            else
            {
                myRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            }
        }
        
    }

    public void CloseCategoriesContainer()
    {
        background.SetActive(false);
        categoriesContainer.gameObject.SetActive(false);
    }

    private void OnDisable()
    {
        background.SetActive(false);
        categoriesContainer.gameObject.SetActive(false);
    }

    public void LoadCategories(List<Category> categories)
    {
        foreach(Category category in categories)
        {
            LoadCategory(category);
        }
    }

    public void UnLoadCategory(Category category)
    {
        CategoryObject categoryObject = categoriesObjects.Find(x => x.GetCategory() != null && x.GetCategory().GetID() == category.GetID());
        Destroy(categoryObject.gameObject);
    }

    public void LoadCategory(Category category)
    {
        GameObject categoryObject = Instantiate(categoryPrefab);
        CategoryObject categoryObjectScript = categoryObject.GetComponent<CategoryObject>();
        categoryObject.transform.SetParent(categoriesContainer);
        categoryObjectScript.Set(category);
        categoriesObjects.Add(categoryObjectScript);
    }

    internal void ReLoadCategory(Category editingCategory)
    {
        CategoryObject categoryObject = categoriesObjects.Find(x => x.GetCategory() != null && x.GetCategory().GetID() == editingCategory.GetID());
        CategoryObject categoryObjectScript = categoryObject.GetComponent<CategoryObject>();
        categoryObjectScript.Set(editingCategory);
    }
}
