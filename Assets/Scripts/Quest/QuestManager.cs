using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
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

    [SerializeField]
    NotificationManager notificationManager;

    [SerializeField]
    ArchiveManager archiveManager;

    Dictionary<string, float> questToBeRemoved = new Dictionary<string, float>();


    private void Start()
    {
        questDetails = detailsCanvas.GetComponent<QuestDetails>();
    }

    private void Awake()
    {
        filepath = Application.persistentDataPath + "/save.dat";
        LoadFromFile();
        CheckExpired();
        Load();
    }

    public void AddQuest(QuestData questData)
    {
        questData.Initialize();
        activeQuests.Add(questData);
        Save();
        Reload();

        foreach (QuestDisplayer questDisplayer in questDisplayers)
        {
            questDisplayer.RefreshContainerAfterAddingQuest(questData);
        }
    }

    private void Reload()
    {
        Unload();
        LoadFromFile();
        Load();
        StartRemovalQuestsAfterReload();
    }

    /// <summary>
    /// Prevents from cancelling removal of multiple quests. All quests that are to be removed will try to be removed after the reload
    /// </summary>
    public void StartRemovalQuestsAfterReload()
    {
        if (questToBeRemoved.Count > 0)
        {
            //Make a copy of the list to prevent invalidOperationException (changing the list during enumeration)
            Dictionary<string, float> removalList = new Dictionary<string, float>();
            foreach(KeyValuePair<string,float> pair in questToBeRemoved)
            {
                removalList.Add(pair.Key, pair.Value);
            }
            foreach (KeyValuePair<string, float> pair in removalList)
            {
                //if exist - important, cyclick quests cause exception here without it
                if (activeQuests.Exists(x => x.ID == pair.Key))
                {
                    float time = Time.time - pair.Value;
                    //We remove the quest via first displayer, the removal needs to have source.
                    questDisplayers[0].FindQuestWithID(pair.Key).Remove(time);
                }
            }
            questToBeRemoved.Clear();
        }
    }

    public void OnQuestRemovalStart(string ID)
    {
        if(!questToBeRemoved.ContainsKey(ID))
        {
            questToBeRemoved.Add(ID, Time.time);
        }
    }

    public void OnQuestRemovalStop(string ID)
    {
        questToBeRemoved.Remove(ID);
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
        activeQuests.Remove(questData);
        if (questData.deadline != default && questData.remind && questData.notificationID != default)
        {
            notificationManager.CancelNotification(questData.notificationID);
        }
        foreach (QuestDisplayer questDisplayer in questDisplayers)
        {
            questDisplayer.RemoveQuest(id);
        }
        Save();
        CheckCycle(questData);
    }

    public void FastRemoveQuest(string id)
    {
        QuestData questData = FindQuestWithID(id);
        activeQuests.Remove(questData);
        if(questData.deadline!=default && questData.remind && questData.notificationID != default)
        {
            notificationManager.CancelNotification(questData.notificationID);
        }
        foreach (QuestDisplayer questDisplayer in questDisplayers)
        {
            questDisplayer.RemoveQuest(id);
        }
        Save();
        archiveManager.ArchiveQuest(questData, false);
    }

    public void RemoveEditedQuest(string id)
    {
        QuestData questData = FindQuestWithID(id);
        activeQuests.Remove(questData);
        if (questData.deadline != default && questData.remind && questData.notificationID != default)
        {
            notificationManager.CancelNotification(questData.notificationID);
        }
        foreach (QuestDisplayer questDisplayer in questDisplayers)
        {
            questDisplayer.RemoveQuest(id);
        }
        Save();
    }

    /// <summary>
    /// Called only before load
    /// </summary>
    /// <param name="questData"></param>
    public void SilentRemoveQuest(QuestData questData)
    {
        activeQuests.Remove(questData);
        Save();
        archiveManager.ArchiveQuest(questData, false);
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
            do
            {
                if (questData.repeatCycle == 30)
                {
                    questData.date = questData.date.AddMonths(1);
                }
                else
                {
                    questData.date = questData.date.AddDays(questData.repeatCycle);
                }
            } while (questData.date <= DateTime.Today);

            AddQuest(questData);
        }
        else {
            archiveManager.ArchiveQuest(questData, true);
        }
    }

    public void StartRemovall(string ID)
    {
        foreach(QuestDisplayer questDisplayer in questDisplayers)
        {
            questDisplayer.StartRemovall(ID);
        }
    }

    public void StartRemovall(string ID, float time)
    {
        foreach (QuestDisplayer questDisplayer in questDisplayers)
        {
            questDisplayer.StartRemovall(ID, time);
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

        GiveExp(questData.weight);
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

    void CheckExpired()
    {
        if(activeQuests != null && activeQuests.Count > 0)
        {
            List<QuestData> questsToBeRemoved = new List<QuestData>(); 
            foreach (QuestData questData in activeQuests)
            {
                if (questData.autoRemove && questData.deadline != default && questData.deadline < DateTime.Today)
                {
                    questsToBeRemoved.Add(questData);
                }
            }
            if(questsToBeRemoved.Count > 0)
            {
                foreach (QuestData questData in questsToBeRemoved)
                {
                    SilentRemoveQuest(questData);
                }
            }
        }
    }

    public void AddSubQuest(string ID, SubQuestData subQuest)
    {
        QuestData questData = FindQuestWithID(ID);
        questData.subQuests.Add(subQuest);
        Save();
    }

    public void RemoveSubQuest(string ID, SubQuestData subQuest)
    {
        QuestData questData = FindQuestWithID(ID);
        questData.subQuests.Remove(subQuest);
        Save();
    }

    public void ChangeCompletition(bool completed, string ID, int subQuestID)
    {
        QuestData questData = FindQuestWithID(ID);
        SubQuestData subQuest = questData.subQuests[subQuestID];
        subQuest.completed = completed;
        Save();
    }
}
