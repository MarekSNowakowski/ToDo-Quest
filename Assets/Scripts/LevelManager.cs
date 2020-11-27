﻿using System.Collections;
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
    TextMeshProUGUI currentExpText;
    [SerializeField]
    TMP_InputField expToNextLevelTextField;
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
    int defaultExpToNextLevel = 100;

    private void Awake()
    {
        filepath = Application.persistentDataPath + "/saveL.dat";
        Load();
    }

    public void AddExperience(int amount)
    {
        currentExp += amount;
        if(currentExp >= expToNextLevel)
        {
            int difference = currentExp - expToNextLevel;
            currentExp = 0;
            LevelUp();
            if(difference>0)
                AddExperience(difference);
        }

        Save();
        SetUp();
    }

    void LevelUp()
    {
        if(rewardText!=null)
        {
            QuestData questData = new QuestData();
            questData.reward = rewardText;
            questData.questName = "Level " + currentLevel;
            rewardManager.AddReward(questData);
            rewardText = null;
            levelRewardTextField.text = rewardPlaceholder;
        }
        expToNextLevel += (int)(expToNextLevel * (float)(currentLevel / defaultExpToNextLevel));  // highering the bar
        currentLevel++;
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
            expToNextLevel = defaultExpToNextLevel;
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
        StartCoroutine(OnInputEndCOR(levelRewardTextField));
    }

    IEnumerator OnInputEndCOR(TMP_InputField inputField)
    {
        //Disable interactable then remove event
        inputField.DeactivateInputField();
        inputField.ReleaseSelection();

        /*Wait until EventSystem is no longer in selecting mode
         This prevents the "Attempting to select while already selecting an object" error
         */
        while (EventSystem.current.alreadySelecting)
            yield return null;

        inputField.interactable = false;
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
        currentExpText.text = $"{currentExp} / ";
        expToNextLevelTextField.text = expToNextLevel.ToString();
        if(rewardText != null)
        {
            levelRewardTextField.text = rewardText;
        }
        levelSlider.value = (float)currentExp / (float)expToNextLevel;
    }

    public void EditExpToNextLevel()
    {
        expToNextLevelTextField.interactable = true;
        expToNextLevelTextField.ActivateInputField();
        if (!expToNextLevelTextField.isFocused)
            expToNextLevelTextField.Select();
    }

    public void OnEditExpToNextLevelEnd()
    {
        int temp = expToNextLevel;
        if (expToNextLevelTextField.text == "" || !int.TryParse(expToNextLevelTextField.text, out expToNextLevel))
        {
            expToNextLevel = temp;
            expToNextLevelTextField.text = expToNextLevel.ToString();
        }
        else if(expToNextLevel <= currentExp)
        {
            expToNextLevel = currentExp + 1;
            expToNextLevelTextField.text = expToNextLevel.ToString();
        }
        Save();
        levelSlider.value = (float)currentExp / (float)expToNextLevel;
        StartCoroutine(OnInputEndCOR(expToNextLevelTextField));
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