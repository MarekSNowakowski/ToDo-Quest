using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Class for creating all types of labels
public class LabelFactory : MonoBehaviour
{
    [SerializeField]
    GameObject labelCategory;
    [SerializeField]
    Sprite emptyBookmarkIcon;
    string emptyLabelText = "Others";

    public CategoryLabel LoadCategory(Category category)
    {
        GameObject ob = Instantiate(labelCategory);
        ob.transform.SetParent(transform);
        CategoryLabel categoryLabel = ob.GetComponent<CategoryLabel>();
        categoryLabel.Initialize(category);

        return categoryLabel;
    }

    public CategoryLabel LoadOthersCategoryLabel()
    {
        GameObject ob = Instantiate(labelCategory);
        ob.transform.SetParent(transform);
        CategoryLabel categoryLabel = ob.GetComponent<CategoryLabel>();
        categoryLabel.Initialize(emptyLabelText,emptyBookmarkIcon);

        return categoryLabel;
    }
}
