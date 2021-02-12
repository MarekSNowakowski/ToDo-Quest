using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TranslationManager : MonoBehaviour
{
    [SerializeField]
    SettingsManager settings;

    public string GetStaticString(int ID)
    {
        switch(settings.GetLanguage())
        {
            case "pl":
                switch (ID)
                {
                    case 0:
                        return "Nadchodzące";
                    case 1:
                        return "Kategorie";
                    case 2:
                        return "Nagrody";
                }
                break;
            default:
                switch (ID)
                {
                    case 0:
                        return "Incoming";
                    case 1:
                        return "Categories";
                    case 2:
                        return "Rewards";
                }
                break;
        }

        return null;
    }
}
