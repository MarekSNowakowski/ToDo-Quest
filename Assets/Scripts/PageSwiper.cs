using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PageSwiper : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Vector3 panelLocation;
    private Vector3 indicatorLocation;
    private Vector3 questsLocation;
    private Vector3 categoriesLocation;
    private Vector3 rewardsLocation;
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
    [SerializeField]
    GameObject rewards;
    RectTransform rewardsRT;
    [SerializeField]
    RectTransform rewardMaskRT;
    [SerializeField]
    GameObject categories;
    RectTransform categoriesRT;
    float upperPanelHeight = 120;
    float questSize = 100;
    float rewardSize = 100;
    float rewardPanelDistance;

    public int currentPage = 1;
    private bool corutineRunning;

    void Start()
    {
        panelLocation = transform.position;
        indicatorLocation = panelIndicator.transform.position;
        questsLocation = quests.transform.position;
        categoriesLocation = quests.transform.position;
        categoriesLocation.y += 180;    //Qfix don't sure why? Create category button has height of 200
        rewardsLocation = new Vector2(Screen.width / 2, rewards.transform.position.y);
        questRT = quests.GetComponent<RectTransform>();
        rewardsRT = rewards.GetComponent<RectTransform>();
        categoriesRT = categories.GetComponent<RectTransform>();
        rewardPanelDistance = Screen.height - rewardMaskRT.rect.height;
    }

    public void OnDrag(PointerEventData data)
    {
        Vector2 difference = data.pressPosition - data.position;
        if (currentPage == 1 && !corutineRunning)
        {
            quests.transform.position = questsLocation - new Vector3(0, difference.y, 0);
        }
        else if(currentPage == 2 && !corutineRunning)
        {
            categories.transform.position = categoriesLocation - new Vector3(0, difference.y, 0);
        }
        else if (currentPage == 3 && !corutineRunning && data.pressPosition.y < rewardMaskRT.rect.height)
        {
            rewards.transform.position = rewardsLocation - new Vector3(0, difference.y, 0);
        }
    }

    public void OnEndDrag(PointerEventData data)
    {
        Vector2 difference = data.pressPosition - data.position;
        float percentage = difference.x / Screen.width;
        //Change the page
        if (Mathf.Abs(percentage) >= percentThreshold &&
            ((percentage > 0 && currentPage < totalPages) || (percentage < 0 && currentPage > 1)))    //Prevent scroll freezing for a second while we swipe without changing page
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
        else if (currentPage == 1 && !corutineRunning)
        {
            //Scroll to the begining
            float begining = Screen.height - upperPanelHeight - (questRT.rect.height / 2);
            if (quests.transform.position.y < begining || questRT.rect.height < Screen.height - upperPanelHeight)
            {
                questsLocation.y = begining;
                StartCoroutine(SmoothMoveScroll(quests,quests.transform.position, questsLocation, easing));
            }
            //Scroll to the end
            else if (quests.transform.position.y > questRT.rect.height / 2)
            {
                questsLocation.y = questRT.rect.height / 2;
                StartCoroutine(SmoothMoveScroll(quests,quests.transform.position, questsLocation, easing));
            }
            //Save the location
            else
            {
                questsLocation.y = quests.transform.position.y;
            }
        }
        else if (currentPage == 2 && !corutineRunning)
        {
            //Scroll to the begining
            float begining = Screen.height - upperPanelHeight - (categoriesRT.rect.height / 2);
            if (categories.transform.position.y < begining || categoriesRT.rect.height < Screen.height - upperPanelHeight)
            {
                categoriesLocation.y = begining;
                StartCoroutine(SmoothMoveScroll(categories, categories.transform.position, categoriesLocation, easing));
            }
            //Scroll to the end
            else if (categories.transform.position.y > categoriesRT.rect.height / 2)
            {
                categoriesLocation.y = categoriesRT.rect.height / 2;
                StartCoroutine(SmoothMoveScroll(categories, categories.transform.position, categoriesLocation, easing));
            }
            //Save the location
            else
            {
                categoriesLocation.y = categories.transform.position.y;
            }
        }
        else if (currentPage == 3 && !corutineRunning)
        {
            float begining = Screen.height - rewardPanelDistance  - (rewardsRT.rect.height / 2);
            if (rewards.transform.position.y < begining || rewardsRT.rect.height < Screen.height - rewardPanelDistance)
            {
                rewardsLocation.y = begining;
                StartCoroutine(SmoothMoveScroll(rewards,rewards.transform.position, rewardsLocation, easing));
            }
            //Scroll to the end
            else if (rewards.transform.position.y > rewardsRT.rect.height / 2)
            {
                rewardsLocation.y = rewardsRT.rect.height / 2;
                StartCoroutine(SmoothMoveScroll(rewards,rewards.transform.position, rewardsLocation, easing));
            }
            //Save the location
            else
            {
                rewardsLocation.y = rewards.transform.position.y;
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
            StopAllCoroutines();
            StartCoroutine(SmoothMove(transform.position, newLocation, panelIndicator.transform.position, newIndicatorLocation, easing));
        }
    }

    IEnumerator SmoothMove(Vector3 startpos, Vector3 endpos, Vector3 indStartPos, Vector3 indEndPos, float seconds)
    {
        corutineRunning = true;
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            panelIndicator.transform.position = Vector3.Lerp(indStartPos, indEndPos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        quests.transform.position = new Vector3(quests.transform.position.x, questsLocation.y);
        categories.transform.position = new Vector3(categories.transform.position.x, categoriesLocation.y);
        rewards.transform.position = new Vector3(rewards.transform.position.x, rewardsLocation.y);
        corutineRunning = false;
    }

    IEnumerator SmoothMoveScroll(GameObject ob, Vector3 startpos, Vector3 endpos, float seconds)
    {
        corutineRunning = true;
        startpos.x = Screen.width / 2;
        endpos.x = Screen.width / 2;
        float t = 0f;
        while (t <= 1.0)
        {
            t += Time.deltaTime / seconds;
            ob.transform.position = Vector3.Lerp(startpos, endpos, Mathf.SmoothStep(0f, 1f, t));
            yield return null;
        }
        corutineRunning = false;
    }

    //To be changed? We want to see quest we just created/stay where we were while deleating

    public void resetQuestsPositionl(bool adding)   
    {
        float panelDifference = 25;  //default 25 but no idea why :(
        float endQuest = questRT.rect.height / 2;
        float endCategories = categoriesRT.rect.height / 2;
        if (adding)
        {
            //When you are at the end
            if (endQuest - (questSize / 2) == questsLocation.y + panelDifference)
            {
                questsLocation.y += questSize / 2 + panelDifference;
            }
            else
            {
                questsLocation.y -= questSize / 4;
            }
            //When you are at the end
            if (endCategories - (questSize / 2) == categoriesLocation.y + panelDifference)
            {
                categoriesLocation.y += questSize / 2 + panelDifference;
            }
            else
            {
                categoriesLocation.y -= questSize / 4;
            }
        }
        else
        {
            Debug.Log(questRT.rect.height + " > " + (Screen.height - upperPanelHeight) + " " + (endQuest - (questSize / 2)) + " < " + (questsLocation.y - panelDifference));
            //When you are at the end?
            if (questRT.rect.height > Screen.height - upperPanelHeight && endQuest - (questSize / 2)  < questsLocation.y)
            {
                questsLocation.y -= questSize / 4;
            }
            else
            {
                questsLocation.y += questSize / 4;
            }
            //When you are at the end?
            if (categoriesRT.rect.height > Screen.height - upperPanelHeight && endCategories - (questSize / 2) - 0.5 < categoriesLocation.y)
            {
                categoriesLocation.y -= questSize / 4;
            }
            else
            {
                categoriesLocation.y += questSize / 4;
            }
        }
        quests.transform.position = new Vector3(quests.transform.position.x, questsLocation.y);
        categories.transform.position = new Vector3(categories.transform.position.x, categoriesLocation.y);
    }

    public void resetRewardsPosition(bool adding)
    {
        float end = rewardsRT.rect.height / 2;
        if (adding)
        {
            if (end - (rewardSize / 2) == rewardsLocation.y)
            {
                rewardsLocation.y += rewardSize / 2;
            }
            else
            {
                rewardsLocation.y -= rewardSize / 2;
            }
        }
        else
        {
            if (rewardsRT.rect.height > Screen.height - rewardPanelDistance && end - (rewardSize / 2) - 0.5 < rewardsLocation.y)
            {
                rewardsLocation.y -= rewardSize / 2 + 0.5f;
            }
            else
            {
                rewardsLocation.y += rewardSize / 2;
            }
        }
        rewards.transform.position = new Vector3(rewards.transform.position.x, rewardsLocation.y);
    }
}