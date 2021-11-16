using System.Collections;
using UnityEngine;
using System.Threading.Tasks;

public class AddPanelView : MonoBehaviour
{
    [SerializeField]
    RectTransform addPanel;

    [SerializeField]
    RectTransform keyboardPlaceholder;

    [SerializeField]
    TMPro.TMP_InputField nameInput;
    [SerializeField]
    TMPro.TMP_InputField rewardInput;

    [SerializeField]
    BlockerFade blocker;

    [SerializeField]
    GameObject mainAddPanel;
    [SerializeField]
    GameObject commentAddPanel;
    [SerializeField]
    GameObject datePanel;
    [SerializeField]
    GameObject deadlinePanel;
    [SerializeField]
    GameObject categoriesPanel;

    [Header("CommentPanel")]
    [SerializeField]
    GameObject commentPanel;
    [SerializeField]
    TMPro.TMP_InputField commentFeild;

    [Header("Discard")]
    [SerializeField]
    GameObject discardPaenl;


    float screenHeight = Screen.height;
    float keyboardHeight = 0;


    void Start()
    {
        nameInput.ActivateInputField();
        nameInput.Select();
    }

    void OnEnable()
    {
        nameInput.ActivateInputField();
        nameInput.Select();
    }

    public void Close()
    {
        mainAddPanel.SetActive(true);
        commentAddPanel.SetActive(false);
        datePanel.SetActive(false);
        deadlinePanel.SetActive(false);
        categoriesPanel.SetActive(false);
        keyboardPlaceholder.gameObject.SetActive(false);
        discardPaenl.SetActive(false);
        blocker.DisableBlocker(this.gameObject);
        //this.gameObject.SetActive(false);
    }

    public void TryClose()
    {
        discardPaenl.SetActive(true);
    }

    public void CancelClose()
    {
        discardPaenl.SetActive(false);
    }

    public void selectRewardInputField()
    {
        rewardInput.ActivateInputField();
        rewardInput.Select();
    }

    public void OpenCommentPanel(string comment)
    {
        commentFeild.text = comment;
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
        nameInput.ActivateInputField();
        nameInput.Select();
    }

    public void OpenDatePanel()
    {
        keyboardPlaceholder.gameObject.SetActive(true);
        datePanel.SetActive(true);
    }

    public void CloseDatePanel()
    {
        datePanel.SetActive(false);
        keyboardPlaceholder.gameObject.SetActive(false);
    }

    public void OpenDeadlinePanel()
    {
        keyboardPlaceholder.gameObject.SetActive(true);
        deadlinePanel.SetActive(true);
    }

    public void CloseDeadlinePanel()
    {
        deadlinePanel.SetActive(false);
        keyboardPlaceholder.gameObject.SetActive(false);
    }

    public void OpenCategoryPanel()
    {
        keyboardPlaceholder.gameObject.SetActive(true);
        categoriesPanel.SetActive(true);
    }

    public void CloseCategoryPanel()
    {
        categoriesPanel.SetActive(false);
        keyboardPlaceholder.gameObject.SetActive(false);
    }
}
