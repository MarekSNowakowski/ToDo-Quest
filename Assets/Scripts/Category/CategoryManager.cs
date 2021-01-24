using System;
using System.Collections;
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
    [SerializeField]
    QuestManager questManager;
    private Color editingCategoryColor;

    // Start is called before the first frame update
    void Start()
    {
        categories = new List<Category>();
        if (categories.Count == 0 || categories == null)
        {
            LoadCategories();
        }
        categoriesBox.RefreshSize(categories.Count);
        categoriesBox.LoadCategories(categories);
    }

    void LoadCategories()
    {
        filepath = Application.persistentDataPath + "/saveC.dat";
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
            if(categories != null && categories.Count!=0)
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
            if(categories!=null)
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

    public void RemoveCategory(Category category)
    {
        categories.RemoveAll(x => x.GetID() == category.GetID());
        foreach (Category c in categories)
        {
            Debug.Log(c.GetName());
        }
        categoriesBox.RefreshSize(categories.Count);
        categoriesBox.UnLoadCategory(category);
        Save();
    }

    public bool CheckColor(Color color)
    {
        if (categories.Count == 0) return false;
        return categories.Exists(x => x.GetColor() != addPanelManager.GetEditingCategoryColor() && x.GetColor() == color);
    }

    public void ChooseCategory(Category category)
    {
        addPanelManager.ChooseCategory(category);
        categoriesBox.CloseCategoriesContainer();
    }

    internal void EditCategory(Category editingCategory, string name, Color color)
    {
        editingCategoryColor = color;
        editingCategory.Edit(name, color);
        categoriesBox.UnLoadCategory(editingCategory);
        categoriesBox.LoadCategory(editingCategory);
        questManager.ReloadQuests();
        categories.RemoveAll(x => x.GetID() == editingCategory.GetID());
        categories.Add(editingCategory);
        questManager.ChangeLabel(editingCategory);
        Save();
    }

    public List<Category> GetCategories()
    {
        if(categories==null || categories.Count == 0)
        {
            LoadCategories();
        }
        return categories;
    }
}


