using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class RewardDetails : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI rewardName;
    [SerializeField]
    TextMeshProUGUI questName;
    [SerializeField]
    TextMeshProUGUI date;
    [SerializeField]
    GameObject rewardDetailsPanel;
    [SerializeField]
    GameObject questDetailsPanel;

    public void ShowRewardDetails(RewardData rewardData)
    {
        rewardDetailsPanel.SetActive(true);
        questDetailsPanel.SetActive(false);
        rewardName.text = rewardData.rewardName;
        questName.text = rewardData.questName;
        date.text = rewardData.questCompletitionTime.ToShortDateString();
    }

    public void CloseDetails()
    {
        questName.text = "";
        rewardName.text = "";
        date.text = "";
        rewardDetailsPanel.SetActive(false);
        questDetailsPanel.SetActive(true);

        this.gameObject.SetActive(false);
    }
}
