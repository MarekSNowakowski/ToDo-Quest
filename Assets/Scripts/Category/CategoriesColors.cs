using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoriesColors : MonoBehaviour
{
    [SerializeField]
    List<CategoryColor> categoriesColors;

    public void UpdateCategories()
    {
        foreach(CategoryColor categoryColor in categoriesColors)
        {
            categoryColor.CheckIfLocked();
        }
    }

    public void BlockWhite()
    {
        categoriesColors[0].Block();
    }
}
