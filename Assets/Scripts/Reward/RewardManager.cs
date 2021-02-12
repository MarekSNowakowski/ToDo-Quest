using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [SerializeField]
    RewardFactory rewardFactory;

    [SerializeField]
    RewardContainer container;

    RewardDetails rewardDetails;
    [SerializeField]
    GameObject detailsCanvas;

    [SerializeField]
    TranslationManager translationManager;

    List<Reward> activeRewards = new List<Reward>();

    string filepath;

    private void Start()
    {
        rewardDetails = detailsCanvas.GetComponent<RewardDetails>();
    }

    private void Awake()
    {
        filepath = Application.persistentDataPath + "/saveR.dat";
        if(File.Exists(filepath))
        Load();
    }

    public void AddReward(QuestData questData)
    {
        int amount = 1;
        string name;
        //Check if the amount of rewards is to be changed
        if(questData.reward.Length > 3 && (questData.reward[1] == 'x' || questData.reward[1] == 'X') && char.IsDigit(questData.reward[0]) && questData.reward[2] == ' ')
        {
            amount = (int) char.GetNumericValue(questData.reward[0]);
            name = questData.reward.Substring(3);
        }
        else
        {
            name = questData.reward;
        }
        //Check if we can add amount to existing reward to prevent from duplicating rewards with the same name
        if (activeRewards.Exists(x => x.GetName() == name))
        {
            //Increase amount
            Reward reward = activeRewards.Find(x => x.GetName() == name);
            reward.IncreaseAmount(amount);
            reward.SetUp(translationManager.GetStaticString(24), translationManager.GetStaticString(25));
            Save();
            Unload();
            Load();
        }
        else
        {
            Reward reward = rewardFactory.AddReward();
            if(amount > 1)
            {
                reward.Initialize(questData, name, amount, translationManager.GetStaticString(24));
            }
            else
            {
                reward.Initialize(questData, translationManager.GetStaticString(24));
            }

            SetUpAfterAddingReward(reward);
        }
    }

    void SetUpAfterAddingReward(Reward reward)
    {
        activeRewards.Add(reward);
        Save();
        Unload();
        Load();
        container.RefreshSize(true);
    }

    public void Save()
    {
        List<RewardData> data = new List<RewardData>();

        activeRewards.Sort();
        foreach (Reward reward in activeRewards)
        {
            RewardData rewardData = reward.Save();
            data.Add(rewardData);
        }

        using (FileStream file = File.Create(filepath))
        {
            new BinaryFormatter().Serialize(file, data);
        }
    }

    public void Load()
    {
        if(File.Exists(filepath))
        {
            List<RewardData> data = new List<RewardData>();

            using (FileStream file = File.Open(filepath, FileMode.Open))
            {
                object loadedData = new BinaryFormatter().Deserialize(file);
                data = (List<RewardData>)loadedData;
            }
            data.Sort();
            foreach (RewardData rewardData in data)
            {
                Reward reward = rewardFactory.LoadReward(rewardData);
                activeRewards.Add(reward);
                reward.GetManager(this);
            }
        }else
        {
            Save();
        }
    }

    public void Unload()
    {
        transform.DetachChildren();
        foreach (Reward reward in activeRewards)
        {
            Destroy(reward.gameObject);
        }
        activeRewards.Clear();
    }

    public void RemoveReward(Reward reward)
    {
        activeRewards.Remove(reward);
        Destroy(reward.gameObject);
        Save();
        StartCoroutine(r_waitFrame());
        container.RefreshSize(false);
    }

    IEnumerator r_waitFrame()
    {
        yield return new WaitForEndOfFrame();
    }

    public void ShowRewardDetails(RewardData rewardData)
    {
        detailsCanvas.SetActive(true);
        rewardDetails.ShowRewardDetails(rewardData);
    }

    public Reward FindRewardWithID(string ID)
    {
        return activeRewards.Find(x => x.id == ID);
    }
}
