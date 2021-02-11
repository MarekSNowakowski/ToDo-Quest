using UnityEngine;
using UnityEngine.UI;

class IntroductionTutorial : Tutorial
{
    [SerializeField]
    GameObject menuBarPanel;
    [SerializeField]
    GameObject nameAndRewardPanel;
    [SerializeField]
    GameObject commentPanel;
    [SerializeField]
    GameObject datePanel;
    [SerializeField]
    GameObject deadlinePanel;
    [SerializeField]
    GameObject weightPanel;
    [SerializeField]
    GameObject categoryPanel;
    [SerializeField]
    GameObject savePaenl;
    [SerializeField]
    GameObject sortByDatePanel;
    [SerializeField]
    GameObject sortByCategoryPanel;
    [SerializeField]
    GameObject addCategoryPanel;
    [SerializeField]
    GameObject levelRewardsPanel;
    [SerializeField]
    GameObject levelExpPanel;
    [SerializeField]
    GameObject levelLevelPanel;
    [SerializeField]
    GameObject finalPanel;

    [SerializeField]
    Button nextButton;
    [SerializeField]
    Button previousButton;

    private GameObject currentPanel;

    private void Start()
    {
        currentPanel = menuBarPanel;
        currentTutorialStage = 0;
    }

    public override void OnPageChange()
    {
        switch(currentTutorialStage)
        {
            case 0:
                previousButton.interactable = false;
                currentPanel.SetActive(false);
                currentPanel = menuBarPanel;
                currentPanel.SetActive(true);
                break;
            case 1:
                previousButton.interactable = true;
                currentPanel.SetActive(false);
                currentPanel = nameAndRewardPanel;
                currentPanel.SetActive(true);
                break;
            case 2:
                currentPanel.SetActive(false);
                currentPanel = commentPanel;
                currentPanel.SetActive(true);
                break;
            case 3:
                currentPanel.SetActive(false);
                currentPanel = datePanel;
                currentPanel.SetActive(true);
                break;
            case 4:
                currentPanel.SetActive(false);
                currentPanel = deadlinePanel;
                currentPanel.SetActive(true);
                break;
            case 5:
                currentPanel.SetActive(false);
                currentPanel = weightPanel;
                currentPanel.SetActive(true);
                break;
            case 6:
                currentPanel.SetActive(false);
                currentPanel = categoryPanel;
                currentPanel.SetActive(true);
                break;
            case 7:
                currentPanel.SetActive(false);
                currentPanel = savePaenl;
                currentPanel.SetActive(true);
                break;
            case 8:
                currentPanel.SetActive(false);
                currentPanel = sortByDatePanel;
                currentPanel.SetActive(true);
                break;
            case 9:
                currentPanel.SetActive(false);
                currentPanel = sortByCategoryPanel;
                currentPanel.SetActive(true);
                break;
            case 10:
                currentPanel.SetActive(false);
                currentPanel = addCategoryPanel;
                currentPanel.SetActive(true);
                break;
            case 11:
                currentPanel.SetActive(false);
                currentPanel = levelRewardsPanel;
                currentPanel.SetActive(true);
                break;
            case 12:
                currentPanel.SetActive(false);
                currentPanel = levelExpPanel;
                currentPanel.SetActive(true);
                break;
            case 13:
                currentPanel.SetActive(false);
                currentPanel = levelLevelPanel;
                currentPanel.SetActive(true);
                nextButton.interactable = true;
                break;
            case 14:
                currentPanel.SetActive(false);
                currentPanel = finalPanel;
                currentPanel.SetActive(true);
                nextButton.interactable = false;
                break;
        }
    }
}
