using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    [SerializeField]
    QuestFactory questFactory;

    List<Quest> activeQuests = new List<Quest>();
    

    private void Awake()
    {
        Load();
        //Test();
    }

    public void Test()
    {
        QuestData questData = new QuestData();
        questData.questName = "TEST NAME2";
        questData.reward = "TEST REWARD2";
        AddQuest(questData);
    }

    public void AddQuest(QuestData questData)
    {
        Quest quest = questFactory.AddQuest(questData);
        activeQuests.Add(quest);
        Save();
        Load();
    }

    public void Save()
    {
        List<QuestData> data = new List<QuestData>();

        foreach(Quest quest in activeQuests)
        {
            QuestData questData = quest.Save();
            data.Add(questData);
        }

        if(data.Count!=0)
        {
            string filepath = Application.persistentDataPath + "/save.dat";

            using (FileStream file = File.Create(filepath))
            {
                new BinaryFormatter().Serialize(file, data);
            }
        }
    }

    public void Load()
    {
        string filepath = Application.persistentDataPath + "/save.dat";
        List<QuestData> data = new List<QuestData>();

        using (FileStream file = File.Open(filepath, FileMode.Open))
        {
            object loadedData = new BinaryFormatter().Deserialize(file);
            data = (List<QuestData>)loadedData;
        }

        foreach(QuestData questData in data)
        {
            Quest quest = questFactory.LoadQuest(questData);
            activeQuests.Add(quest);
        }
    }
}
