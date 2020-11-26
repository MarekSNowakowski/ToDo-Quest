using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelManager : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI levelText;
    [SerializeField]
    TextMeshProUGUI experienceText;
    [SerializeField]
    TMP_InputField levelRewardTextField;
    [SerializeField]
    RewardManager rewardManager;
    [SerializeField]
    Slider levelSlider;

    string rewardText;
    string rewardPlaceholder = "Add your reward!";
    int currentLevel;
    int currentExp;
    int expToNextLevel;
    string filepath;

    private void Awake()
    {
        filepath = Application.persistentDataPath + "/saveL.dat";
        Load();
    }

    public void addExperience(int amount)
    {
        currentExp += amount;
        if(currentExp > expToNextLevel)
        {
            int difference = expToNextLevel - currentExp;
            LevelUp();
            addExperience(difference);
        }
        levelSlider.value = (float)currentExp / (float)expToNextLevel;
    }

    void LevelUp()
    {
        if(rewardText!=null)
        {
            QuestData questData = new QuestData();
            questData.reward = rewardText;
            questData.questName = "Level " + currentLevel;
            rewardManager.AddReward(questData);
        }
        currentLevel++;
        levelText.text = currentLevel.ToString();
        Save();
    }

    void Save()
    {
        LevelData data;

        if(rewardText != null)
        {
            data = new LevelData(currentLevel, rewardText, currentExp, expToNextLevel);
        }else
        {
            data = new LevelData(currentLevel, currentExp, expToNextLevel);
        }
    
        using (FileStream file = File.Create(filepath))
        {
            new BinaryFormatter().Serialize(file, data);
        }
    }

    void Load()
    {
        LevelData data;

        if (File.Exists(filepath))
        {
            using (FileStream file = File.Open(filepath, FileMode.Open))
            {
                object loadedData = new BinaryFormatter().Deserialize(file);
                data = (LevelData)loadedData;
            }
            currentLevel = data.level;
            rewardText = data.reward;
            currentExp = data.currentExp;
            expToNextLevel = data.expToNextLevel;
        }
        else
        {
            rewardText = null;
            currentLevel = 1;
            currentExp = 0;
            expToNextLevel = 50;
            Save();
        }

        SetUp();
    }

    public void OnRewardEditEnd()
    {
        if(levelRewardTextField.text == "" || levelRewardTextField.text == rewardPlaceholder)
        {
            levelRewardTextField.text = rewardPlaceholder;
            rewardText = null;
        }else
        {
            rewardText = levelRewardTextField.text;
        }
        Save();
        StartCoroutine(OnInputEndCOR());
    }

    IEnumerator OnInputEndCOR()
    {
        //Disable interactable then remove event
        levelRewardTextField.DeactivateInputField();
        levelRewardTextField.ReleaseSelection();

        /*Wait until EventSystem is no longer in selecting mode
         This prevents the "Attempting to select while already selecting an object" error
         */
        while (EventSystem.current.alreadySelecting)
            yield return null;

        levelRewardTextField.interactable = false;
    }

    public void EditReward()
    {
        levelRewardTextField.interactable = true;
        levelRewardTextField.ActivateInputField();
        if(!levelRewardTextField.isFocused)
        levelRewardTextField.Select();
        if(rewardText == null || levelRewardTextField.text == rewardPlaceholder)
        {
            levelRewardTextField.text = "";
        }
    }

    void SetUp()
    {
        levelText.text = currentLevel.ToString();
        experienceText.text = $"{currentExp} / {expToNextLevel} exp";
        if(rewardText != null)
        {
            levelRewardTextField.text = rewardText;
        }
        levelSlider.value = (float)currentExp / (float)expToNextLevel;
    }
}

[System.Serializable] 
public struct LevelData
{
    public int level;
    public string reward;
    public int currentExp;
    public int expToNextLevel;

    public LevelData(int level, string reward, int currentExp, int expToNextLevel)
    {
        this.level = level;
        this.reward = reward;
        this.currentExp = currentExp;
        this.expToNextLevel = expToNextLevel;
    }

    public LevelData(int level, int currentExp, int expToNextLevel)
    {
        this.level = level;
        this.reward = null;
        this.currentExp = currentExp;
        this.expToNextLevel = expToNextLevel;
    }
}