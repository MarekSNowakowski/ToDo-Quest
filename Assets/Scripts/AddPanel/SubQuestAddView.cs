using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubQuestAddView : MonoBehaviour
{
    [SerializeField]
    TMPro.TMP_InputField nameInputField;
    [SerializeField]
    QuestManager questManager;
    [SerializeField]
    QuestDetails questDetails;
    [SerializeField]
    GameObject subQuestPanel;

    void Start()
    {
        nameInputField.ActivateInputField();
        nameInputField.Select();
    }

    private void OnDisable()
    {
        nameInputField.text = "";
    }

    public void AddSubQuest()
    {
        if(nameInputField.text != "")
        {
            SubQuestData subQuestData = new SubQuestData(nameInputField.text);
            questDetails.AddSubQuest(subQuestData,questManager);
            subQuestPanel.SetActive(false);
        }
    }
}
