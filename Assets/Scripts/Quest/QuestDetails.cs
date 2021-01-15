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

    [Header("Cycle")]
    [SerializeField]
    Image cycleImage;
    [SerializeField]
    TextMeshProUGUI cycleText;

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
            CultureInfo cultureInfo = new CultureInfo("en-US");
            date.text = questData.date.ToString("dddd, dd MMMM yyyy", cultureInfo);
            if(questData.repeatCycle != 0)
            {
                cycleImage.gameObject.SetActive(true);

                switch (questData.repeatCycle)
                {
                    case 1:
                        cycleText.text = "Every day";
                        break;
                    case 7:
                        cycleText.text = "Every week";
                        break;
                    case 14:
                        cycleText.text = "Every 2 weeks";
                        break;
                    case 30:
                        cycleText.text = "Every month";
                        break;
                }
            }
        }
        if (questData.category != null)
        {
            categoryField.SetActive(true);
            categoryName.text = questData.category.GetName();
            categoryIcon.color = questData.category.GetColor();
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
        questName.text = "";
        rewardName.text = "";
        //date.text = "";
        categoryName.text = "";
        categoryIcon.color = Color.white;
        date.text = "";
        cycleImage.gameObject.SetActive(false);
        cycleText.text = "";

        rewardField.SetActive(false);
        commentField.SetActive(false);
        //dateField.SetActive(false);
        categoryField.SetActive(false);
        dateField.SetActive(false);
        this.gameObject.SetActive(false);
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
}
