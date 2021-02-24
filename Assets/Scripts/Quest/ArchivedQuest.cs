using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

public class ArchivedQuest : MonoBehaviour
{
    private ArchivedQuestData data;
    private ArchiveManager archiveManager;
    [SerializeField]
    TMPro.TextMeshProUGUI questName;
    [SerializeField]
    TMPro.TextMeshProUGUI date;
    [SerializeField]
    Image completitionImage;
    [SerializeField]
    Sprite compleatedIcon;
    [SerializeField]
    Sprite removedIcon;

    public void Initialize(ArchivedQuestData data, ArchiveManager archiveManager, string today, string yesterday)
    {
        this.data = data;
        this.archiveManager = archiveManager;
        Set(today, yesterday);
    }

    private void Set(string today, string yesterday)
    {
        questName.text = data.questData.questName;

        if (data.compleated)
        {
            completitionImage.sprite = compleatedIcon;
        }
        else
        {
            completitionImage.sprite = removedIcon;
        }

        if (data.completitionDate.Date == DateTime.Today)
        {
            date.text = today;
        }
        else if (data.completitionDate.Date == DateTime.Today.AddDays(-1))
        {
            date.text = yesterday;
        }
        else
        {
            date.text = data.completitionDate.ToShortTimeString();
        }
    }

    public void ShowDetails()
    {
        archiveManager.ShowDetails(data);
    }
}

[Serializable]
public class ArchivedQuestData
{
    public QuestData questData;
    public DateTime completitionDate;
    public bool compleated;

    public ArchivedQuestData(QuestData questData, DateTime completitionDate, bool compleated)
    {
        this.questData = questData;
        this.completitionDate = completitionDate;
        this.compleated = compleated;
    }
}
