using UnityEngine;
using System;

[Serializable]
public class Category : IComparable<Category>
{
    readonly string id;
    readonly string categoryName;
    string color;
    [NonSerialized]
    CategoryManager categoryManager;
    DateTime creationDateTime;

    public Category(string name, Color color, CategoryManager categoryManager)
    {
        id = CorrelationIdGenerator.GetNextId();
        this.categoryName = name;
        this.color = ColorUtility.ToHtmlStringRGB(color);
        this.categoryManager = categoryManager;
        creationDateTime = DateTime.Now;
    }

    public void SetManager(CategoryManager categoryManager)
    {
        this.categoryManager = categoryManager;
    }

        public Color GetColor()
        {
            ColorUtility.TryParseHtmlString("#"+color, out Color col);
            return col;
        }

        public string GetID()
        {
            if (id != null)
            {
                return id;
            }
            else
            {
                return "No ID";
            }

        }

        public string GetName()
        {
            return categoryName;
        }

    public CategoryManager GetCategoryManager()
    {
        return categoryManager;
    }

    public DateTime GetCreationDate()
    {
        return creationDateTime;
    }

    public int CompareTo(Category other)
    {
        return (-1) * creationDateTime.CompareTo(other.GetCreationDate());
    }
}
