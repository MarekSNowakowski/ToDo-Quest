using UnityEngine;
using UnityEngine.UI;

public class CategoryObject : MonoBehaviour
{
    [SerializeField]
    Image categoryColor;
    [SerializeField]
    TMPro.TextMeshProUGUI categoryText;
    Category category;

    public void Set(Category category)
    {
        categoryColor.color = category.GetColor();
        categoryText.text = category.GetName();
        this.category = category;
    }

    public void OnCategoryChoose()
    {
        category.GetCategoryManager().ChooseCategory(category);
    }
}