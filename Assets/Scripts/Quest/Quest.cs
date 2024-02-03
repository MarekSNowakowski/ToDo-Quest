using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Threading;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Quest : MonoBehaviour, IComparable<Quest>, IDragHandler, IEndDragHandler
{
    [SerializeField]
    readonly float screenHeightRatio = 0.045f;
    [SerializeField]
    TMPro.TextMeshProUGUI nameText;
    [SerializeField]
    GameObject rewardImage;
    [SerializeField]
    GameObject commentImage;
    [SerializeField]
    Image weightImage;
    [SerializeField]
    Image categoryImage;
    [SerializeField]
    GameObject repeatCycleImage;
    [SerializeField]
    Image deadlineImage;

    QuestManager questManager;
    //Important for sorting
    QuestDisplayerState sortingState;

    [Header("Quest Data")]
    public string ID;
    string questName;
    string reward;
    int weight;
    string comment;
    DateTime questCreationDateTime;
    Category category;
    DateTime date;
    int repeatCycle;
    DateTime deadline;
    bool remind;
    bool autoRemove;
    int notificationID;
    List<SubQuestData> subQuests = new List<SubQuestData>();

    [Header("Removal")]
    [SerializeField]
    GameObject removeButton;
    [SerializeField]
    GameObject cancelRemovalButton;
    bool toBeRemoved;
    private bool thisQuestIsRemoving;

    // Dragging
    private RectTransform rectTransform;
    private int newIndex;
    private Transform dateTarget;
    private DateTime newDate;
    private Category newCategory;


    private void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, screenHeightRatio * Screen.height);
    }

    void SetUp()
    {
        nameText.text = questName;
        if (reward != "" && reward != null) rewardImage.gameObject.SetActive(true);
        if (comment != "" && comment != null) commentImage.gameObject.SetActive(true);
        if (repeatCycle != 0) repeatCycleImage.SetActive(true);
        if (deadline != default)
        {
            deadlineImage.gameObject.SetActive(true);
            if (deadline == DateTime.Today) deadlineImage.color = Color.yellow;
            if (deadline < DateTime.Today) deadlineImage.color = Color.red;
        }
        if (category != null && sortingState != QuestDisplayerState.SortByCategory && sortingState != QuestDisplayerState.ShowOneCategory)
        {
            categoryImage.gameObject.SetActive(true);
            categoryImage.color = category.GetColor();
        }
        switch (weight)
        {
            case 1:
                weightImage.color = new Color(0, 210, 0);
                if(cancelRemovalButton) cancelRemovalButton.GetComponent<Image>().color = new Color(0, 210, 0);
                break;
            case 2:
                weightImage.color = new Color(0, 0, 210);
                if (cancelRemovalButton) cancelRemovalButton.GetComponent<Image>().color = new Color(0, 0, 210);
                break;
            case 3:
                weightImage.color = new Color(210, 0, 0);
                if (cancelRemovalButton) cancelRemovalButton.GetComponent<Image>().color = new Color(210, 0, 0);
                break;
            default:
                break;
        }

    }

    public QuestData Save()
    {
        QuestData saveData = new QuestData(ID, questName, reward, weight, comment, category, questCreationDateTime, date, repeatCycle, deadline, remind, autoRemove, notificationID, subQuests);
        return saveData;
    }

    public void Load(QuestData questData, QuestDisplayerState state)
    {
        this.ID = questData.ID;
        this.questName = questData.questName;
        this.reward = questData.reward;
        this.weight = questData.weight;
        this.comment = questData.comment;
        this.questCreationDateTime = questData.creationDateTime;
        this.category = questData.category;
        this.date = questData.date;
        this.sortingState = state;
        this.repeatCycle = questData.repeatCycle;
        this.deadline = questData.deadline;
        this.remind = questData.remind;
        this.autoRemove = questData.autoRemove;
        this.notificationID = questData.notificationID;
        this.subQuests = questData.subQuests;
        SetUp();
    }

    public void GetManager(QuestManager questManager)
    {
        this.questManager = questManager;
    }

    public DateTime GetDate()
    {
        return date;
    }

    public DateTime GetDeadline()
    {
        return deadline;
    }

    public void Remove()
    {
        questManager.StartRemovall(ID);
        thisQuestIsRemoving = true;
        questManager.OnQuestRemovalStart(ID);
    }

    public void Remove(float time)
    {
        questManager.StartRemovall(ID, time);
        thisQuestIsRemoving = true;
    }

    public void RemoveSelf()
    {
        StartCoroutine(ToBeRemovedCO());
    }

    public void RemoveSelf(float time)
    {
        StartCoroutine(ToBeRemovedCO(time));
    }

    public void CancellRemoval()
    {
        toBeRemoved = false;
        thisQuestIsRemoving = false;
        nameText.text = questName;
        cancelRemovalButton.SetActive(false);
        removeButton.SetActive(true);
    }

    public void StartCancellRemoval()
    {
        questManager.CancellRemoval(ID);
        questManager.OnQuestRemovalStop(ID);
    }

    IEnumerator ToBeRemovedCO()
    {
        questManager.OnQuestRemovalStart(ID);
        float questRemovalTime = 3;

        toBeRemoved = true;
        nameText.text = $"<s>{questName}</s>";
        cancelRemovalButton.SetActive(true);
        removeButton.SetActive(false);

        for (int i = 0; toBeRemoved && i < questRemovalTime; i++)
        {
            yield return new WaitForSeconds(1);
        }
        if(toBeRemoved && thisQuestIsRemoving)
        {
            questManager.RemoveQuest(this.ID);
        }
        toBeRemoved = false;
        questManager.OnQuestRemovalStop(ID);
    }

    IEnumerator ToBeRemovedCO(float time)
    {
        float questRemovalTime = 3 - time;

        toBeRemoved = true;
        nameText.text = $"<s>{questName}</s>";
        cancelRemovalButton.SetActive(true);
        removeButton.SetActive(false);

        for (int i = 0; toBeRemoved && i < questRemovalTime; i++)
        {
            yield return new WaitForSeconds(1);
        }
        if (toBeRemoved && thisQuestIsRemoving)
        {
            questManager.RemoveQuest(this.ID);
        }
        toBeRemoved = false;
        questManager.OnQuestRemovalStop(ID);
    }

    public void ShowDetails()
    {
        QuestData questData = Save();
        questManager.ShowQuestDetails(questData);
    }

    public int CompareTo(Quest other)
    {
        if(sortingState == QuestDisplayerState.SortByCategory)
        {
            if (this.category != other.category) return (-1) * category.CompareTo(other.category);
            else if (this.weight != other.weight) return (-1) * weight.CompareTo(other.weight);
            else return (-1) * questCreationDateTime.CompareTo(other.questCreationDateTime);
        }
        else
        {
            if (this.weight != other.weight) return (-1) * weight.CompareTo(other.weight);
            else return (-1) * questCreationDateTime.CompareTo(other.questCreationDateTime);
        }
    }

    public string GetName()
    {
        return this.questName;
    }

    public Category GetCategory()
    {
        return category;
    }


    public void OnDrag(PointerEventData eventData)
    {
        if(Input.mousePosition.y > rectTransform.position.y + rectTransform.sizeDelta.y / 2 && rectTransform.GetSiblingIndex() > 1)
        {
            newIndex = rectTransform.GetSiblingIndex() - 1;
            dateTarget = rectTransform.parent.GetChild(newIndex - 1);
            newDate = default;
            rectTransform.SetSiblingIndex(newIndex);
        }
        else if(Input.mousePosition.y < rectTransform.position.y - rectTransform.sizeDelta.y / 2 && rectTransform.GetSiblingIndex() < rectTransform.parent.childCount - 1)
        {
            newIndex = rectTransform.GetSiblingIndex() + 1;
            dateTarget = rectTransform.parent.GetChild(newIndex);
            newDate = default;
            rectTransform.SetSiblingIndex(newIndex);
        }
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if(dateTarget != null)
        {
            if (sortingState == QuestDisplayerState.SortByDate)
            {
                ChangeQuestDateOnEndDrag();
            }
            else if(sortingState == QuestDisplayerState.SortByCategory)
            {
                ChangeQuestCategoryOnEndDrag();
            }
        }
    }

    private void ChangeQuestDateOnEndDrag()
    {
        if (dateTarget.TryGetComponent(out Quest quest))
        {
            newDate = quest.GetDate();
        }
        else if (dateTarget.TryGetComponent(out DateLabel dateLabel))
        {
            newDate = dateLabel.GetDate();
        }

        if (date != newDate)
        {
            Debug.Log("New date: " + newDate);
            date = newDate;
            QuestData questData = Save();
            questManager.RemoveEditedQuest(questData.ID);
            questData.date = date;
            questManager.AddQuest(questData);
        }
    }

    private void ChangeQuestCategoryOnEndDrag()
    {
        if (dateTarget.TryGetComponent(out Quest quest))
        {
            newCategory = quest.GetCategory();
        }
        else if (dateTarget.TryGetComponent(out CategoryLabel dateLabel))
        {
            newCategory = dateLabel.GetCategory();
        }

        if (category != newCategory)
        {
            if(newCategory != null)
            {
                Debug.Log("New category: " + newCategory.GetName());
            }
            else
            {
                Debug.Log("Category removed");
            }
            
            category = newCategory;
            QuestData questData = Save();
            questManager.RemoveEditedQuest(questData.ID);
            questData.category = category;
            questManager.AddQuest(questData);
        }
    }
}

