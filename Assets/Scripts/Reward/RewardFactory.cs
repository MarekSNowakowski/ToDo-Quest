using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewardFactory : MonoBehaviour
{
    [SerializeField]
    GameObject rewardObject;
    [SerializeField]
    TranslationManager translationManager;

    public Reward LoadReward(RewardData rewardData)
    {
        GameObject ob = Instantiate(rewardObject);
        ob.transform.SetParent(transform);
        Reward reward = ob.GetComponent<Reward>();
        reward.Load(rewardData, translationManager.GetStaticString(24), translationManager.GetStaticString(76));

        return reward;
    }

    public Reward AddReward()
    {
        GameObject ob = Instantiate(rewardObject);
        Reward reward = ob.GetComponent<Reward>();

        return reward;
    }
}
