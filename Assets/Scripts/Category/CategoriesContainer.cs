using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoriesContainer : QuestContainer
{
    [SerializeField] Transform addCategoryButton;

    public override void Start()
    {
        addCategoryButton.SetAsLastSibling();
        initialHeight = 200;
        base.Start();
    }

    public override void RefreshSize(bool adding)
    {
        base.RefreshSize(adding);
        addCategoryButton.SetParent(transform);
        addCategoryButton.SetAsLastSibling();
    }
}
