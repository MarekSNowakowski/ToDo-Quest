﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class ArchiveManager : MonoBehaviour
{
    private string filepath;
    private string Filepath
    {
        get
        {
            if(filepath == null)
            {
                filepath = Application.persistentDataPath + "/archive.dat";
            }
            return filepath;
        }
    }
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
        if(archivedQuests==null) LoadFromFile();
        if (archivedQuestsObjects==null) Load();
    }

    public void Save()
    {
        using (FileStream file = File.Create(Filepath))
        {
            new BinaryFormatter().Serialize(file, archivedQuests);
        }
    }

    public void LoadFromFile()
    {
        archivedQuests = new List<ArchivedQuestData>();
        if (File.Exists(Filepath))
        {
            using (FileStream file = File.Open(Filepath, FileMode.Open))
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
