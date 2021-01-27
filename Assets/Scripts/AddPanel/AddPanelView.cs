using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Input;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class AddPanelView : MonoBehaviour
{
    [SerializeField]
    RectTransform addPanel;

    [SerializeField]
    TMPro.TMP_InputField nameInput;
    [SerializeField]
    TMPro.TMP_InputField rewardInput;

    [SerializeField]
    GameObject mainAddPanel;
    [SerializeField]
    GameObject commentAddPanel;
    [SerializeField]
    GameObject datePanel;
    [SerializeField]
    GameObject deadlinePanel;

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

    bool keyboardActive;


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
        if (keyboardHeight == 0)
        {
            StartCoroutine(WaitForKeyboardToOpen());
        }
        OnKeyboardOpen();
    }

    IEnumerator WaitForKeyboardToOpen()
    {
        float waitingTime = 0.25f;
        yield return new WaitForSeconds(waitingTime);
        keyboardHeight = GetKeyboardHeight();
        Debug.Log("Keyboard size: " + keyboardHeight);
        addPanel.anchoredPosition = new Vector2(0, keyboardHeight);
        keyboardActive = true;
    }

    public void Close()
    {
        mainAddPanel.SetActive(true);
        commentAddPanel.SetActive(false);
        CloseDatePanel();
        CloseDeadlinePanel();
        discardPaenl.SetActive(false);
        this.gameObject.SetActive(false);
    }

    public void TryClose()
    {
        discardPaenl.SetActive(true);
    }

    public void CancelClose()
    {
        discardPaenl.SetActive(false);
    }

    public void OnKeyboardClose()
    {
        addPanel.anchoredPosition = new Vector2(0, 0);
        keyboardActive = false;
    }

    public void OnKeyboardOpen()
    {
        if (!keyboardActive)
        {
            addPanel.anchoredPosition = new Vector2(0, keyboardHeight);
            keyboardActive = true;
        }
    }

    /// <summary>
    /// Returns the keyboard height in display pixels.
    /// </summary>
    public int GetKeyboardHeight()
    {
#if UNITY_EDITOR
        return 0;
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
        datePanel.SetActive(true);
    }

    public void CloseDatePanel()
    {
        datePanel.SetActive(false);
    }

    public void OpenDeadlinePanel()
    {
        deadlinePanel.SetActive(true);
    }

    public void CloseDeadlinePanel()
    {
        deadlinePanel.SetActive(false);
    }
}
