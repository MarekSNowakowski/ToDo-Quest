using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestContainer : MonoBehaviour
{
    [SerializeField]
    PageSwiper pageSwiper;

    int numberOfChildren;
    float questPanelHeight = 115;
    RectTransform myRectTransform;

    private void Start()
    {
        myRectTransform = gameObject.GetComponent<RectTransform>();
        numberOfChildren = myRectTransform.childCount;

        var height = numberOfChildren * questPanelHeight;

        myRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        myRectTransform.anchoredPosition = new Vector3(0, -0.5f * height);
    }

    // Start is called before the first frame update
    void Awake()
    {
        myRectTransform = gameObject.GetComponent<RectTransform>();
    }

    public void RefreshSize(bool adding)
    {
        //numberOfChildren = myRectTransform.childCount;
        if (adding) numberOfChildren++;
        else numberOfChildren--;
        var height = numberOfChildren * questPanelHeight;

        myRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        myRectTransform.anchoredPosition = new Vector3(0, -0.5f * height);
        pageSwiper.resetQuestsPositionl(adding);
    }

}
