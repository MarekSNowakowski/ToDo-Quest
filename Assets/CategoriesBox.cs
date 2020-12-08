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
        if(background.activeInHierarchy)
        {
            background.SetActive(false);
        }
        else
        {
            background.SetActive(true);
            myRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        }
        
    }

    private void OnDisable()
    {
        background.SetActive(false);
    }
}
