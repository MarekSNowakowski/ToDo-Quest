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

    public void Set(Category category)
    {
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
            category.GetCategoryManager().ChooseCategory(category);
        }
        else
        {
            categoryManager.ChooseCategory(null);
        }
    }

    public Category GetCategory()
    {
        return category;
    }
}