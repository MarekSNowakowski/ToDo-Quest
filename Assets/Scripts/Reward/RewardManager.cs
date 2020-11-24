using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class RewardManager : MonoBehaviour
{
    [SerializeField]
    RewardFactory rewardFactory;

    [SerializeField]
    RewardContainer container;

    //QuestDetails questDetails;
    //[SerializeField]
    //GameObject detailsCanvas;

    List<Reward> activeRewards = new List<Reward>();

    private void Start()
    {
        //questDetails = detailsCanvas.GetComponent<QuestDetails>();
    }

    private void Awake()
    {
        Load();
    }

    public void AddReward(QuestData questData)
    {
        Reward reward = rewardFactory.AddQuest(questData);
        activeRewards.Add(reward);
        Save();
        Unload();
        Load();
        container.RefreshSize();
    }

    public void Save()
    {
        List<RewardData> data = new List<RewardData>();

        foreach (Reward reward in activeRewards)
        {
            RewardData rewardData = reward.Save();
            data.Add(rewardData);
        }

        string filepath = Application.persistentDataPath + "/saveR.dat";

        using (FileStream file = File.Create(filepath))
        {
            new BinaryFormatter().Serialize(file, data);
        }
    }

    public void Load()
    {
        string filepath = Application.persistentDataPath + "/saveR.dat";
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
        Unload();
        StartCoroutine(r_waitFrame());
        Load();
        container.RefreshSize();
    }

    IEnumerator r_waitFrame()
    {
        yield return new WaitForEndOfFrame();
    }

    //public void ShowQuestDetails(QuestData questData)
    //{
    //    detailsCanvas.SetActive(true);
    //    questDetails.ShowQuestDetails(questData);
    //}

    public Reward FindRewardWithID(string ID)
    {
        return activeRewards.Find(x => x.id == ID);
    }
}
