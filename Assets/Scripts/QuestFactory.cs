using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestFactory : MonoBehaviour
{
    [SerializeField]
    GameObject questObject;

    void AddQuest(string questName)
    {
        GameObject ob = Instantiate(questObject);
        ob.GetComponent<Quest>().Initialize(questName);
        ob.transform.parent = this.transform;
    }

    void AddQuest(string questName, string questReward)
    {
        GameObject ob = Instantiate(questObject);
        ob.GetComponent<Quest>().Initialize(questName, questReward);
        ob.transform.parent = this.transform;
    }

    void AddQuest(string questName, string questReward, int questWeight)
    {
        GameObject ob = Instantiate(questObject);
        ob.GetComponent<Quest>().Initialize(questName, questReward, questWeight);
        ob.transform.parent = this.transform;
    }
}
