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

    // Start is called before the first frame update
    void Start()
    {
        filepath = Application.persistentDataPath + "/saveC.dat";
        LoadCategories();
        categoriesBox.RefreshSize(categories.Count);
    }

    void LoadCategories()
    {
        if (File.Exists(filepath))
        {
            using (FileStream file = File.Open(filepath, FileMode.Open))
            {
                if(file.Length != 0)
                {
                    object loadedData = new BinaryFormatter().Deserialize(file);
                    categories = (List<Category>)loadedData;
                }else
                {
                    categories = new List<Category>();
                }

            }
            categories.Sort();
        }
        else
        {
            Save();
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
        Category category = new Category(name, color);
        categories.Add(category);
        categoriesBox.RefreshSize(categories.Count);
    }
}

public struct Category
{
    public string id;
    public string name;
    public Color color;

    public Category(string name, Color color)
    {
        id = CorrelationIdGenerator.GetNextId();
        this.name = name;
        this.color = color;
    }
}
