using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddPanelManager : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_InputField questName;
    [SerializeField]
    TMPro.TMP_InputField reward;
    [SerializeField]
    QuestManager questManager;
    [SerializeField]
    AddPanelView addPanelView;

    public void Submit()
    {
        if (questName.text == "") Debug.Log("Can't create quest without a name");
        else {
            QuestData questData = new QuestData();
            questData.questName = questName.text;
            if (reward.text != "") questData.reward = reward.text;

            questManager.AddQuest(questData);

            questName.text = "";
            reward.text = "";
            addPanelView.Close();
        }
    }
}
