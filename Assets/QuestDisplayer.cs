using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class QuestDisplayer : MonoBehaviour
{
    [SerializeField]
    QuestFactory questFactory;

    [SerializeField]
    QuestContainer container;

    List<Quest> activeQuests = new List<Quest>();

    public void AddQuest(QuestManager questManager, QuestData questData)
    {
        Quest quest = questFactory.AddQuest(questData);
        quest.GetManager(questManager);
        quest.Load(questData);
        SetUpAfterAddingQuest(quest);
    }

    void SetUpAfterAddingQuest(Quest quest)
    {
        activeQuests.Add(quest);
        container.RefreshSize(true);
    }

    public void Load(QuestManager questManager, List<QuestData> data)
    {
            foreach (QuestData questData in data)
            {
                Quest quest = questFactory.LoadQuest(questData);
                activeQuests.Add(quest);
                quest.GetManager(questManager);
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

    public void RemoveQuest(string id)
    {
        Quest quest = FindQuestWithID(id);
        activeQuests.Remove(quest);
        Destroy(quest.gameObject);
        container.RefreshSize(false);
    }

    IEnumerator r_waitFrame()
    {
        yield return new WaitForEndOfFrame();
    }

    public Quest FindQuestWithID(string ID)
    {
        return activeQuests.Find(x => x.ID == ID);
    }
}
