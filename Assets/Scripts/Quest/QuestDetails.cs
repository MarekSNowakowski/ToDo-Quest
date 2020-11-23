using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

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
    //[SerializeField]
    //TextMeshProUGUI date;
    //[SerializeField]
    //GameObject dateField;
    //[SerializeField]
    //TextMeshProUGUI categoryName;
    //[SerializeField]
    //GameObject categoryField;
    [SerializeField]
    TextMeshProUGUI comment;
    [SerializeField]
    GameObject commentField;

    public void ShowQuestDetails(QuestData questData)
    {
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
        //date and category showing TBD
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
                break;
        }
    }

    public void CloseDetails()
    {
        questName.text = "";
        rewardName.text = "";
        //date.text = "";
        //category.text = "";

        rewardField.SetActive(false);
        commentField.SetActive(false);
        //dateField.SetActive(false);
        //categoryField.SetActive(false);

        this.gameObject.SetActive(false);
    }
}
