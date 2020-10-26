using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Quest : MonoBehaviour
{
    [SerializeField]
    Text nameText;
    [SerializeField]
    Text rewardText;

    string questName;
    string reward;
    int weight;

     public void Initialize(string name, string reward, int weight)
    {
        this.questName = name;
        this.reward = reward;
        this.weight = weight;
    }

    public void Initialize(string name, string reward)
    {
        this.questName = name;
        this.reward = reward;
        this.weight = 1;
    }

    public void Initialize(string name)
    {
        this.questName = name;
        this.reward = null;
        this.weight = 1;
    }
            

    void Start()
    {
        nameText.text = questName;
        rewardText.text = reward;
    }
}
