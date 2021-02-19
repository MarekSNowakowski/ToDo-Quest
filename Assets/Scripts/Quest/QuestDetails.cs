using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Globalization;

public class QuestDetails : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI questName;
    [SerializeField]
    Image weightImage;

    [Header("Reward")]
    [SerializeField]
    GameObject rewardField;
    [SerializeField]
    TextMeshProUGUI rewardName;

    [Header("Category")]
    [SerializeField]
    GameObject categoryField;
    [SerializeField]
    TextMeshProUGUI categoryName;
    [SerializeField]
    Image categoryIcon;

    [Header("Comment")]
    [SerializeField]
    GameObject commentField;
    [SerializeField]
    TextMeshProUGUI comment;

    [Header("Date")]
    [SerializeField]
    GameObject dateField;
    [SerializeField]
    TextMeshProUGUI date;

    [Header("Cycle")]
    [SerializeField]
    Image cycleImage;
    [SerializeField]
    TextMeshProUGUI cycleText;

    [Header("Deadline")]
    [SerializeField]
    GameObject deadlineField;
    [SerializeField]
    TextMeshProUGUI deadlineText;
    [SerializeField]
    Image deadlineImage;
    [SerializeField]
    Image remindImage;
    [SerializeField]
    Image autoRemove;

    [Header("Other")]
    [SerializeField]
    GameObject addPanelCanvas;
    [SerializeField]
    CategoryDetails categoryDetails;
    AddPanelManager addPanelManager;
    QuestData questData;
    Category category;
    [SerializeField]
    TranslationManager translationManager;

    private void Start()
    {
        addPanelManager = addPanelCanvas.GetComponent<AddPanelManager>();
    }

    public void ShowQuestDetails(QuestData questData)
    {
        this.questData = questData;
        questName.text = questData.questName;
        CultureInfo cultureInfo = translationManager.GetCultureInfo();
        if (questData.reward!= null)
        {
            rewardField.SetActive(true);
            rewardName.text = questData.reward;
        }
        if (questData.comment != null)
        {
            commentField.SetActive(true);
            comment.text = questData.comment;
        }
        if (questData.date != default)
        {
            dateField.SetActive(true);
            date.text = questData.date.ToString("dddd, dd MMMM yyyy", cultureInfo);
            if(questData.repeatCycle != 0)
            {
                cycleImage.gameObject.SetActive(true);

                switch (questData.repeatCycle)
                {
                    case 1:
                        cycleText.text = translationManager.GetStaticString(20);
                        break;
                    case 7:
                        cycleText.text = translationManager.GetStaticString(30) + $" {questData.date.ToString("dddd", cultureInfo).ToLower()}";
                        break;
                    case 14:
                        cycleText.text = translationManager.GetStaticString(30) + " " + translationManager.GetStaticString(28);
                        break;
                    case 30:
                        cycleText.text = translationManager.GetStaticString(30) + " " + translationManager.GetStaticString(29);
                        break;
                }
            }
        }
        if (questData.deadline != default)
        {
            deadlineField.SetActive(true);
            if (questData.deadline == DateTime.Today) deadlineImage.color = Color.yellow;
            else if (questData.deadline < DateTime.Today) deadlineImage.color = Color.red;
            deadlineText.text = questData.deadline.ToString("dddd, dd MMMM yyyy", cultureInfo);
            if (questData.remind) remindImage.gameObject.SetActive(true);
            if (questData.autoRemove) autoRemove.gameObject.SetActive(true);
        }
        if (questData.category != null)
        {
            categoryField.SetActive(true);
            categoryName.text = questData.category.GetName();
            categoryIcon.color = questData.category.GetColor();
            category = questData.category;
        }
        switch (questData.weight)
        {
            case 1:
                weightImage.color = new Color(0, 210, 0);
                break;
            case 2:
                weightImage.color = new Color(0, 0, 210);
                break;
            case 3:
                weightImage.color = new Color(210, 0, 0);
                break;
            default:
                weightImage.color = new Color(255, 255, 255);
                break;
        }
    }

    public void CloseDetails()
    {
        Clear();
        this.gameObject.SetActive(false);
    }

    public void Clear()
    {
        questName.text = "";
        rewardName.text = "";
        categoryName.text = "";
        categoryIcon.color = Color.white;
        date.text = "";
        cycleImage.gameObject.SetActive(false);
        cycleText.text = "";
        category = null;
        deadlineImage.color = Color.white;
        deadlineText.text = "";
        remindImage.gameObject.SetActive(false);
        autoRemove.gameObject.SetActive(false);

        rewardField.SetActive(false);
        commentField.SetActive(false);
        categoryField.SetActive(false);
        dateField.SetActive(false);
        deadlineField.SetActive(false);
    }

    public void EditQuest()
    {
        addPanelCanvas.SetActive(true);
        addPanelManager.EditQuest(questData);
        CloseDetails();
    }
        
    public void RemoveQuest()
    {
        addPanelManager.GetQuestManager().FastRemoveQuest(questData.ID);
    }

    public void OpenCategoryDetails()
    {
        categoryDetails.ShowCategoryDetails(category);
        Clear();
    }
}
