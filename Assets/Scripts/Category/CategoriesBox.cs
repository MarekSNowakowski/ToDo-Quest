using System;
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
    [SerializeField]
    CategoryManager categoryManager;
    List<CategoryObject> categoriesObjects = new List<CategoryObject>();
    Category categorySelected = null;
    bool categoryLocked = false;

    protected float categoryHeightRatio = 0.045f;
    protected float categoryHeight;
    protected float categoriesBoxMaxHeight;
    protected float height;

    [SerializeField]
    Image categoryImage;
    [SerializeField]
    Sprite emptyCategorySprite;
    [SerializeField]
    Sprite fullCategorySprite;

    private void Awake()
    {
        myRectTransform = gameObject.GetComponent<RectTransform>();
        categoryHeight = categoryHeightRatio * Screen.height;
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
        if(categoryObject)
        {
            Destroy(categoryObject.gameObject);
        }
    }

    public void LoadCategory(Category category)
    {
        GameObject categoryObject = Instantiate(categoryPrefab);
        CategoryObject categoryObjectScript = categoryObject.GetComponent<CategoryObject>();
        categoryObject.transform.SetParent(categoriesContainer);
        categoryObjectScript.Set(category, this);
        categoriesObjects.Add(categoryObjectScript);
    }

    internal void ReLoadCategory(Category editingCategory)
    {
        CategoryObject categoryObject = categoriesObjects.Find(x => x.GetCategory() != null && x.GetCategory().GetID() == editingCategory.GetID());
        CategoryObject categoryObjectScript = categoryObject.GetComponent<CategoryObject>();
        categoryObjectScript.Set(editingCategory, this);
    }

    public void OnCategoryChoose(Category category)
    {
        categorySelected = category;
        categoryImage.sprite = fullCategorySprite;
        categoryImage.color = category.GetColor();
    }

    public void OnCategoryChooseNoCategory()
    {
        ClearIcon();
    }

    public void ClearIcon()
    {
        categorySelected = null;
        categoryImage.sprite = emptyCategorySprite;
        categoryImage.color = Color.white;
        categoryLocked = false;
    }

    public void GoBack()
    {
        if(!categoryLocked)
        {
            ClearIcon();
        }
    }

    public void LockCategory()
    {
        categoryLocked = true;
    }

    public void SubmitCategory()
    {
        categoryManager.ChooseCategory(categorySelected);
        categoryLocked = true;
    }
}
