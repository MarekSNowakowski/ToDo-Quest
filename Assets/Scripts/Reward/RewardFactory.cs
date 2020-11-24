using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardFactory : MonoBehaviour
{
    [SerializeField]
    GameObject rewardObject;

    public Reward LoadReward(RewardData rewardData)
    {
        GameObject ob = Instantiate(rewardObject);
        ob.transform.SetParent(transform);
        Reward reward = ob.GetComponent<Reward>();
        reward.Load(rewardData);

        return reward;
    }

    public Reward AddQuest(QuestData questData)
    {
        GameObject ob = Instantiate(rewardObject);
        Reward reward = ob.GetComponent<Reward>();
        reward.Initialize(questData);

        return reward;
    }
}
