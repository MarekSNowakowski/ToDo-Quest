﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour, IComparable<Quest>
{
    [SerializeField]
    TMPro.TextMeshProUGUI nameText;
    [SerializeField]
    GameObject rewardImage;
    [SerializeField]
    GameObject commentImage;
    [SerializeField]
    Image weightImage;
    [SerializeField]
    Image categoryImage;

    QuestManager questManager;
    //Important for sorting
    QuestDisplayerState sortingState;

    [Header("Quest Data")]
    public string ID;
    string questName;
    string reward;
    int weight;
    string comment;
    DateTime questCreationDateTime;
    Category category;

    [Header("Removal")]
    [SerializeField]
    GameObject removeButton;
    [SerializeField]
    GameObject cancelRemovalButton;
    bool toBeRemoved;
    private bool thisQuestIsRemoving;

    void SetUp()
    {
        nameText.text = questName;
        if (reward != "" && reward != null) rewardImage.gameObject.SetActive(true);
        if (comment != "" && comment != null) commentImage.gameObject.SetActive(true);
        if (category != null && sortingState != QuestDisplayerState.SortByCategory)
        {
            categoryImage.gameObject.SetActive(true);
            categoryImage.color = category.GetColor();
        }
        switch (weight)
        {
            case 1:
                weightImage.color = new Color(0, 210, 0);
                break;
            case 2:
                weightImage.color = new Color(0, 0, 210);
                break;
            case 3:
                weightImage.color = new Color(210, 0, 0);
                break;
            default:
                break;
        }

    }

    public QuestData Save()
    {
        QuestData saveData = new QuestData(ID, questName, reward, weight, comment, category, questCreationDateTime);
        return saveData;
    }

    public void Load(QuestData questData, QuestDisplayerState state)
    {
        this.ID = questData.ID;
        this.questName = questData.questName;
        this.reward = questData.reward;
        this.weight = questData.weight;
        this.comment = questData.comment;
        this.questCreationDateTime = questData.creationDateTime;
        this.category = questData.category;
        this.sortingState = state;
        SetUp();
    }

    public void GetManager(QuestManager questManager)
    {
        this.questManager = questManager;
    }

    public void Remove()
    {
        questManager.StartRemovall(Save());
        thisQuestIsRemoving = true;
    }

    public void RemoveSelf()
    {
        StartCoroutine(ToBeRemovedCO());
    }

    public void CancellRemoval()
    {
        toBeRemoved = false;
        thisQuestIsRemoving = false;
        nameText.text = questName;
        cancelRemovalButton.SetActive(false);
        removeButton.SetActive(true);
    }

    public void StartCancellRemoval()
    {
        questManager.CancellRemoval(ID);
    }

    IEnumerator ToBeRemovedCO()
    {
        float questRemovalTime = 3;

        toBeRemoved = true;
        nameText.text = $"<s>{questName}</s>";
        cancelRemovalButton.SetActive(true);
        removeButton.SetActive(false);

        for (int i = 0; toBeRemoved && i < questRemovalTime; i++)
        {
            yield return new WaitForSeconds(1);
        }
        if(toBeRemoved && thisQuestIsRemoving)
        {
            questManager.RemoveQuest(this.ID);
        }
        toBeRemoved = false;
    }

    public void ShowDetails()
    {
        QuestData questData = Save();
        questManager.ShowQuestDetails(questData);
    }

    public int CompareTo(Quest other)
    {
        if(sortingState == QuestDisplayerState.SortByCategory)
        {
            if (this.category != other.category) return (-1) * category.CompareTo(other.category);
            else if (this.weight != other.weight) return (-1) * weight.CompareTo(other.weight);
            else return (-1) * questCreationDateTime.CompareTo(other.questCreationDateTime);
        }
        else
        {
            if (this.weight != other.weight) return (-1) * weight.CompareTo(other.weight);
            else return (-1) * questCreationDateTime.CompareTo(other.questCreationDateTime);
        }
    }

    public string GetName()
    {
        return this.questName;
    }

    public Category GetCategory()
    {
        return category;
    }
}

[Serializable]
public struct QuestData : IComparable<QuestData>
{
    public string ID;
    public string questName;
    public string reward;
    public int weight;
    public string comment;
    public DateTime creationDateTime;
    public Category category;

    public QuestData(string ID, string questName, string reward, int weight, string comment, Category category, DateTime creationDateTime)
    {
        this.ID = ID;
        this.questName = questName;
        this.reward = reward;
        this.weight = weight;
        this.comment = comment;
        this.creationDateTime = DateTime.Now;
        this.category = category;
    }

    public void Initialize()
    {
        creationDateTime = DateTime.Now;
        ID = CorrelationIdGenerator.GetNextId();
    }

    public int CompareTo(QuestData other)
    {
        if (this.weight != other.weight) return (-1) * weight.CompareTo(other.weight);
        else return (-1) * creationDateTime.CompareTo(other.creationDateTime);
    }

    public void UpdateData(string questName, string reward, int weight, string comment)
    {
        this.questName = questName;
        this.reward = reward;
        this.weight = weight;
        this.comment = comment;
    }
}

internal static class CorrelationIdGenerator
{
    private static long _lastId = DateTime.UtcNow.Ticks;

    public static string GetNextId() => GenerateId(Interlocked.Increment(ref _lastId));

    private static string GenerateId(long id)
    {

        string _encode32Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUV";

        var buffer = new char[13];

        buffer[0] = _encode32Chars[(int)(id >> 60) & 31];
        buffer[1] = _encode32Chars[(int)(id >> 55) & 31];
        buffer[2] = _encode32Chars[(int)(id >> 50) & 31];
        buffer[3] = _encode32Chars[(int)(id >> 45) & 31];
        buffer[4] = _encode32Chars[(int)(id >> 40) & 31];
        buffer[5] = _encode32Chars[(int)(id >> 35) & 31];
        buffer[6] = _encode32Chars[(int)(id >> 30) & 31];
        buffer[7] = _encode32Chars[(int)(id >> 25) & 31];
        buffer[8] = _encode32Chars[(int)(id >> 20) & 31];
        buffer[9] = _encode32Chars[(int)(id >> 15) & 31];
        buffer[10] = _encode32Chars[(int)(id >> 10) & 31];
        buffer[11] = _encode32Chars[(int)(id >> 5) & 31];
        buffer[12] = _encode32Chars[(int)id & 31];

        return new string(buffer, 0, 13);
    }
}
