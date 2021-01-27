using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SettingsManager : MonoBehaviour
{
    [SerializeField]
    private int[] questCompleteExp = { 1, 2, 5, 10 };

    public int[] GetQuestCompleteExp()
    {
        return questCompleteExp;
    }

}
