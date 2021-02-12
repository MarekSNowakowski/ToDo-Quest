using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CycleManager : MonoBehaviour
{
    private int repeatCycle;

    [SerializeField]
    TMPro.TextMeshProUGUI cycleText;
    [SerializeField]
    TranslationManager translationManager;

    public void OnNextPress()
    {
        switch(repeatCycle)
        {
            case 0:
                SetUp(1);
                break;
            case 1:
                SetUp(7);
                break;
            case 7:
                SetUp(14);
                break;
            case 14:
                SetUp(30);
                break;
            case 30:
                SetUp(0);
                break;
        }
    }

    public void OnPreviousPress()
    {
        switch (repeatCycle)
        {
            case 0:
                SetUp(30);
                break;
            case 1:
                SetUp(0);
                break;
            case 7:
                SetUp(1);
                break;
            case 14:
                SetUp(7);
                break;
            case 30:
                SetUp(14);
                break;
        }
    }

    public int SubmitCycle()
    {
        int tmp = repeatCycle;
        Clear();
        return tmp;
    }

    public void Clear()
    {
        SetUp(0);
    }

    public void SetUp(int repeatCycle)
    {
        this.repeatCycle = repeatCycle;
        switch(repeatCycle)
        {
            case 0:
                cycleText.text = "-";
                break;
            case 1:
                cycleText.text = translationManager.GetStaticString(26);
                break;
            case 7:
                cycleText.text = translationManager.GetStaticString(27);
                break;
            case 14:
                cycleText.text = translationManager.GetStaticString(28);
                break;
            case 30:
                cycleText.text = translationManager.GetStaticString(29);
                break;
        }
    }
}
