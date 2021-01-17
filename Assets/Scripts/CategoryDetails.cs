using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CategoryDetails : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI categoryName;
    [SerializeField]
    Image categoryImage;
    [SerializeField]
    QuestDisplayer questDisplayer;
    [SerializeField]
    QuestManager questManager;
    [SerializeField]
    CategoryManager categoryManager;
    [SerializeField]
    GameObject categoryDetails;
    Category displayedCategory;

    public void ShowCategoryDetails(Category category)
    {
        categoryDetails.SetActive(true);
        categoryName.text = category.GetName();
        categoryImage.color = category.GetColor();
        displayedCategory = category;
        questManager.ShowCategoryQuests(questDisplayer, category);
    }

    public void CloseDetails()
    {
        categoryName.text = "";
        categoryImage.color = Color.white;
        displayedCategory = null;
        questDisplayer.Unload();
        categoryDetails.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void EditCategory()
    {
        //TBD
        CloseDetails();
    }

    public void RemoveCategory()
    {
        questManager.RemoveCategory(displayedCategory);
        categoryManager.RemoveCategory(displayedCategory);
        CloseDetails();
    }
}