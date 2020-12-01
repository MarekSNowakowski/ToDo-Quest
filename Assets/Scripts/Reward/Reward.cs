using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reward : MonoBehaviour, IComparable<Reward>
{
    [SerializeField]
    TMPro.TextMeshProUGUI nameText;
    [SerializeField]
    TMPro.TextMeshProUGUI dateText;

    [Header("Reward data")]
    public string id;
    string rewardName;
    string questName;
    DateTime questCompletitionTime;

    RewardManager rewardManager;

    [Header("Removal")]
    [SerializeField]
    GameObject removeButton;
    [SerializeField]
    GameObject cancelRemovalButton;
    bool toBeRemoved;

    public void Initialize(QuestData questData)
    {
        this.id = questData.ID;
        this.rewardName = questData.reward;
        this.questName = questData.questName;
        questCompletitionTime = DateTime.Today;
        setUp();
    }

    void setUp()
    {
        nameText.text = rewardName;
        if(questCompletitionTime.Date == DateTime.Today)
        {
            dateText.text = "Today";
        }
        else if(questCompletitionTime.Date == DateTime.Today.AddDays(-1))
        {
            dateText.text = "Yesterday";
        }
        else
        {
            dateText.text = questCompletitionTime.ToShortDateString();
        }
    }

    public RewardData Save()
    {
        RewardData saveData = new RewardData(id, rewardName, questName, questCompletitionTime);
        return saveData;
    }

    public void Load(RewardData rewardData)
    {
        this.id = rewardData.ID;
        this.rewardName = rewardData.rewardName;
        this.questName = rewardData.questName;
        questCompletitionTime = rewardData.questCompletitionTime;
        setUp();
    }

    public void GetManager(RewardManager rewardManager)
    {
        this.rewardManager = rewardManager;
    }

    public void RemoveSelf()
    {
        StartCoroutine(ToBeRemovedCO());
    }

    public void CancelRemoval()
    {
        toBeRemoved = false;
        nameText.text = rewardName;
        cancelRemovalButton.SetActive(false);
        removeButton.SetActive(true);
    }

    IEnumerator ToBeRemovedCO()
    {
        float questRemovalTime = 3;

        toBeRemoved = true;
        nameText.text = $"<s>{rewardName}</s>";
        cancelRemovalButton.SetActive(true);
        removeButton.SetActive(false);

        for (int i = 0; toBeRemoved && i < questRemovalTime; i++)
        {
            yield return new WaitForSeconds(1);
        }
        if (toBeRemoved)
        {
            rewardManager.RemoveReward(this);
        }
        toBeRemoved = false;
    }

    public void ShowDetails()
    {
        RewardData rewardData = Save();
        rewardManager.ShowRewardDetails(rewardData);
    }

    public int CompareTo(Reward other)
    {
        return (-1) * questCompletitionTime.CompareTo(other.questCompletitionTime);
    }
}

[System.Serializable]
public struct RewardData : IComparable<RewardData>
{
    public string ID;
    public string rewardName;
    public string questName;
    public DateTime questCompletitionTime;

    public RewardData(string ID, string rewardName, string questName, DateTime questCompletitionTime)
    {
        this.ID = ID;
        this.rewardName = rewardName;
        this.questName = questName;
        this.questCompletitionTime = questCompletitionTime;
    }

    public int CompareTo(RewardData other)
    {
        return (-1) * questCompletitionTime.CompareTo(other.questCompletitionTime);
    }

    public void UpdateData(string ID, string rewardName, string questName, DateTime questCompletitionTime)
    {
        this.ID = ID;
        this.rewardName = rewardName;
        this.questName = questName;
        this.questCompletitionTime = questCompletitionTime;
    }
}
