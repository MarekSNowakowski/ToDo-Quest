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
        StartCoroutine(WaitForKeyboardToOpen());
    }

    void OnEnable()
    {
        nameInput.ActivateInputField();
        nameInput.Select();
        CheckIfKeyboardOpened();
    }

    IEnumerator WaitForKeyboardToOpen()
    {
        float waitingTime = 0.5f;
        yield return new WaitForSeconds(waitingTime);
        keyboardHeight = GetKeyboardHeight();
        addPanel.anchoredPosition = new Vector2(0, keyboardHeight);
        keyboardPlaceholder.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, keyboardHeight + (0.046f * Screen.height));
    }

    public void CheckIfKeyboardOpened()
    {
        if (keyboardHeight < 200)
        {
            StartCoroutine(WaitForKeyboardToOpen());
        }
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


    /// <summary>
    /// Returns the keyboard height in display pixels.
    /// </summary>
    public int GetKeyboardHeight()
    {
#if UNITY_EDITOR
        return 685;
#endif
#if UNITY_ANDROID
        using (var unityClass = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            var unityPlayer = unityClass.GetStatic<AndroidJavaObject>("currentActivity").Get<AndroidJavaObject>("mUnityPlayer");
            var view = unityPlayer.Call<AndroidJavaObject>("getView");
            var dialog = unityPlayer.Get<AndroidJavaObject>("mSoftInputDialog");

            if (view == null || dialog == null)
                return 0;

            var decorHeight = 0;

            /*if (includeInput)
            {
                var decorView = dialog.Call<AndroidJavaObject>("getWindow").Call<AndroidJavaObject>("getDecorView");

                if (decorView != null)
                    decorHeight = decorView.Call<int>("getHeight");
            }*/

            using (var rect = new AndroidJavaObject("android.graphics.Rect"))
            {
                view.Call("getWindowVisibleDisplayFrame", rect);
                return (int)screenHeight - rect.Call<int>("height") + decorHeight;
            }
        }
#else
        var height = Mathf.RoundToInt(TouchScreenKeyboard.area.height);
        return height >= Display.main.systemHeight ? 0 : height;
#endif
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
