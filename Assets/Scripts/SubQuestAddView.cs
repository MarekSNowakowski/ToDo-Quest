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

    // Start is called before the first frame update
    void Start()
    {
        myRectTransform = GetComponent<RectTransform>();
        CheckIfKeyboardOpened();
    }

    void OnEnable()
    {
        nameInputField.ActivateInputField();
        nameInputField.Select();
    }

    private void OnDisable()
    {
        nameInputField.text = "";
    }

    IEnumerator WaitForKeyboardToOpen()
    {
        float waitingTime = 0.5f;
        yield return new WaitForSeconds(waitingTime);
        keyboardHeight = GetKeyboardHeight();
        myRectTransform.anchoredPosition = new Vector2(0, keyboardHeight);
    }

    public void CheckIfKeyboardOpened()
    {
        if (keyboardHeight < 200)
        {
            StartCoroutine(WaitForKeyboardToOpen());
        }
    }

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
