﻿using System;
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
    List<Label> activeLabels = new List<Label>();

    string lastLabelCreatedID = null;

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

    public void SetCategoryButton()
    {
        CategoriesContainer categoriesContainer;
        container.TryGetComponent<CategoriesContainer>(out categoriesContainer);
        if (categoriesContainer != null) categoriesContainer.SetCategoryButton();
    }

    public void Load(QuestManager questManager, List<QuestData> data)
    {
        if(state == QuestDisplayerState.SortByCategory)
        {
            data.Sort(SortQuestDataByCategory());
        }
        else if(state == QuestDisplayerState.SortByDate)
        {
            data.Sort(SortQuestDataByDate());
        }
            
        foreach (QuestData questData in data)
        {
            TryCreateCategoryLabel(questData.category);
            TryCreateDateLabel(questData.date);
            Quest quest = questFactory.LoadQuest(questData);
            activeQuests.Add(quest);
            quest.GetManager(questManager);
            quest.Load(questData, state);
        }
        if (state == QuestDisplayerState.SortByDate)
        {
            if (lastLabelCreatedID != "6" || lastLabelCreatedID != "7")
            {
                if (lastLabelCreatedID == "Overdue")
                {
                    lastLabelCreatedID = "0";
                }
                CreateLabels(int.Parse(lastLabelCreatedID) + 1);
            }
        }

        lastLabelCreatedID = null;
    }

    void TryCreateCategoryLabel(Category category)
    {
        //Category Label Creation
        if(state == QuestDisplayerState.SortByCategory)
        {
            if (category != null && category.GetID() != lastLabelCreatedID)
            {
                lastLabelCreatedID = category.GetID();
                CategoryLabel categoryLabel = labelFactory.LoadCategory(category);
                categoryLabel.QuestAdded();
                activeLabels.Add(categoryLabel);
            } else if (category != null && category.GetID() == lastLabelCreatedID)
            {
                activeLabels[activeLabels.Count - 1].QuestAdded();
            }
            else if (category == null && lastLabelCreatedID != "No category")
            {
                lastLabelCreatedID = "No category";
                CategoryLabel categoryLabel = labelFactory.LoadOthersCategoryLabel();
                categoryLabel.QuestAdded();
                activeLabels.Add(categoryLabel);
            }
            else if (category == null && activeLabels.Exists(x=>x.GetID() == "No category"))
            {
                activeLabels[activeLabels.Count-1].QuestAdded();
            }
        }
    }


    /// <summary>
    /// Contains instructions when to create dateLabel
    /// </summary>
    /// <param name="date"></param>
    void TryCreateDateLabel(DateTime date)
    {
        if (state == QuestDisplayerState.SortByDate)
        {
            switch (lastLabelCreatedID)
            {
                //At the start create Overdue label if date is overdue
                case null:
                    if(date != default && date < DateTime.Today)
                    {
                        lastLabelCreatedID = "Overdue";
                        DateLabel dateLabel = labelFactory.LoadDate(date);
                        dateLabel.QuestAdded();
                        activeLabels.Add(dateLabel);
                    }
                    //If there is no overdue - create today label
                    else
                    {
                        CreateLabelsTillDate(0,date);
                    }
                    break;
                case "Overdue":
                    if (date != default && date < DateTime.Today)
                    {
                        activeLabels[activeLabels.Count - 1].QuestAdded();
                    }
                    else
                    {
                        CreateLabelsTillDate(0,date);
                    }
                    break;
                case "0":
                    CheckIfLabelCreated(date, 0);
                    break;
                case "1":
                    CheckIfLabelCreated(date, 1);
                    break;
                case "2":
                    CheckIfLabelCreated(date, 2);
                    break;
                case "3":
                    CheckIfLabelCreated(date, 3);
                    break;
                case "4":
                    CheckIfLabelCreated(date, 4);
                    break;
                case "5":
                    CheckIfLabelCreated(date, 5);
                    break;
                case "6":
                    CheckIfLabelCreated(date, 6);
                    break;
                case "7":
                    CheckIfLabelCreated(date, 7);
                    break;

            }
        }
    }

    void CreateLabelsTillDate(int start, DateTime date)
    {
        if(start >= 0 && start <= 7)
        {
            for (int i = start; i <= 7; i++)
            {
                if(i<7)
                {
                    lastLabelCreatedID = i.ToString();
                    DateLabel dateLabel = labelFactory.LoadDate(DateTime.Today.AddDays(i));
                    activeLabels.Add(dateLabel);
                    if (date == DateTime.Today.AddDays(i))
                    {
                        dateLabel.QuestAdded();
                        break;
                    }
                    else
                    {
                        dateLabel.TurnInactive();
                    }
                }
                else
                {
                    lastLabelCreatedID = i.ToString();
                    DateLabel dateLabel = labelFactory.LoadDate(default);
                    activeLabels.Add(dateLabel);
                    if (date == default || date >= DateTime.Today.AddDays(7)) 
                    {
                        dateLabel.QuestAdded();
                        break;
                    }
                }
            }
        }
    }

    void CreateLabels(int start)
    {
        if (start >= 0 && start < 7)
        {
            for (int i = start; i <= 7; i++)
            {
                if (i < 7)
                {
                    lastLabelCreatedID = i.ToString();
                    DateLabel dateLabel = labelFactory.LoadDate(DateTime.Today.AddDays(i));
                    activeLabels.Add(dateLabel);

                    dateLabel.TurnInactive();
                }
            }
        }
    }

    void CheckIfLabelCreated(DateTime date, int num)
    {
        if (date == DateTime.Today.AddDays(num) || (num == 7 && date == default))
        {
            //Add quest to last label
            activeLabels[activeLabels.Count - 1].QuestAdded();
        }
        else
        {
            CreateLabelsTillDate(num + 1, date);
        }
    }

    public void Unload()
    {
        questFactory.transform.DetachChildren();
        foreach (Quest quest in activeQuests)
        {
            Destroy(quest.gameObject);
        }
        foreach (Label label in activeLabels)
        {
            Destroy(label.gameObject);
        }
        activeQuests.Clear();
        activeLabels.Clear();
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
        lastLabelCreatedID = null;
        if (state==QuestDisplayerState.SortByCategory)
        {
            Label categoryLabel;
            if(quest.GetCategory() == null)
            {
                categoryLabel = activeLabels.Find(x => x.GetID() == "No category");
            }
            else
            {
                categoryLabel = activeLabels.Find(x => x.GetID() == quest.GetCategory().GetID());
            }
            categoryLabel.QuestRemoved();
            if(categoryLabel.GetNumberOfQuestsInside() == 0)
            {
                activeLabels.Remove(categoryLabel);
                Destroy(categoryLabel.gameObject);
            }
        }
        else if (state == QuestDisplayerState.SortByDate)
        {
            Label label = GetDateLabel(quest.date);

            label.QuestRemoved();
            if (label.GetNumberOfQuestsInside() == 0)
            {
                if(label.GetID()=="Other" || label.GetID() == "Overdue")
                {
                    activeLabels.Remove(label);
                    Destroy(label.gameObject);
                }
                else
                {
                    label.GetComponent<DateLabel>().TurnInactive();
                }
            }
        }
    }

    Label GetDateLabel(DateTime date)
    {
        string id;
        if(date == default || date >= DateTime.Today.AddDays(7))
        {
            id = "Other";
        }
        else if(date < DateTime.Today)
        {
            id = "Overdue";
        }
        else
        {
            id = date.ToString();
        }

        return activeLabels.Find(x => x.GetID() == id);
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

    public void ShowCategoryQuests(QuestManager questManager, List<QuestData> data)
    {
        foreach (QuestData questData in data)
        {
            Quest quest = questFactory.LoadQuest(questData);
            activeQuests.Add(quest);
            quest.GetManager(questManager);
            quest.Load(questData, state);
        }
    }

    private class SortQuestDataByCategoryHelper : IComparer<QuestData>
    {
        public int Compare(QuestData x, QuestData y)
        {
            if (x.category == y.category)
            {
                return (-1) * x.weight.CompareTo(y.weight);
            }
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

    private class SortQuestDataByDateHelper : IComparer<QuestData>
    {
        public int Compare(QuestData x, QuestData y)
        {
            if(x.date == y.date)
            {
                return (-1) * x.weight.CompareTo(y.weight);
            }
            if(x.date != default && y.date != default)
            {
                return x.date.CompareTo(y.date);
            }
            else if(x.date == default && y.date != default)
            {
                return 1;
            }
            else if(x.date != default && y.date == default)
            {
                return -1;
            }
            else
            {
                return 0;
            }
            
        }
    }

    public static IComparer<QuestData> SortQuestDataByDate()
    {
        return new SortQuestDataByDateHelper();
    }
}

public enum QuestDisplayerState
{
    SortByCategory,
    SortByDate,
    ShowOneCategory
}