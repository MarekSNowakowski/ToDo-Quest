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
    SubQuestDisplayer subQuestDisplayer;
    [SerializeField]
    GameObject subQuestPanel;
    RectTransform myRectTransform;
    float keyboardHeight = 0;
    float screenHeight = Screen.height;

    void Start()
    {
        myRectTransform = GetComponent<RectTransform>();
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
