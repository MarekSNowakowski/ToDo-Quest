using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    SettingsManager settingsManager;

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
        bool addingCategoryLabel = false;
        bool addingDateLabel = false;

        questData.Initialize();
        activeQuests.Add(questData);
        Save();
        Unload();
        LoadFromFile();
        Load();

        foreach (QuestDisplayer questDisplayer in questDisplayers)
        {
            questDisplayer.RefreshContainerAfterAddingQuest(questData);
        }
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
        } else
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
        foreach (QuestDisplayer questDisplayer in questDisplayers)
        {
            questDisplayer.Unload();
        }
    }

    public void ShowCategoryQuests(QuestDisplayer questDisplayer, Category category)
    {
        detailsCanvas.SetActive(true);
        List<QuestData> categoryQuestDataList = FindQuestsWithCategory(category);
        questDisplayer.ShowCategoryQuests(this, categoryQuestDataList);
    }

    public void RemoveQuest(string id)
    {
        QuestData questData = FindQuestWithID(id);
        GiveReward(questData);
        CheckCycle(questData);
        activeQuests.Remove(questData);
        foreach (QuestDisplayer questDisplayer in questDisplayers)
        {
            questDisplayer.RemoveQuest(id);
        }
        Save();
    }

    public void FastRemoveQuest(string id)
    {
        QuestData questData = FindQuestWithID(id);
        activeQuests.Remove(questData);
        foreach (QuestDisplayer questDisplayer in questDisplayers)
        {
            questDisplayer.RemoveQuest(id);
        }
        Save();
    }

    internal void ChangeLabel(Category editingCategory)
    {
        foreach(QuestDisplayer questDisplayer in questDisplayers)
        {
            questDisplayer.TryChangeLabel(editingCategory);
        }
    }

    void CheckCycle(QuestData questData)
    {
        if(questData.repeatCycle != 0)
        {
            if (questData.repeatCycle == 30)
            {
                questData.date = questData.date.AddMonths(1);
            }
            else
            {
                questData.date = questData.date.AddDays(questData.repeatCycle);
            }

            AddQuest(questData);
        }
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
                GiveExp(questData.weight);
            }
        }
        else
        {
            GiveExp(questData.weight);
        }
    }

    void GiveExp(int weight)
    {
        levelManager.AddExperience(settingsManager.GetQuestCompleteExp()[weight]);
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

    public void RemoveCategory(Category category)
    {
        List<QuestData> categoryQuests = FindQuestsWithCategory(category);
        Debug.Log(categoryQuests.Count);
        foreach(QuestData questData in categoryQuests)
        {
            FastRemoveQuest(questData.ID);
            questData.RemoveCategory();
            AddQuest(questData);
        }
        Save();
        Unload();
        Load();
        SetCategoryButton();
    }

    public List<QuestData> FindQuestsWithCategory(Category category)
    {
        return activeQuests.FindAll(x => x.category!=null && x.category.GetID() == category.GetID());
    }

    public void ReloadQuests()
    {
        Unload();
        Load();
        SetCategoryButton();
    }

    void SetCategoryButton()
    {
        foreach (QuestDisplayer questDisplayer in questDisplayers)
        {
            questDisplayer.SetCategoryButton();
        }
    }
}
