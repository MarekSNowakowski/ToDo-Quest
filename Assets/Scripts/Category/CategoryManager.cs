﻿using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class CategoryManager : MonoBehaviour
{
    string filepath;
    List<Category> categories;
    [SerializeField]
    CategoriesBox categoriesBox;
    [SerializeField]
    AddPanelManager addPanelManager;

    // Start is called before the first frame update
    void Start()
    {
        categories = new List<Category>();
        filepath = Application.persistentDataPath + "/saveC.dat";
        LoadCategories();
        categoriesBox.RefreshSize(categories.Count);
        categoriesBox.LoadCategories(categories);
    }

    void LoadCategories()
    {
        if (File.Exists(filepath))
        {
            using (FileStream file = File.Open(filepath, FileMode.Open))
            {
                if (file.Length != 0)
                {
                    object loadedData = new BinaryFormatter().Deserialize(file);
                    categories = (List<Category>)loadedData;
                }
            }
            AddManagers();
        }
        else
        {
            Save();
        }
    }

    void AddManagers()
    {
        foreach(Category category in categories)
        {
            category.SetManager(this); 
        }
    }

    void Save()
    {
        using (FileStream file = File.Create(filepath))
        {
            new BinaryFormatter().Serialize(file, categories);
        }
    }
    
    public void AddCategory(string name, Color color)
    {
        Category category = new Category(name, color, this);
        categories.Add(category);
        categoriesBox.RefreshSize(categories.Count);
        categoriesBox.LoadCategory(category);
        Save();
    }

    public bool CheckColor(Color color)
    {
        if (categories.Count == 0) return false;
        return categories.Find(x => x.GetColor() == color).GetID() != null;
    }

    public void ChooseCategory(Category category)
    {
        addPanelManager.ChooseCategory(category);
        categoriesBox.CloseCategoriesContainer();
    }
}

