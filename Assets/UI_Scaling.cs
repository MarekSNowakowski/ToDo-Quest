using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Scaling : MonoBehaviour
{
    [SerializeField]
    float heightRatio;

    // Start is called before the first frame update
    void Start()
    {
        GetComponent<RectTransform>().SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, heightRatio * Screen.height);
    }
}
