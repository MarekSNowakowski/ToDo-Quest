using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPanelSlide : MonoBehaviour
{
    [SerializeField]
    RectTransform addPanel;
    [SerializeField]
    RectTransform blocker;
    [SerializeField]
    int slideTimeMs = 200;
    [SerializeField]
    int slideFrames = 10;
    [SerializeField]
    float addPanelHeight = 750;

    float screenHeight = Screen.height;

    bool corutineActive;
    Coroutine unflodingCo;
    Coroutine foldingCo;

    void Start()
    {
        addPanel.sizeDelta = new Vector2(0,0);
        blocker.sizeDelta = new Vector2(0, screenHeight);
    }

    public void Unflod()
    {
        if(!corutineActive)
            unflodingCo = StartCoroutine(r_unflod());
    }

    public void Fold()
    {
        if(!corutineActive)
            foldingCo = StartCoroutine(r_fold());
    }

    IEnumerator r_unflod()
    {
        corutineActive = true;
        for (int i = 0; i <= slideFrames; i++)
        {
            addPanel.sizeDelta = new Vector2(0, i*(addPanelHeight/slideFrames));
            blocker.sizeDelta = new Vector2(0, screenHeight - i * (addPanelHeight / slideFrames));
            Debug.Log(addPanel.sizeDelta);
            yield return new WaitForSeconds(slideTimeMs/slideFrames*0.001f);
        }
        corutineActive = false;
    }

    IEnumerator r_fold()
    {
        corutineActive = true;
        for(float i = slideFrames; i>0; i--)
        {
            addPanel.sizeDelta = new Vector2(0, i * (addPanelHeight / slideFrames));
            blocker.sizeDelta = new Vector2(0, screenHeight - i * (addPanelHeight / slideFrames));
            yield return new WaitForSeconds(slideTimeMs / slideFrames*0.001f);
        }
        corutineActive = false;
        this.gameObject.SetActive(false);
    }
}
