using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SubQuest : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI nameText;
    [SerializeField]
    Image completedImage;
    [SerializeField]
    Sprite completedSprite;
    [SerializeField]
    Sprite notCompletedSprite;

    SubQuestDisplayer subQuestDisplayer;
    SubQuestData data;

    bool completed = false;

    public void Set(SubQuestData data, SubQuestDisplayer subQuestDisplayer)
    {
        this.data = data;
        this.subQuestDisplayer = subQuestDisplayer;
        if (data.completed)
        {
            completedImage.sprite = completedSprite;
            completed = true;
            nameText.text = $"<s>{data.name}</s>";
        }
        else
        {
            nameText.text = data.name;
        }
    }

    public void OnCompleteButtonPress()
    {
        if (completed)
        {
            nameText.text = data.name;
            completedImage.sprite = notCompletedSprite;
            completed = false;
        }
        else
        {
            nameText.text = $"<s>{data.name}</s>";
            completedImage.sprite = completedSprite;
            completed = true;
        }
        subQuestDisplayer.ChangeSubQuestCompletition(completed, data);
    }

    public void Remove()
    {
        subQuestDisplayer.RemoveSubQuest(data);
    }

    public bool IsCompleted()
    {
        return completed;
    }
}

[Serializable]
public class SubQuestData
{
    public string name;
    public bool completed;

    public SubQuestData(string name)
    {
        this.name = name;
        completed = false;
    }
}
