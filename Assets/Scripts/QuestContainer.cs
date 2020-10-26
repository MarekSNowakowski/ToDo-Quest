using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestContainer : MonoBehaviour
{
    int numberOfChildren;
    float questPanelHeight = 255;
    RectTransform myRectTransform;


    // Start is called before the first frame update
    void Start()
    {
        numberOfChildren = transform.childCount;
        myRectTransform = GetComponent<RectTransform>();

        myRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, numberOfChildren * questPanelHeight);
    }

}
