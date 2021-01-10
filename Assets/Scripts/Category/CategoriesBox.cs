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

    protected float categoryHeight = 100;
    protected float categoriesBoxMaxHeight = 500;
    protected float height;

    private void Awake()
    {
        myRectTransform = gameObject.GetComponent<RectTransform>();
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

    public void LoadCategory(Category category)
    {
        GameObject categoryObject = Instantiate(categoryPrefab);
        CategoryObject categoryObjectScript = categoryObject.GetComponent<CategoryObject>();
        categoryObject.transform.SetParent(categoriesContainer);
        categoryObjectScript.Set(category);
    }
}
