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

    public void Set(SubQuestData data)
    {
        nameText.text = data.name;
        if(data.completed)
        {
            completedImage.sprite = completedSprite;
        }
    }

    public void Complete()
    {
        //tbd
    }

    public void UnComplete()
    {
        //tbd
    }

    public void Remove()
    {
        //tbd
    }
}

[Serializable]
public struct SubQuestData
{
    public string name;
    public bool completed;

    public SubQuestData(string name)
    {
        this.name = name;
        completed = false;
    }
}
