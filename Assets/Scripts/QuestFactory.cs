using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestFactory : MonoBehaviour
{
    [SerializeField]
    GameObject questObject;

    public Quest LoadQuest(QuestData questData)
    {
        GameObject ob = Instantiate(questObject);
        ob.transform.SetParent(transform);
        Quest quest = ob.GetComponent<Quest>();
        quest.Load(questData);

        return quest;
    }

    public Quest AddQuest(QuestData questData)
    {
        GameObject ob = Instantiate(questObject);
        Quest quest = ob.GetComponent<Quest>();
        quest.Initialize(questData);

        return quest;
    }

}
