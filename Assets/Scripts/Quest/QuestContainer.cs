using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestContainer : MonoBehaviour
{
    [SerializeField]
    protected PageSwiper pageSwiper;

    protected float initialHeight = 0;
    protected int questCount;
    protected int labelCount;
    protected float questHeightRatio = 0.045f;
    protected float labelHeightRatio = 0.03f;
    protected float questHeight;
    protected float labelHeight;
    protected RectTransform myRectTransform;

    public virtual void Start()
    {
        myRectTransform = gameObject.GetComponent<RectTransform>();
        CountChildren();

        questHeight = questHeightRatio * Screen.height;
        labelHeight = labelHeightRatio * Screen.height;

        questHeight = Screen.width * questHeightRatio;
        var height = initialHeight + (questCount * questHeight) + (labelCount * labelHeight);

        myRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        myRectTransform.anchoredPosition = new Vector3(0, -0.5f * height);
    }

    public virtual void CountChildren()
    {
        questCount = 0;
        labelCount = 0;
        foreach(Transform child in transform)
        {
            if (child.tag == "Quest") questCount++;
            else if (child.tag == "Label") labelCount++;
        }
    }

    void Awake()
    {
        myRectTransform = gameObject.GetComponent<RectTransform>();
    }

    public virtual void RefreshSize(bool adding, bool dateLabelInteraction, bool categoryLabelInteraction)
    {
        CountChildren();

        if(!adding)
        {
            questCount--;
        }

        if(!adding && (dateLabelInteraction || categoryLabelInteraction))
        {
            labelCount--;
        }

        var height = initialHeight + (questCount * questHeight) + (labelCount * labelHeight);

        myRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        myRectTransform.anchoredPosition = new Vector3(0, -0.5f * height);
        pageSwiper.resetQuestsPositionl(adding, dateLabelInteraction, categoryLabelInteraction);
    }

}
