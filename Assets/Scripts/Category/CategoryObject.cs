using UnityEngine;
using UnityEngine.UI;

public class CategoryObject : MonoBehaviour
{
    [SerializeField]
    Image categoryColor;
    [SerializeField]
    TMPro.TextMeshProUGUI categoryText;
    Category category;
    /// <summary>
    /// Only for no category object
    /// </summary>
    [SerializeField]
    CategoryManager categoryManager;
    [SerializeField]
    CategoriesBox categoriesBox;

    public void Set(Category category, CategoriesBox categoriesBox)
    {
        this.categoriesBox = categoriesBox;
        if(category == null)
        {
            categoryColor.color = Color.white;
            categoryText.text = "No category";
            this.category = null;
        }
        else
        {
            categoryColor.color = category.GetColor();
            categoryText.text = category.GetName();
            this.category = category;
        }
    }

    public void OnCategoryChoose()
    {
        if(category!=null)
        {
            categoriesBox.OnCategoryChoose(category);
        }
        else
        {
            categoriesBox.OnCategoryChooseNoCategory();
        }
    }

    public Category GetCategory()
    {
        return category;
    }
}