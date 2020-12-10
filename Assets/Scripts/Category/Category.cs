using UnityEngine;

[System.Serializable]
public class Category
{
    readonly string id;
    readonly string categoryName;
    string color;
    [System.NonSerialized]
    CategoryManager categoryManager;

    public Category(string name, Color color, CategoryManager categoryManager)
    {
        id = CorrelationIdGenerator.GetNextId();
        this.categoryName = name;
        this.color = ColorUtility.ToHtmlStringRGB(color);
        Debug.Log(color);
        this.categoryManager = categoryManager;
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
            return id;
        }

        public string GetName()
        {
            return categoryName;
        }

    public CategoryManager GetCategoryManager()
    {
        return categoryManager;
    }


}
