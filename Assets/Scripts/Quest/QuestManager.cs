using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    QuestFactory questFactory;

    [SerializeField]
    QuestContainer container;

    QuestDetails questDetails;
    [SerializeField]
    GameObject detailsCanvas;

    [SerializeField]
    RewardManager rewardManager;

    [SerializeField]
    LevelManager levelManager;

    List<Quest> activeQuests = new List<Quest>();

    string filepath;

    private void Start()
    {
        questDetails = detailsCanvas.GetComponent<QuestDetails>();
    }

    private void Awake()
    {
        filepath = Application.persistentDataPath + "/save.dat";
        Load();
    }

    public void AddQuest(QuestData questData)
    {
        Quest quest = questFactory.AddQuest(questData);
        quest.Initialize(questData);
        SetUpAfterAddingQuest(quest);
    }

    void SetUpAfterAddingQuest(Quest quest)
    {
        activeQuests.Add(quest);
        Save();
        Unload();
        Load();
        container.RefreshSize(true);
    }

    public void Save()
    {
        List<QuestData> data = new List<QuestData>();

        activeQuests.Sort();
        foreach (Quest quest in activeQuests)
        {
            QuestData questData = quest.Save();
            data.Add(questData);
        }

        using (FileStream file = File.Create(filepath))
        {
            new BinaryFormatter().Serialize(file, data);
        }
    }

    public void Load()
    {
        if (File.Exists(filepath))
        {
            List<QuestData> data = new List<QuestData>();

            using (FileStream file = File.Open(filepath, FileMode.Open))
            {
                object loadedData = new BinaryFormatter().Deserialize(file);
                data = (List<QuestData>)loadedData;
            }
            data.Sort();
            foreach (QuestData questData in data)
            {
                Quest quest = questFactory.LoadQuest(questData);
                activeQuests.Add(quest);
                quest.GetManager(this);
            }
        }else
        {
            Save();
        }
    }

    public void Unload()
    {
        transform.DetachChildren();
        foreach (Quest quest in activeQuests)
        {
            Destroy(quest.gameObject);
        }
        activeQuests.Clear();
    }

    public void RemoveQuest(Quest quest)
    {
        QuestData questData = quest.Save();
        if(questData.reward!=null)
        {
            if(questData.reward.StartsWith("+"))
            {
                string rewardPoints = questData.reward.Remove(0,1);
                int points;

                if(int.TryParse(rewardPoints, out points))
                {
                    if(points > 0 && points < 1000)
                    levelManager.AddExperience(points);
                }else
                {
                    rewardManager.AddReward(questData);
                }
            }else
            {
                rewardManager.AddReward(questData);
            }
        }
        activeQuests.Remove(quest);
        Destroy(quest.gameObject);
        Save();
        Unload();
        StartCoroutine(r_waitFrame());
        Load();
        container.RefreshSize(false);
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

    public Quest FindQuestWithID(string ID)
    {
        return activeQuests.Find(x => x.ID == ID);
    }
}
