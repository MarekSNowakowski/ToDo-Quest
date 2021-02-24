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
    int amount;

    RewardManager rewardManager;

    [Header("Removal")]
    [SerializeField]
    GameObject removeButton;
    [SerializeField]
    GameObject cancelRemovalButton;
    bool toBeRemoved;

    public void Initialize(QuestData questData, string today)
    {
        this.id = questData.ID;
        this.rewardName = questData.reward;
        this.questName = questData.questName;
        this.amount = 1;
        questCompletitionTime = DateTime.Today;
        SetUp(today, "");
    }

    public void Initialize(QuestData questData, string name, int amount, string today)
    {
        this.id = questData.ID;
        this.rewardName = name;
        this.questName = name;
        this.amount = amount;
        questCompletitionTime = DateTime.Today;
        SetUp(today, "");
    }

    public void SetUp(string today, string yesterday)
    {
        nameText.text = GetText();
        if(questCompletitionTime.Date == DateTime.Today)
        {
            dateText.text = today;
        }
        else if(questCompletitionTime.Date == DateTime.Today.AddDays(-1))
        {
            dateText.text = yesterday;
        }
        else
        {
            dateText.text = questCompletitionTime.ToShortDateString();
        }
    }

    internal void IncreaseAmount(int amount)
    {
        this.amount += amount;
    }

    public RewardData Save()
    {
        RewardData saveData = new RewardData(id, rewardName, questName, questCompletitionTime, amount);
        return saveData;
    }

    public void Load(RewardData rewardData, string today, string yesterday)
    {
        this.id = rewardData.ID;
        this.rewardName = rewardData.rewardName;
        this.questName = rewardData.questName;
        this.amount = rewardData.amount;
        questCompletitionTime = rewardData.questCompletitionTime;
        SetUp(today, yesterday);
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
        nameText.text = $"<s>{GetText()}</s>";
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

    public string GetName()
    {
        return rewardName;
    }

    public string GetText()
    {
        if (amount > 1)
        {
            return amount + "x " + rewardName;
        }
        else
        {
            return rewardName;
        }
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
    public int amount;

    public RewardData(string ID, string rewardName, string questName, DateTime questCompletitionTime, int amount)
    {
        this.ID = ID;
        this.rewardName = rewardName;
        this.questName = questName;
        this.questCompletitionTime = questCompletitionTime;
        this.amount = amount;
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

    public string GetText()
    {
        if (amount > 1)
        {
            return amount + "x " + rewardName;
        }
        else
        {
            return rewardName;
        }
    }
}