[Serializable]
public struct QuestData : IComparable<QuestData>, ISerializable
{
    public string ID;
    public string questName;
    public string reward;
    public int weight;
    public string comment;
    public DateTime creationDateTime;
    public DateTime date;
    public DateTime deadline;
    public Category category;
    public int repeatCycle;
    public bool remind;
    public bool autoRemove;
    public int notificationID;
    public List<SubQuestData> subQuests;

    public QuestData(string ID, string questName, string reward, int weight, string comment, Category category,
        DateTime creationDateTime, DateTime date, int repeatCycle, DateTime deadline, bool remind, bool autoRemove, int notificationID, List<SubQuestData> subQuests)
    {
        this.ID = ID;
        this.questName = questName;
        this.reward = reward;
        this.weight = weight;
        this.comment = comment;
        this.creationDateTime = DateTime.Now;
        this.category = category;
        this.date = date;
        this.repeatCycle = repeatCycle;
        this.deadline = deadline;
        this.remind = remind;
        this.autoRemove = autoRemove;
        this.notificationID = notificationID;
        this.subQuests = subQuests;
    }

    public void Initialize()
    {
        creationDateTime = DateTime.Now;
        ID = CorrelationIdGenerator.GetNextId();
        if(subQuests == null)
        {
            subQuests = new List<SubQuestData>();
        }
    }

