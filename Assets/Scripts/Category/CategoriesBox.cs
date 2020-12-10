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
    GameObject noCategoriesText;
    [SerializeField]
    GameObject categoryPrefab;
    [SerializeField]
    Transform categoriesContainer;

    protected float categoryHeight = 100;
    protected float height;

    private void Awake()
    {
        myRectTransform = gameObject.GetComponent<RectTransform>();
    }

    public virtual void RefreshSize(int numberOfCategories)
    {
        height = numberOfCategories * categoryHeight;
        if(height==0)
        {
            height = categoryHeight;
            noCategoriesText.SetActive(true);
        }
        else if(noCategoriesText.activeInHierarchy)
        {
            noCategoriesText.SetActive(false);
        }
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
            myRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
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
