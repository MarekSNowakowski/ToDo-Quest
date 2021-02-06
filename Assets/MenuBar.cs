using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuBar : MonoBehaviour
{
    [SerializeField]
    float menuWidthRatio;
    [SerializeField]
    int slideSpeed;
    [SerializeField]
    GameObject blocker;

    float width;
    int currentPosition;
    bool opened = false;

    RectTransform myRectTransform;

    private void Start()
    {
        myRectTransform = GetComponent<RectTransform>();
        width = Screen.width * menuWidthRatio;
        myRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
        transform.position = new Vector2(-width / 2, transform.position.y);
    }

    public void OnMenuButtonPress()
    {
        StopAllCoroutines();
        currentPosition = (int)transform.position.x;
        blocker.SetActive(true);
        StartCoroutine(MenuOpeningCorutine());
    }

    public void OnBlockerPress()
    {
        StopAllCoroutines();
        currentPosition = (int)transform.position.x;
        blocker.SetActive(false);
        StartCoroutine(MenuClosingCorutine());
    }

    IEnumerator MenuOpeningCorutine()
    {
        opened = true;
        for( ; currentPosition <= width/2 ; currentPosition += slideSpeed)
        {
            yield return null;
            transform.position = new Vector2(currentPosition, transform.position.y);
        }
        currentPosition = (int) width / 2;
        transform.position = new Vector2(currentPosition, transform.position.y);
    }

    IEnumerator MenuClosingCorutine()
    {
        opened = false;
        for (; currentPosition >= -width / 2; currentPosition -= slideSpeed)
        {
            yield return null;
            transform.position = new Vector2(currentPosition, transform.position.y);
        }
        currentPosition = (int) -width / 2;
        transform.position = new Vector2(currentPosition, transform.position.y);
    }
}
