﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    QuestDetails questDetails;
    [SerializeField]
    GameObject detailsCanvas;

    [SerializeField]
    RewardManager rewardManager;

    [SerializeField]
    LevelManager levelManager;

    [SerializeField]
    List<QuestDisplayer> questDisplayers;

    List<QuestData> activeQuests = new List<QuestData>();

    string filepath;

    private void Start()
    {
        questDetails = detailsCanvas.GetComponent<QuestDetails>();
    }

    private void Awake()
    {
        filepath = Application.persistentDataPath + "/save.dat";
        LoadFromFile();
        Load();
    }

    public void AddQuest(QuestData questData)
    {
        questData.Initialize();
        foreach(QuestDisplayer questDisplayer in questDisplayers)
        {
            questDisplayer.AddQuest(this, questData);
        }
        activeQuests.Add(questData);
        Save();
    }


    public void Save()
    {
        using (FileStream file = File.Create(filepath))
        {
            new BinaryFormatter().Serialize(file, activeQuests);
        }
    }

    public void LoadFromFile()
    {
        if (File.Exists(filepath))
        {
            using (FileStream file = File.Open(filepath, FileMode.Open))
            {
                object loadedData = new BinaryFormatter().Deserialize(file);
                activeQuests = (List<QuestData>)loadedData;
            }
            activeQuests.Sort();
        }else
        {
            Save();
        }
    }

    public void Load()
    {
        foreach (QuestDisplayer questDisplayer in questDisplayers)
        {
            questDisplayer.Load(this, activeQuests);
        }
    }

    public void Unload()
    {
        transform.DetachChildren();
        foreach (QuestDisplayer questDisplayer in questDisplayers)
        {
            questDisplayer.Unload();
        }
    }

    public void RemoveQuest(string id)
    {
        QuestData questData = FindQuestWithID(id);
        GiveReward(questData);
        activeQuests.Remove(questData);
        foreach (QuestDisplayer questDisplayer in questDisplayers)
        {
            questDisplayer.RemoveQuest(id);
        }
        Save();
        //Unload();
        //StartCoroutine(r_waitFrame());
        //Load();
    }

    public void StartRemovall(QuestData questData)
    {
        foreach(QuestDisplayer questDisplayer in questDisplayers)
        {
            questDisplayer.StartRemovall(questData.ID);
        }
    }

    public void GiveReward(QuestData questData)
    {
        if (questData.reward != null)
        {
            if (questData.reward.StartsWith("+"))
            {
                string rewardPoints = questData.reward.Remove(0, 1);
                int points;

                if (int.TryParse(rewardPoints, out points))
                {
                    if (points > 0 && points < 1000)
                        levelManager.AddExperience(points);
                }
                else
                {
                    rewardManager.AddReward(questData);
                }
            }
            else
            {
                rewardManager.AddReward(questData);
            }
        }
    }

    IEnumerator r_waitFrame()
    {
        yield return new WaitForEndOfFrame();
    }

    public void ShowQuestDetails(QuestData questData)
    {
        detailsCanvas.SetActive(true);
        questDetails.ShowQuestDetails(questData);
    }

    public QuestData FindQuestWithID(string ID)
    {
        return activeQuests.Find(x => x.ID == ID);
    }

    public void CancellRemoval(string id)
    {
        foreach(QuestDisplayer questDisplayer in questDisplayers)
        {
            questDisplayer.CancellRemoval(id);
        }
    }
}
