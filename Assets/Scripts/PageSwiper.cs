using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 panelLocation;
    private Vector3 indicatorLocation;
    private Vector3 questsLocation;
    [SerializeField]
    float percentThreshold = 0.2f;
    [SerializeField]
    float easing = 0.5f;
    [SerializeField]
    int totalPages = 1;
    [SerializeField]
    GameObject panelIndicator;
    [SerializeField]
    GameObject quests;
    RectTransform questRT;

    public int currentPage = 1;

    void Start()
    {
        panelLocation = transform.position;
        indicatorLocation = panelIndicator.transform.position;
        questsLocation = quests.transform.position;
        questRT = quests.GetComponent<RectTransform>();
    }

    public void OnDrag(PointerEventData data)
    {
        Vector2 difference = data.pressPosition - data.position;
        if(currentPage==1)
        {
            quests.transform.position = questsLocation - new Vector3(0, difference.y, 0);
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        Vector2 difference = data.pressPosition - data.position;
        float percentage = difference.x / Screen.width;
        if (Mathf.Abs(percentage) >= percentThreshold)
        {
            Vector3 newLocation = panelLocation;
            Vector3 newIndicatorLocation = indicatorLocation;
            if (percentage > 0 && currentPage < totalPages)
            {
                currentPage++;
                newLocation += new Vector3(-Screen.width, 0, 0);
                newIndicatorLocation += new Vector3(Screen.width / totalPages, 0, 0);
            }
            else if (percentage < 0 && currentPage > 1)
            {
                currentPage--;
                newLocation += new Vector3(Screen.width, 0, 0);
                newIndicatorLocation += new Vector3(-Screen.width / totalPages, 0, 0);
            }
            panelLocation = newLocation;
            indicatorLocation = newIndicatorLocation;
            StopAllCoroutines();
            StartCoroutine(SmoothMove(transform.position, panelLocation, panelIndicator.transform.position, indicatorLocation, easing));
        }
        else if (currentPage == 1) 
        {
            Debug.Log(quests.transform.position);
            if(quests.transform.position.y < -175)
            {
                questsLocation.y = -175;
                StartCoroutine(SmoothMoveScroll(quests.transform.position, questsLocation, easing));
            }
            else if(quests.transform.position.y > questRT.rect.height / 2)
            {
                questsLocation.y = questRT.rect.height / 2;
                StartCoroutine(SmoothMoveScroll(quests.transform.position, questsLocation, easing));
            }
            else
            {
                questsLocation.y = quests.transform.position.y;
            }
        }
    }

    public void GoToPage(int page)
    {
        if(page!=currentPage)
        {
            Vector3 newLocation = panelLocation;
            Vector3 newIndicatorLocation = indicatorLocation;
            int pageDiff = page - currentPage;
            newLocation += new Vector3(pageDiff * -Screen.width, 0, 0);
            newIndicatorLocation += new Vector3(pageDiff * (Screen.width / totalPages), 0, 0);
            currentPage = page;
            panelLocation = newLocation;
            indicatorLocation = newIndicatorLocation;
            StartCoroutine(SmoothMove(transform.position, newLocation, panelIndicator.transform.position, newIndicatorLocation, easing));
        }
    }

    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, Vector3 indStartPos, Vector3 indEndPos, float seconds)
    {
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            panelIndicator.transform.position = Vector3.Lerp(indStartPos, indEndPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        quests.transform.position = new Vector3(quests.transform.position.x, questsLocation.y);
    }

    IEnumerator SmoothMoveScroll(Vector3 startpos, Vector3 endpos, float seconds)
    {
        startpos.x = Screen.width / 2;
        endpos.x = Screen.width / 2;
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            quests.transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
    }
}