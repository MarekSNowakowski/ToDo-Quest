﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;

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
            date.text = questData.date.ToString("dddd, dd MMMM yyyy");
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
}
