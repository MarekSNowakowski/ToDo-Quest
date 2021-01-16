using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SettingsManager",menuName = "Settings Manager")]
public class SettingsManager : ScriptableObject
{
    [SerializeField]
    private int[] questCompleteExp = { 1, 2, 5, 10 };

    public int[] GetQuestCompleteExp()
    {
        return questCompleteExp;
    }

}
