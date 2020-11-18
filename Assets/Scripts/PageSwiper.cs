using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 panelLocation;
    private Vector3 indicatorLocation;
    [SerializeField]
    float percentThreshold = 0.2f;
    [SerializeField]
    float easing = 0.5f;
    [SerializeField]
    int totalPages = 1;
    [SerializeField]
    GameObject panelIndicator;
    public int currentPage = 1;

    // Start is called before the first frame update

    void Start()
    {
        panelLocation = transform.position;
        indicatorLocation = panelIndicator.transform.position;
    }

    public void OnDrag(PointerEventData data)
    {
        float difference = data.pressPosition.x - data.position.x;
        transform.position = panelLocation - new Vector3(difference, 0, 0);
        panelIndicator.transform.position = indicatorLocation + new Vector3(difference / totalPages, 0, 0);
    }

    public void OnEndDrag(PointerEventData data)
    {
        float percentage = (data.pressPosition.x - data.position.x) / Screen.width;
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
            StartCoroutine(SmoothMove(transform.position, newLocation, panelIndicator.transform.position, newIndicatorLocation, easing));
            panelLocation = newLocation;
            indicatorLocation = newIndicatorLocation;
        }
        else
        {
            StartCoroutine(SmoothMove(transform.position, panelLocation, panelIndicator.transform.position, indicatorLocation, easing));
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
    }
}