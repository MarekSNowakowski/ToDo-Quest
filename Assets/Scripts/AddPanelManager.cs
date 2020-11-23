using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AddPanelManager : MonoBehaviour
{
    [Header("MainAddPanel")]
    [SerializeField]
    GameObject mainAddPanel;
    [SerializeField]
    TMP_InputField questNameFeild;
    [SerializeField]
    TMP_InputField rewardFeild;
    int weight = 0;
    [SerializeField]
    Image weightImage;

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
    

    public void Submit()
    {
        if (questNameFeild.text == "") Debug.Log("Can't create quest without a name");
        else {
            QuestData questData = new QuestData();
            questData.questName = questNameFeild.text;
            if (rewardFeild.text != "") questData.reward = rewardFeild.text;
            if (comment != "") questData.comment = comment;
            questData.weight = weight;

            if(editingID!=null)
            {
                questManager.RemoveQuest(questManager.FindQuestWithID(editingID));
            }
            questManager.AddQuest(questData);

            ResetWeight();
            questNameFeild.text = "";
            rewardFeild.text = "";
            editingID = null;
            addPanelView.Close();
        }
    }

    public void OpenCommentPanel()
    {
        commentPanel.SetActive(true);
        mainAddPanel.SetActive(false);
        commentFeild.ActivateInputField();
        commentFeild.Select();
    }

    public void CloseCommentPanel()
    {
        commentFeild.text = "";
        commentPanel.SetActive(false);
        mainAddPanel.SetActive(true);
        questNameFeild.ActivateInputField();
        questNameFeild.Select();
    }

    public void SubmitComment()
    {
        comment = commentFeild.text;
        CloseCommentPanel();
    }

    public void IncreaseWeight()
    {
        weight++;
        if (weight == 1) weightImage.color = new Color(0, 210, 0);
        else if (weight == 2) weightImage.color = new Color(0, 0, 210);
        else if (weight == 3) weightImage.color = new Color(210, 0, 0);
        else if (weight == 4 || weight == 0)
        {
            weight = 0;
            weightImage.color = new Color(255, 255, 255);
        }
    }

    public void ResetWeight()
    {
        weight = 0;
        weightImage.color = new Color(255, 255, 255);
    }

    public void EditQuest(QuestData questData)
    {
        questNameFeild.text = questData.questName;
        rewardFeild.text = questData.reward;
        weight = questData.weight - 1;
        IncreaseWeight();
        commentFeild.text = questData.comment;
        editingID = questData.ID;
    }

    public void Close()
    {
        ResetWeight();
        questNameFeild.text = "";
        rewardFeild.text = "";
        editingID = null;
        addPanelView.Close();
    }
}
