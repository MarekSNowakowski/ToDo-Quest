using UnityEngine;
using TMPro;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine.SceneManagement;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    TMP_InputField weight1InputField;
    [SerializeField]
    TMP_InputField weight2InputField;
    [SerializeField]
    TMP_InputField weight3InputField;
    [SerializeField]
    TMP_InputField weight4InputField;

    [SerializeField]
    TMP_InputField deadlineHourInputField;
    [SerializeField]
    TMP_InputField deadlineMinutesInputField;

    [SerializeField]
    TMP_Dropdown languageDropdown;

    [SerializeField]
    TMP_InputField archiveSizeInputField;
    [SerializeField]
    ArchiveManager archiveManager;

    string filepath;

    Settings settings;

    private void Awake()
    {
        filepath = Application.persistentDataPath + "/settings.dat";
        Load();
        Set();
        languageDropdown.onValueChanged.AddListener(
            delegate
            {
                OnLanguageChange(languageDropdown.value);
            });
    }

    private void Set()
    {
        weight1InputField.text = settings.questCompleteExp[0].ToString();
        weight2InputField.text = settings.questCompleteExp[1].ToString();
        weight3InputField.text = settings.questCompleteExp[2].ToString();
        weight4InputField.text = settings.questCompleteExp[3].ToString();
        deadlineHourInputField.text = settings.deadlineTimeHours.ToString("d2");
        deadlineMinutesInputField.text = settings.deadlineTimeMinutes.ToString("d2");
        languageDropdown.value = GetDropdownValue();
        archiveSizeInputField.text = settings.archiveMaxSize.ToString();
    }

    public int GetDropdownValue()
    {
        switch (settings.language)
        {
            case "pl":
                return 1;
            default:
                return 0;
        }
    }

    public int GetNotificationHour()
    {
        return settings.deadlineTimeHours;
    }

    public int GetNotificationMinutes()
    {
        return settings.deadlineTimeMinutes;
    }

    public int GetArchiveMaxSize()
    {
        return settings.archiveMaxSize;
    }

    private void Load()
    {
        if(filepath==null) filepath = Application.persistentDataPath + "/settings.dat";
        if (File.Exists(filepath))
        {
            using (FileStream file = File.Open(filepath, FileMode.Open))
            {
                object loadedData = new BinaryFormatter().Deserialize(file);
                settings = (Settings)loadedData;
            }
        }
        else
        {
            settings = new Settings();
            //Default language selection
            SetDefaultLanguage();
            Save();
        }
    }

    private void SetDefaultLanguage()
    {
        switch (Application.systemLanguage)
        {
            case SystemLanguage.Polish:
                settings.language = "pl";
                languageDropdown.value = 1;
                break;
            default:
                settings.language = "en";
                languageDropdown.value = 0;
                break;
        }
    }

    private void Save()
    {
        using (FileStream file = File.Create(filepath))
        {
            new BinaryFormatter().Serialize(file, settings);
        }
    }

    public int[] GetQuestCompleteExp()
    {
        return settings.questCompleteExp;
    }

    public void OnBaseExpEditingEnd()
    {
        //Check if data is correct, if not change it to last value
        if (weight1InputField.text == "" || !int.TryParse(weight1InputField.text, out settings.questCompleteExp[0]))
        {
            weight1InputField.text = settings.questCompleteExp[0].ToString();
        }
        if (weight2InputField.text == "" || !int.TryParse(weight2InputField.text, out settings.questCompleteExp[1]))
        {
            weight2InputField.text = settings.questCompleteExp[1].ToString();
        }
        if (weight3InputField.text == "" || !int.TryParse(weight3InputField.text, out settings.questCompleteExp[2]))
        {
            weight3InputField.text = settings.questCompleteExp[2].ToString();
        }
        if (weight4InputField.text == "" || !int.TryParse(weight4InputField.text, out settings.questCompleteExp[3]))
        {
            weight4InputField.text = settings.questCompleteExp[3].ToString();
        }

        //Then save
        Save();
    }

    public void OnDeadlineHourChange()
    {
        int hour;
        if(deadlineHourInputField.text == "" || !int.TryParse(deadlineHourInputField.text, out hour) || hour > 23)
        {
            deadlineHourInputField.text = settings.deadlineTimeHours.ToString("d2");
        }
        else
        {
            settings.deadlineTimeHours = hour;
            deadlineHourInputField.text = settings.deadlineTimeHours.ToString("d2");
            Save();
        }

        //Select minutes field after editing hours
        deadlineMinutesInputField.ActivateInputField();
        deadlineMinutesInputField.Select();
    }

    public void OnDeadlineMinuteChange()
    {
        int minutes;
        if(deadlineMinutesInputField.text == "" || !int.TryParse(deadlineMinutesInputField.text, out minutes) || minutes > 59)
        {
            deadlineMinutesInputField.text = settings.deadlineTimeMinutes.ToString("d2");
        }
        else
        {
            settings.deadlineTimeMinutes = minutes;
            deadlineMinutesInputField.text = settings.deadlineTimeMinutes.ToString("d2");
            Save();
        }
    }

    public void SetLanguage(string language)
    {
        if(language=="en"||language=="pl")
        {
            settings.language = language;
        }
        else
        {
            settings.language = "en";
        }
        Save();
    }

    public string GetLanguage()
    {
        if (settings == null) 
        {
            Load();
        }
        return settings.language;
    }

    public void OnLanguageChange(int number)
    {
        switch(number)
        {
            case 0:
                settings.language = "en";
                Save();
                ReloadScene();
                break;
            case 1:
                settings.language = "pl";
                Save();
                ReloadScene();
                break;
        }
    }

    public void ReloadScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.UnloadSceneAsync(currentScene.name);
        SceneManager.LoadScene(currentScene.name);
    }

    public bool IsFirstRun()
    {
        return settings.firstRun;
    }

    public void SetFirstRun()
    {
        settings.firstRun = false;
        Save();
    }

    public void OnRestoreDefaultSettingsButtonPress()
    {
        settings = new Settings();
        SetDefaultLanguage();
        SetFirstRun();
        Set();
    }

    public void OnArchiveMaxSizeEditEnd()
    {
        if (archiveSizeInputField.text == "" || !int.TryParse(archiveSizeInputField.text, out settings.archiveMaxSize))
        {
            archiveSizeInputField.text = settings.archiveMaxSize.ToString();
        }
        else
        {
            Save();
        }
        archiveManager.CheckIfFull();
    }
}

[Serializable]
public class Settings
{
    public int[] questCompleteExp;

    public int deadlineTimeHours;
    public int deadlineTimeMinutes;
    public int archiveMaxSize;

    public string language;

    public bool firstRun;

    public Settings()
    {
        questCompleteExp = new int[] { 1, 2, 5, 10 };
        deadlineTimeHours = 10;
        deadlineTimeMinutes = 0;
        archiveMaxSize = 100;
        firstRun = true;
    }
}
