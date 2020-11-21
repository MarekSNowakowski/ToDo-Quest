using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AddPanelManager : MonoBehaviour
{
    [Header("MainAddPanel")]
    [SerializeField]
    GameObject mainAddPanel;
    [SerializeField]
    TMP_InputField questNameFeild;
    [SerializeField]
    TMP_InputField rewardFeild;

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

    public void Submit()
    {
        if (questNameFeild.text == "") Debug.Log("Can't create quest without a name");
        else {
            QuestData questData = new QuestData();
            questData.questName = questNameFeild.text;
            if (rewardFeild.text != "") questData.reward = rewardFeild.text;
            if (comment != "") questData.comment = comment;

            questManager.AddQuest(questData);

            questNameFeild.text = "";
            rewardFeild.text = "";
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
}
