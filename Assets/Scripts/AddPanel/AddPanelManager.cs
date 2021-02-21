using System.Collections;
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
    //Get subQuests if quest is edited
    List<SubQuestData> subQuests = new List<SubQuestData>();

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
    Category editingCategory = null;
    [SerializeField]
    CategoriesBox categoriesBox;

    [Header("Date")]
    [SerializeField]
    Calendar dateCalendar;
    DateTime date;
    int repeatCycle = 0;
    bool dateChosen;

    [Header("Deadline")]
    [SerializeField]
    Calendar deadlineCalendar;
    DateTime deadline;
    bool deadlineChosen;
    bool remind;
    bool autoRemove;
    [SerializeField]
    NotificationManager notificationManager;

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
            if(deadlineChosen)
            {
                questData.deadline = deadline;
                questData.remind = remind;
                questData.autoRemove = autoRemove;
                
                if (remind && deadline!=default)
                {
                    int notificationID = (int)UnityEngine.Random.value * 100;
                    notificationManager.ShedudleNotification(questData.questName, deadline, notificationID);
                    questData.notificationID = notificationID;
                }
            }
            if (editingID != null)
            {
                questManager.RemoveEditedQuest(editingID);
            }
            questData.repeatCycle = repeatCycle;
            questData.subQuests = subQuests;

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
        if (questData.comment != null && questData.comment != "")
        {
            commentFeild.text = questData.comment;
            comment = questData.comment;
            iconManager.FillCommentIcon();
        }
        ChooseCategory(questData.category);
        if(questData.category!=null)
        {
            categoriesBox.OnCategoryChoose(questData.category);
            categoriesBox.LockCategory();
        }
        editingID = questData.ID;
        date = questData.date;
        deadline = questData.deadline;
        repeatCycle = questData.repeatCycle;
        dateCalendar.SetUpCycle(repeatCycle);
        if (date != default)
        {
            dateChosen = true;
            dateCalendar.ChooseDate(date);
            iconManager.FillDateIcon();
        }
        if(deadline!=default)
        {
            deadlineChosen = true;
            deadlineCalendar.ChooseDate(deadline);
            iconManager.FillDeadlineIcon();
            remind = questData.remind;
            autoRemove = questData.autoRemove;
            deadlineCalendar.SetUpDeadlineBools(remind, autoRemove);
        }
        subQuests = questData.subQuests;
    }

    public void EditCategory(Category category)
    {
        OpenCategory();
        categoryInputField.text = category.GetName();
        categoryCreationIcon.color = category.GetColor();
        editingCategory = category;
        categoryInputField.ActivateInputField();
        categoryInputField.Select();
    }

    public Color GetEditingCategoryColor()
    {
        if(editingCategory!=null && editingCategory.GetColor()!=null)
        {
            return editingCategory.GetColor();
        }
        else
        {
            return Color.gray;
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
        dateCalendar.Discard();
        deadlineCalendar.Discard();

        addPanelView.Close();
    }

    public void CancelClose()
    {
        addPanelView.CancelClose();
    }

    public void TryClose()
    {
        if (CanClose())
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
        if (questNameFeild.text != "" || rewardFeild.text != "" || editingID != null || comment != "" || commentFeild.text != "" || dateCalendar.IsDateSelected() ||
            deadlineCalendar.IsDateSelected() || !iconManager.IsWeightIconWhite() || activeCategory != null || dateChosen)
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
        deadlineChosen = false;
        editingCategory = null;
        categoryInputField.text = "";
        categoryCreationIcon.color = Color.white;
        categoriesBox.ClearIcon();
    }

    public void OpenCategory()
    {
        commentPanel.SetActive(false);
        mainAddPanel.SetActive(false);
        categoryPanel.SetActive(true);
        editingCategory = null;
        categoryInputField.text = "";
        categoryCreationIcon.color = Color.white;
        categoryInputField.ActivateInputField();
        categoryInputField.Select();
    }

    public void CloseCategory()
    {
        categoryPanel.SetActive(false);
        categoryInputField.text = "";
        editingCategory = null;
    }

    public void SubmitCategory()
    {
        string name = categoryInputField.text;
        if(editingCategory != null)
        {
            categoryManager.EditCategory(editingCategory, name, categoryCreationIcon.color);
            CloseCategory();
        }
        else
        {
            if (categoryColor != null)
            {
                if (name != null && name != "")
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
                        Debug.LogWarning("Nie można utworzyć kategorii z powodu braku nazwy!");
                    }
                }
                else
                {
                    Debug.LogWarning("Nie można utworzyć kategorii z powodu braku wybranego koloru!");
                }
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
        if (category != null)
        {
            iconManager.SetCategoryIconColor(category.GetColor());
            activeCategory = category;
        }
        else
        {
            iconManager.ClearCategoryIcon();
            activeCategory = null;
        }

    }

    public void SubmitDate(DateTime dateTime, int repeatCycle)
    {
        date = dateTime;
        dateChosen = true;
        addPanelView.CloseDatePanel();
        iconManager.FillDateIcon();
        this.repeatCycle = repeatCycle;
    }

    public void SubmitDate(int repeatCycle)
    {
        this.repeatCycle = repeatCycle;
        if (repeatCycle == 0)
        {
            dateChosen = false;
            addPanelView.CloseDatePanel();
            iconManager.ClearDateIcon();
        }
        else
        {
            date = DateTime.Today;
            dateChosen = true;
            addPanelView.CloseDatePanel();
            iconManager.FillDateIcon();
        }
    }

    public void SubmitDeadline(DateTime dateTime, bool remind, bool autoRemove)
    {
        deadline = dateTime;
        deadlineChosen = true;
        addPanelView.CloseDeadlinePanel();
        iconManager.FillDeadlineIcon();
        this.remind = remind;
        this.autoRemove = autoRemove; 
    }

    public void SubmitDeadline()
    {
        deadlineChosen = false;
        addPanelView.CloseDatePanel();
        iconManager.ClearDeadlineIcon();
    }

    public bool IsDateChosen()
    {
        return dateChosen;
    }

    public bool IsDeadlineChosen()
    {
        return deadlineChosen;
    }

    public QuestManager GetQuestManager()
    {
        return questManager;
    }
}
