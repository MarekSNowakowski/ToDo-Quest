using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubQuestDisplayer : MonoBehaviour
{
    [SerializeField]
    GameObject subQuestObject;
    [SerializeField]
    QuestManager questManager;
    [SerializeField]
    QuestDetails questDetails;

    [SerializeField]
    TMPro.TextMeshProUGUI counterText;
    [SerializeField]
    RectTransform foldImageRT;

    QuestData questData;
    List<SubQuest> subQuestObjects = new List<SubQuest>();
    protected float subQuestHeightRatio = 0.045f;
    bool folded = false;

    public void LoadSubQuests(List<SubQuestData> subQuests, QuestData questData)
    {
        this.questData = questData;
        int completed = 0;
        foreach (SubQuestData data in subQuests)
        {
            if (data.completed) completed++;
            GameObject ob = Instantiate(subQuestObject);
            ob.transform.SetParent(transform);
            SubQuest subQuest = ob.GetComponent<SubQuest>();
            subQuest.Set(data, this);
            subQuestObjects.Add(subQuest);
        }
        counterText.text = $"{completed} / {subQuests.Count}";
        UpdateSize(subQuests.Count);
    }

    public void Unload()
    {
        foreach(SubQuest subQuest in subQuestObjects)
        {
            Destroy(subQuest.gameObject);
        }
        subQuestObjects.Clear();
        UpdateSize(0);
    }

    public void UpdateSize(int questCount)
    {
        folded = false;
        //Ensure that icon is rotated properly
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        foldImageRT.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 270);

        RectTransform myRectTransform = gameObject.GetComponent<RectTransform>();

        float questHeight = subQuestHeightRatio * Screen.height;

        var height = 0.045f * Screen.height + questCount * questHeight;

        myRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
        myRectTransform.anchoredPosition = new Vector3(0, -0.5f * height);
    }

    public void RemoveSubQuest(SubQuestData subQuest)
    {
        questManager.RemoveSubQuest(questData.ID, subQuest);
        questDetails.ReloadSubQuestDisplayer();
    }

    public void ChangeSubQuestCompletition(bool completed, SubQuestData subQuestData)
    {
        int subQuestID = questData.subQuests.FindIndex(x => x.Equals(subQuestData));
        questManager.ChangeCompletition(completed, questData.ID, subQuestID);
        UpdateCountText();
    }

    public void UpdateCountText()
    {
        int completed = 0;
        foreach(SubQuest subQuest in subQuestObjects)
        {
            if (subQuest.IsCompleted()) completed++; 
        }
        counterText.text = $"{completed} / {subQuestObjects.Count}";
    }

    public void OnFoldButtonPressed()
    {
        if(folded)
        {
            folded = false;
            StartCoroutine(UnFoldCoroutine());
        }
        else
        {
            folded = true;
            StartCoroutine(FoldCoroutine());
        }
    }

    IEnumerator FoldCoroutine()
    {
        RectTransform myRectTransform = gameObject.GetComponent<RectTransform>();
        float questHeight = subQuestHeightRatio * Screen.height;
        float height = 0.045f * Screen.height + subQuestObjects.Count * questHeight;

        foreach (SubQuest subQuest in subQuestObjects)
        {
            height -= questHeight;
            subQuest.gameObject.SetActive(false);
            myRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            myRectTransform.anchoredPosition = new Vector3(0, -0.5f * height);
            yield return new WaitForSeconds(0.1f);
        }
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        foldImageRT.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 90);
    }

    IEnumerator UnFoldCoroutine()
    {
        RectTransform myRectTransform = gameObject.GetComponent<RectTransform>();
        float questHeight = subQuestHeightRatio * Screen.height;
        float height = 0.045f * Screen.height;

        foreach (SubQuest subQuest in subQuestObjects)
        {
            height += questHeight;
            subQuest.gameObject.SetActive(true);
            myRectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
            myRectTransform.anchoredPosition = new Vector3(0.5f * Screen.width, -0.5f * height);
            yield return new WaitForSeconds(0.1f);
        }
        Vector3 eulerRotation = transform.rotation.eulerAngles;
        foldImageRT.rotation = Quaternion.Euler(eulerRotation.x, eulerRotation.y, 270);
    }
}
