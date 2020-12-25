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

    [Header("CommentPanel")]
    [SerializeField]
    GameObject commentPanel;
    [SerializeField]
    TMPro.TMP_InputField commentFeild;


    float screenHeight = Screen.height;

    int counter;


    void Start()
    {
        nameInput.ActivateInputField();
        nameInput.Select();
        StartCoroutine(waitForKeyboardToOpen());
    }

    void OnEnable()
    {
        counter = 0;
        nameInput.ActivateInputField();
        nameInput.Select();
    }

    IEnumerator waitForKeyboardToOpen()
    {
        float waitingTime = 0.25f;
        yield return new WaitForSeconds(waitingTime);
        int keyboardSize = GetKeyboardHeight();
        addPanel.anchoredPosition = new Vector2(0, keyboardSize);
    }

    public void Close()
    {
        mainAddPanel.SetActive(true);
        commentAddPanel.SetActive(false);
        datePanel.SetActive(false);
        this.gameObject.SetActive(false);
    }

    private void Update()
    {
        //counter++;
        //if (counter % 100 == 0 && !nameInput.isFocused && !rewardInput.isFocused)
        //{
        //    Close();
        //}
        //if (counter > 9999) counter = 101;
    }

    /// <summary>
    /// Returns the keyboard height in display pixels.
    /// </summary>
    public static int GetKeyboardHeight()
    {
#if UNITY_EDITOR
        return 1;
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
                return Display.main.systemHeight - rect.Call<int>("height") + decorHeight;
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
}
