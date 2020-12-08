using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestContainer : MonoBehaviour
{
    [SerializeField]
    protected PageSwiper pageSwiper;

    protected int questCount;
    protected float questPanelHeight = 115;
    protected RectTransform myRectTransform;

    public virtual void Start()
    {
        myRectTransform = gameObject.GetComponent<RectTransform>();
        CountChildren();

        var height = questCount * questPanelHeight;

        myRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        myRectTransform.anchoredPosition = new Vector3(0, -0.5f * height);
    }

    public virtual void CountChildren()
    {
        questCount = 0;
        foreach(Transform child in transform)
        {
            if (child.tag == "Quest") questCount++;
        }
    }

    // Start is called before the first frame update
    void Awake()
    {
        myRectTransform = gameObject.GetComponent<RectTransform>();
    }

    public virtual void RefreshSize(bool adding)
    {
        //numberOfChildren = myRectTransform.childCount;
        if (adding) questCount++;
        else questCount--;
        var height = questCount * questPanelHeight;

        myRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        myRectTransform.anchoredPosition = new Vector3(0, -0.5f * height);
        pageSwiper.resetQuestsPositionl(adding);
    }

}
