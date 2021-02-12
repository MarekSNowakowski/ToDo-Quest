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
    TextMeshProUGUI rewardName;
    [SerializeField]
    GameObject rewardField;
    [SerializeField]
    Image weightImage;
    [SerializeField]
    TextMeshProUGUI categoryName;
    [SerializeField]
    GameObject categoryField;
    [SerializeField]
    Image categoryIcon;
    [SerializeField]
    TextMeshProUGUI comment;
    [SerializeField]
    GameObject commentField;
    [SerializeField]
    GameObject addPanelCanvas;
    [SerializeField]
    GameObject dateField;
    [SerializeField]
    TextMeshProUGUI date;
    AddPanelManager addPanelManager;
    QuestData questData;
    Category category;
    [SerializeField]
    TranslationManager translationManager;

    [Header("Cycle")]
    [SerializeField]
    Image cycleImage;
    [SerializeField]
    TextMeshProUGUI cycleText;
    [SerializeField]
    CategoryDetails categoryDetails;

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

    private void Start()
    {
        addPanelManager = addPanelCanvas.GetComponent<AddPanelManager>();
    }

    public void ShowQuestDetails(QuestData questData)
    {
        this.questData = questData;
        questName.text = questData.questName;
        if(questData.reward!= null)
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
            CultureInfo cultureInfo = translationManager.GetCultureInfo();
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
            CultureInfo cultureInfo = new CultureInfo("en-US");
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
