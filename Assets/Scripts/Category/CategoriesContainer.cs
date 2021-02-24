﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CategoriesContainer : QuestContainer
{
    [SerializeField] Transform addCategoryButton;

    public override void Start()
    {
        addCategoryButton.SetAsLastSibling();
        initialHeight = 0.0925f * Screen.height;
        base.Start();
    }

    public override void RefreshSize(bool adding, bool dateLabelInteraction, bool categoryLabelInteraction)
    {
        base.RefreshSize(adding, dateLabelInteraction, categoryLabelInteraction);
        SetCategoryButton();
    }

    public void SetCategoryButton()
    {
        addCategoryButton.SetParent(transform);
        addCategoryButton.SetAsLastSibling();
    }

    public virtual void RefreshSizeAfterLabelInteraction(bool adding)
    {
        CountChildren();

        var height = initialHeight + (questCount * questHeight) + (labelCount * labelHeight);

        myRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        myRectTransform.anchoredPosition = new Vector3(0, -0.5f * height);
        pageSwiper.reseCategoryPositionlAfterLabelInteraction(adding);
    }
}
