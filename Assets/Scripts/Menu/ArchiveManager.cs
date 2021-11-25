using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ArchiveManager : MonoBehaviour
{
    readonly SaveManager saveManager = new SaveManager("archive");

    private string filepath;

    private List<ArchivedQuestData> archivedQuests;
    private List<ArchivedQuest> archivedQuestsObjects;
    [SerializeField]
    GameObject archivedQuestPrefab;
    [SerializeField]
    SettingsManager settings;
    [SerializeField]
    TranslationManager translation;
    [SerializeField]
    ArchiveDetails archiveDetails;

    private void Awake()
    {
        filepath = saveManager.FilePath;
        if (archivedQuests == null) LoadFromFile();
        if (archivedQuestsObjects == null) Load();
    }

    public void Save()
    {
        saveManager.SaveData(archivedQuests);
    }

    public void LoadFromFile()
    {
        archivedQuests = new List<ArchivedQuestData>();
        if (File.Exists(filepath))
        {
            using (FileStream file = File.Open(filepath, FileMode.Open))
            {
                if(file.Length!=0)
                {
                    object loadedData = new BinaryFormatter().Deserialize(file);
                    archivedQuests = (List<ArchivedQuestData>)loadedData;
                }
            }
            //archivedQuests.Sort();
        }
        else
        {
            Save();
        }
    }

    public void Load()
    {
        archivedQuestsObjects = new List<ArchivedQuest>();
        foreach (ArchivedQuestData archivedQuestData in archivedQuests)
        {
            LoadArchivedQuest(archivedQuestData);
        }
    }

    private void LoadArchivedQuest(ArchivedQuestData archivedQuestData)
    {
        GameObject ob = Instantiate(archivedQuestPrefab);
        ob.transform.SetParent(transform);
        ArchivedQuest archivedQuest = ob.GetComponent<ArchivedQuest>();
        archivedQuest.Initialize(archivedQuestData, this, translation.GetStaticString(24), translation.GetStaticString(76));
        archivedQuestsObjects.Add(archivedQuest);
    }

    private void Reload()
    {
        if (archivedQuestsObjects != null) Unload();
        Load();
    }

    private void Unload()
    {
        foreach (ArchivedQuest archivedQuest in archivedQuestsObjects)
        {
            Destroy(archivedQuest.gameObject);
        }
        archivedQuestsObjects.Clear();
    }

    public void ArchiveQuest(QuestData questData, bool compleated)
    {
        if (archivedQuests == null) LoadFromFile(); 
        ArchivedQuestData archivedQuestData = new ArchivedQuestData(questData, DateTime.Now, compleated);
        archivedQuests.Insert(0,archivedQuestData);
        Save();
        Reload();
        CheckIfFull();
    }

    public void CheckIfFull()
    {
        int maxSize = settings.GetArchiveMaxSize();
        if(archivedQuests.Count > maxSize)
        {
            int removingNumber = archivedQuests.Count - maxSize;
            archivedQuests.RemoveRange(maxSize, removingNumber);
            Save();
            Reload();
        }
    }

    public void RemoveArchivedQuest(ArchivedQuestData archivedQuestData)
    {
        archivedQuests.Remove(archivedQuestData);
        Save();
        Reload();
    }

    public void ShowDetails(ArchivedQuestData archivedQuestData)
    {
        archiveDetails.gameObject.SetActive(true);
        archiveDetails.ShowDetails(archivedQuestData);
    }
}
