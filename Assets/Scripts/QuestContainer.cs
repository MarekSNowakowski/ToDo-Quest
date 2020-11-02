using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestContainer : MonoBehaviour
{
    int numberOfChildren;
    float questPanelHeight = 255;
    RectTransform myRectTransform;

    private void Start()
    {
        myRectTransform = gameObject.GetComponent<RectTransform>();
        RefreshSize();
    }

    // Start is called before the first frame update
    void Awake()
    {
        myRectTransform = gameObject.GetComponent<RectTransform>();
    }

    public void RefreshSize()
    {
        numberOfChildren = myRectTransform.childCount;
        Debug.Log(numberOfChildren);

        var height = numberOfChildren * questPanelHeight;

        myRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        myRectTransform.anchoredPosition = new Vector3(0, -0.5f * height);
    }

}