    public int CompareTo(QuestData other)
    {
        if (this.weight != other.weight) return (-1) * weight.CompareTo(other.weight);
        else return (-1) * creationDateTime.CompareTo(other.creationDateTime);
    }

    public void RemoveCategory()
    {
        this.category = null;
    }

    //public void UpdateData(string questName, string reward, int weight, string comment)
    //{
    //    this.questName = questName;
    //    this.reward = reward;
    //    this.weight = weight;
    //    this.comment = comment;
    //}
}

internal static class CorrelationIdGenerator
{
    private static long _lastId = DateTime.UtcNow.Ticks;

    public static string GetNextId() => GenerateId(Interlocked.Increment(ref _lastId));

    private static string GenerateId(long id)
    {

        string _encode32Chars = "0123456789ABCDEFGHIJKLMNOPQRSTUV";

        var buffer = new char[13];

        buffer[0] = _encode32Chars[(int)(id >> 60) & 31];
        buffer[1] = _encode32Chars[(int)(id >> 55) & 31];
        buffer[2] = _encode32Chars[(int)(id >> 50) & 31];
        buffer[3] = _encode32Chars[(int)(id >> 45) & 31];
        buffer[4] = _encode32Chars[(int)(id >> 40) & 31];
        buffer[5] = _encode32Chars[(int)(id >> 35) & 31];
        buffer[6] = _encode32Chars[(int)(id >> 30) & 31];
        buffer[7] = _encode32Chars[(int)(id >> 25) & 31];
        buffer[8] = _encode32Chars[(int)(id >> 20) & 31];
        buffer[9] = _encode32Chars[(int)(id >> 15) & 31];
        buffer[10] = _encode32Chars[(int)(id >> 10) & 31];
        buffer[11] = _encode32Chars[(int)(id >> 5) & 31];
        buffer[12] = _encode32Chars[(int)id & 31];

        return new string(buffer, 0, 13);
    }
}
