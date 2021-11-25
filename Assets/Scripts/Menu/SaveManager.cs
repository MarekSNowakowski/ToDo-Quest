using UnityEngine;
using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public class SaveManager
{
    public SaveManager(string fileName)
    {
        this.fileName = fileName;
    }

    private string fileName;

    private string filePath = null;

    private string SAVE_EXTENSION = ".dat";

    public string FilePath
    {
        get
        {
            if (filePath == null)
                SetFilePath();

            return filePath;
        }
    }

    private void SetFilePath()
    {
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        if (!TryDocumentsDefaultPath())
        {
            if (!TryOneDriveDefaultPath())
            {
                filePath = Application.persistentDataPath + "/" + fileName + SAVE_EXTENSION;
            }
        }
    }

    private bool TryDocumentsDefaultPath()
    {
        string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        if (documentsPath != null && documentsPath.Length > 3)
        {
            string direcotryPath = documentsPath + "\\ToDo-Quest";
            Directory.CreateDirectory(direcotryPath);
            filePath = direcotryPath + "\\" + fileName + SAVE_EXTENSION;
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool TryOneDriveDefaultPath()
    {
        try 
        {
            string direcotryPath = "/OneDrive/Documents/ToDo-Quest/";
            if(!Directory.Exists(direcotryPath))
                Directory.CreateDirectory(direcotryPath);
            filePath = direcotryPath + "/" + fileName + SAVE_EXTENSION;
            return true;
        }
        catch
        {
            return false;
        }
    }

    public void SaveData(ISerializable data)
    {
        SaveData<ISerializable>(data);
    }
    public void SaveData<T>(List<T> data) where T : ISerializable
    {
        SaveData<List<T>>(data);
    }

    //This void is private to prevent inproper data. We want to save data only when it is marked as Serializable or it is list of serializable items
    private void SaveData<T>(T data)
    {
        Debug.Log($"Saving data to {FilePath}");

        using FileStream file = File.Create(FilePath);
        new BinaryFormatter().Serialize(file, data);
        file.Close();
    }
}


public interface ISerializable { };
