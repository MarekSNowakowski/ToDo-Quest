using UnityEngine;
using TMPro;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

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

    string filepath;

    Settings settings;

    private void Start()
    {
        filepath = Application.persistentDataPath + "/settings.dat";
        Load();
        weight1InputField.text = settings.questCompleteExp[0].ToString();
        weight2InputField.text = settings.questCompleteExp[1].ToString();
        weight3InputField.text = settings.questCompleteExp[2].ToString();
        weight4InputField.text = settings.questCompleteExp[3].ToString();
        deadlineHourInputField.text = settings.deadlineTimeHours.ToString("d2");
        deadlineMinutesInputField.text = settings.deadlineTimeMinutes.ToString("d2");
    }

    public int GetNotificationHour()
    {
        return settings.deadlineTimeHours;
    }

    public int GetNotificationMinutes()
    {
        return settings.deadlineTimeMinutes;
    }

    private void Load()
    {
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
            Save();
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
}

[Serializable]
public class Settings
{
    public int[] questCompleteExp;

    public int deadlineTimeHours;
    public int deadlineTimeMinutes;

    public Settings()
    {
        questCompleteExp = new int[] { 1, 2, 5, 10 };
        deadlineTimeHours = 10;
        deadlineTimeMinutes = 0;
    }
}
