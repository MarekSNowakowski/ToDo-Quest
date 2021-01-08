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
    protected float questPanelHeight = 100;
    protected float labelHeight = 80;
    protected RectTransform myRectTransform;

    public virtual void Start()
    {
        myRectTransform = gameObject.GetComponent<RectTransform>();
        CountChildren();

        var height = initialHeight + (questCount * questPanelHeight) + (labelCount * labelHeight);

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

    public virtual void RefreshSize(bool adding)
    {
        CountChildren();

        if(!adding)
        {
            questCount--;
        }

        var height = initialHeight + (questCount * questPanelHeight) + (labelCount * labelHeight);
        Debug.Log(height);

        myRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        myRectTransform.anchoredPosition = new Vector3(0, -0.5f * height);
        pageSwiper.resetQuestsPositionl(adding);
    }

}
