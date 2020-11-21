using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
    [SerializeField]
    TMPro.TextMeshProUGUI nameText;
    [SerializeField]
    GameObject rewardImage;
    [SerializeField]
    GameObject commentImage;

    QuestManager questManager;

    [Header("Quest Data")]
    string ID;
    string questName;
    string reward;
    int weight;
    string comment;

    public void Initialize(QuestData questData)
    {
        this.ID = CorrelationIdGenerator.GetNextId();
        questData.ID = this.ID;
        Load(questData);
    }

    void setUpText()
    {
        nameText.text = questName;
        if (reward != "" && reward != null) rewardImage.gameObject.SetActive(true);
        if (comment != "" && comment != null) commentImage.gameObject.SetActive(true);
    }

    public QuestData Save()
    {
        QuestData saveData = new QuestData(ID, questName, reward, weight, comment);
        return saveData;
    }

    public void Load(QuestData questData)
    {
        this.ID = questData.ID;
        this.questName = questData.questName;
        this.reward = questData.reward;
        this.weight = questData.weight;
        this.comment = questData.comment;
        setUpText();
    }

    public void GetManager(QuestManager questManager)
    {
        this.questManager = questManager;
    }

    public void RemoveSelf()
    {
        questManager.RemoveQuest(this);
    }
}

[System.Serializable]
public struct QuestData
{
    public string ID;
    public string questName;
    public string reward;
    public int weight;
    public string comment;

    public QuestData(string ID, string questName, string reward, int weight, string comment)
    {
        this.ID = ID;
        this.questName = questName;
        this.reward = reward;
        this.weight = weight;
        this.comment = comment;
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
