using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class QuestDisplayer : MonoBehaviour
{
    [SerializeField]
    QuestFactory questFactory;
    [SerializeField]
    LabelFactory labelFactory;

    [SerializeField]
    QuestContainer container;

    [SerializeField]
    QuestDisplayerState state;

    List<Quest> activeQuests = new List<Quest>();
    List<CategoryLabel> activeCategoryLabels = new List<CategoryLabel>();

    string lastLabelCategoryCreatedID = null;

    //public void AddQuest(QuestManager questManager, QuestData questData)
    //{
    //    Quest quest = questFactory.AddQuest(questData);
    //    quest.GetManager(questManager);
    //    quest.Load(questData);
    //    SetUpAfterAddingQuest(quest);
    //}

    //void SetUpAfterAddingQuest(Quest quest)
    //{
    //    activeQuests.Add(quest);
    //    container.RefreshSize(true);
    //}

    public void RefreshContainer(bool addition)
    {
        container.RefreshSize(addition);
    }

    public void Load(QuestManager questManager, List<QuestData> data)
    {
        if(state == QuestDisplayerState.SortByCategory)
        {
            data.Sort(SortQuestDataByCategory());
        }
            
        foreach (QuestData questData in data)
        {
            TryCreateCategoryLabel(questData.category);
            Quest quest = questFactory.LoadQuest(questData);
            activeQuests.Add(quest);
            quest.GetManager(questManager);
            quest.Load(questData, state);
        }

        lastLabelCategoryCreatedID = null;
    }

    void TryCreateCategoryLabel(Category category)
    {
        //Category Label Creation
        if(state == QuestDisplayerState.SortByCategory)
        {
            if (category != null && category.GetID() != lastLabelCategoryCreatedID)
            {
                lastLabelCategoryCreatedID = category.GetID();
                CategoryLabel categoryLabel = labelFactory.LoadCategory(category);
                categoryLabel.QuestAdded();
                activeCategoryLabels.Add(categoryLabel);
            } else if (category != null && category.GetID() == lastLabelCategoryCreatedID)
            {
                activeCategoryLabels[activeCategoryLabels.Count - 1].QuestAdded();
            }
            else if (category == null && lastLabelCategoryCreatedID != "No category")
            {
                lastLabelCategoryCreatedID = "No category";
                CategoryLabel categoryLabel = labelFactory.LoadOthersCategoryLabel();
                categoryLabel.QuestAdded();
                activeCategoryLabels.Add(categoryLabel);
            }
            else if (category == null && activeCategoryLabels.Exists(x=>x.GetID() == "No category"))
            {
                activeCategoryLabels[activeCategoryLabels.Count-1].QuestAdded();
            }
        }
    }

    public void Unload()
    {
        questFactory.transform.DetachChildren();
        foreach (Quest quest in activeQuests)
        {
            Destroy(quest.gameObject);
        }
        foreach (CategoryLabel categoryLabel in activeCategoryLabels)
        {
            Destroy(categoryLabel.gameObject);
        }
        activeQuests.Clear();
        activeCategoryLabels.Clear();
    }

    public void RemoveQuest(string id)
    {
        Quest quest = FindQuestWithID(id);
        TryRemoveLabel(quest);
        activeQuests.Remove(quest);
        Destroy(quest.gameObject);

        container.RefreshSize(false);
    }

    void TryRemoveLabel(Quest quest)
    {
        lastLabelCategoryCreatedID = null;
        if (state==QuestDisplayerState.SortByCategory)
        {
            CategoryLabel categoryLabel;
            if(quest.GetCategory() == null)
            {
                categoryLabel = activeCategoryLabels.Find(x => x.GetID() == "No category");
            }
            else
            {
                categoryLabel = activeCategoryLabels.Find(x => x.GetID() == quest.GetCategory().GetID());
            }
            categoryLabel.QuestRemoved();
            if(categoryLabel.GetNumberOfQuestsInside() == 0)
            {
                activeCategoryLabels.Remove(categoryLabel);
                Destroy(categoryLabel.gameObject);
            }
        }
    }

    IEnumerator r_waitFrame()
    {
        yield return new WaitForEndOfFrame();
    }

    public Quest FindQuestWithID(string ID)
    {
        return activeQuests.Find(x => x.ID == ID);
    }

    public void StartRemovall(string id)
    {
        FindQuestWithID(id).RemoveSelf();
    }
    
    public void CancellRemoval(string ID)
    {
        FindQuestWithID(ID).CancellRemoval();
    }

    private class SortQuestDataByCategoryHelper : IComparer<QuestData>
    {
        public int Compare(QuestData x, QuestData y)
        {
            
            if (x.category != null && y.category != null)
            {
                return (-1) * x.category.CompareTo(y.category);
            }
            else if(x.category != null && y.category == null)
            {
                return -1;
            }
            else if(x.category == null && y.category != null)
            {
                return 1;
            }
            else
            {
                return 0;
            }
        }
    }

    public static IComparer<QuestData> SortQuestDataByCategory()
    {
        return new SortQuestDataByCategoryHelper();
    }
}

public enum QuestDisplayerState
{
    SortByCategory,
    SortByDate
}