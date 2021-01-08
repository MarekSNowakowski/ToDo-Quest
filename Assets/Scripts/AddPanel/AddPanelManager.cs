﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

public class AddPanelManager : MonoBehaviour
{
    [SerializeField]
    IconManager iconManager;

    [Header("MainAddPanel")]
    [SerializeField]
    GameObject mainAddPanel;
    [SerializeField]
    TMP_InputField questNameFeild;
    [SerializeField]
    TMP_InputField rewardFeild;
    int weight = 0;
    Category activeCategory;


    [Header("CommentPanel")]
    [SerializeField]
    GameObject commentPanel;
    [SerializeField]
    TMP_InputField commentFeild;
    private string comment;

    [Header("Other")]
    [SerializeField]
    QuestManager questManager;
    [SerializeField]
    AddPanelView addPanelView;
    string editingID;

    [Header("CategoryPanel")]
    [SerializeField]
    CategoryManager categoryManager;
    [SerializeField]
    GameObject categoryPanel;
    [SerializeField]
    TMP_InputField categoryInputField;
    [SerializeField]
    Image categoryCreationIcon;
    CategoryColor categoryColor = null;

    [Header("Calendar")]
    [SerializeField]
    Calendar calendar;
    DateTime date;
    bool dateChosen;




    public void Submit()
    {
        if (questNameFeild.text == "") Debug.Log("Can't create quest without a name");
        else {
            QuestData questData = new QuestData();
            questData.questName = questNameFeild.text;
            if (rewardFeild.text != "") questData.reward = rewardFeild.text;
            if (comment != "") questData.comment = comment;
            questData.weight = weight;
            if (activeCategory != null)
            {
                questData.category = activeCategory;
            }
            if (dateChosen)
            {
                questData.date = date;
            }

            if (editingID!=null)
            {
                questManager.RemoveQuest(editingID);
            }

            questManager.AddQuest(questData);


            Close();
        }
    }

    private void Start()
    {
        comment = "";
        this.gameObject.SetActive(false); 
    }

    public void OpenCommentPanel()
    {
        addPanelView.OpenCommentPanel(comment);
    }

    public void SubmitComment()
    {
        comment = commentFeild.text;
        if (commentFeild.text != "")
        {
            iconManager.FillCommentIcon();
        }
        addPanelView.CloseCommentPanel();
    }

    public void IncreaseWeight()
    {
        weight++;
        if (weight == 1) iconManager.SetWeightColor(new Color(0, 210, 0));
        else if (weight == 2) iconManager.SetWeightColor(new Color(0, 0, 210));
        else if (weight == 3) iconManager.SetWeightColor(new Color(210, 0, 0));
        else if (weight == 4 || weight == 0)
        {
            weight = 0;
            iconManager.ClearWeightIcon();
        }
    }

    public void ResetWeight()
    {
        weight = 0;
        iconManager.ClearWeightIcon();
    }

    public void EditQuest(QuestData questData)
    {
        questNameFeild.text = questData.questName;
        rewardFeild.text = questData.reward;
        weight = questData.weight - 1;
        IncreaseWeight();
        commentFeild.text = questData.comment;
        editingID = questData.ID;
        date = questData.date;
        if (date != default)
        {
            dateChosen = true;
            calendar.ChooseDate(date);
            iconManager.FillDateIcon();
        }
    }

    public void Close()
    {
        if (mainAddPanel.activeInHierarchy || commentPanel.activeInHierarchy)
        {
            Clear();
        }
        if (categoryPanel.activeInHierarchy)
        {
            CloseCategory();
        }
        calendar.Discard();

        addPanelView.Close();
    }

    public void CancelClose()
    {
        addPanelView.CancelClose();
    }

    public void TryClose()
    {
        if(CanClose())
        {
            Close();
        }
        else
        {
            addPanelView.TryClose();
        }
    }

    /// <summary>
    /// Checks if there were any changes, if not returns true
    /// </summary>
    bool CanClose()
    {
        if (questNameFeild.text != "" || rewardFeild.text != "" || editingID != null || comment != "" || commentFeild.text != "" || calendar.IsDateSelected() ||
            !iconManager.IsWeightIconWhite() || activeCategory != null ||  dateChosen)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    void Clear()
    {
        ResetWeight();
        questNameFeild.text = "";
        rewardFeild.text = "";
        editingID = null;
        comment = "";
        commentFeild.text = "";
        iconManager.ClearCommentIcon();
        activeCategory = null;
        iconManager.Clear();
        dateChosen = false;
    }

    public void OpenCategory()
    {
        commentPanel.SetActive(false);
        mainAddPanel.SetActive(false);
        categoryPanel.SetActive(true);
    }

    public void CloseCategory()
    {
        categoryPanel.SetActive(false);
        categoryInputField.text = "";
    }

    public void SubmitCategory()
    {
        string name = categoryInputField.text;
        if(categoryColor!=null)
        {
            if(name!=null && name!="")
            {
                categoryManager.AddCategory(name, categoryColor.GetColor());
                categoryColor.Block();
                CloseCategory();
            }
            else
            {
                Debug.LogWarning("Nie można utworzyć kategorii z powodu braku nazwy!");
            }
        }
        else
        {
            if (!categoryManager.CheckColor(Color.white))
            {
                if (name != null && name != "")
                {
                    categoryManager.AddCategory(name, Color.white);
                    CloseCategory();
                }
                else
                {
                    Debug.LogWarning("Nie można utworzyć kategorii z powodu braku wybranego koloru!");
                }
            }
            else
            {
                Debug.LogWarning("Nie można utworzyć kategorii z powodu braku nazwy!");
            }
        }
    }

    public void OnColorChoose(CategoryColor categoryColor)
    {
        this.categoryColor = categoryColor;
        categoryCreationIcon.color = categoryColor.GetColor();
    }

    public CategoryManager GetCategoryManager()
    {
        return categoryManager;
    }

    public void ChooseCategory(Category category)
    {
        iconManager.SetCategoryIconColor(category.GetColor());
        activeCategory = category;
    }

    public void SubmitDate(DateTime dateTime)
    {
        date = dateTime;
        dateChosen = true;
        addPanelView.CloseDatePanel();
        iconManager.FillDateIcon();
    }

    public void SubmitDate()
    {
        dateChosen = false;
        addPanelView.CloseDatePanel();
        iconManager.ClearDateIcon();
    }

    public bool IsDateChosen()
    {
        return dateChosen;
    }
}