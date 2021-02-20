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

    QuestData questData;
    List<SubQuest> subQuestObjects = new List<SubQuest>();
    protected float subQuestHeightRatio = 0.045f;

    public void LoadSubQuests(List<SubQuestData> subQuests, QuestData questData)
    {
        this.questData = questData;
        foreach(SubQuestData data in subQuests)
        {
            GameObject ob = Instantiate(subQuestObject);
            ob.transform.SetParent(transform);
            SubQuest subQuest = ob.GetComponent<SubQuest>();
            subQuest.Set(data, this);
            subQuestObjects.Add(subQuest);
        }
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

        RectTransform myRectTransform = gameObject.GetComponent<RectTransform>();

        float questHeight = subQuestHeightRatio * Screen.height;

        var height = questCount * questHeight;

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
    }
}
