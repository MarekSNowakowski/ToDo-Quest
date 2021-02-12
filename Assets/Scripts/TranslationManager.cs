using System.Collections;
using System.Collections.Generic;
using System.Globalization;
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
                        return "Zaległe";
                    case 2:
                        return "Inne";
                    case 3:
                        return "Kategorie";
                    case 4:
                        return "Dodaj kategorię";
                    case 5:
                        return "Nagrody";
                    case 6:
                        return "Nagroda za następny poziom";
                    case 7:
                        return "Dodaj swoją nagrodę!";
                    case 8:
                        return "Odbierz nagrody";
                    //Calendar
                    case 9:
                        return "Pon";
                    case 10:
                        return "Wt";
                    case 11:
                        return "Śr";
                    case 12:
                        return "Czw";
                    case 13:
                        return "Pt";
                    case 14:
                        return "Sob";
                    case 15:
                        return "Niedz";
                    case 16:
                        return "Powtarzaj co:";
                    case 17:
                        return "Przypomnij:";
                    case 18:
                        return "Usuń po terminie:";
                    case 19:
                        return "Brak kategorii";
                    case 20:
                        return "Codziennie";
                    case 21:
                        return "Ustawienia";
                    case 22:
                        return "O aplikacji";
                    case 23:
                        return "Pomoc";
                    case 24:
                        return "Dzisiaj";
                    case 25:
                        return "Jutro";
                    case 26:
                        return "dzień";
                    case 27:
                        return "tydzień";
                    case 28:
                        return "2 tygodnie";
                    case 29:
                        return "miesiąc";
                    case 30:
                        return "Co";
                    default:
                        Debug.LogWarning("Translation with id " + ID + " not found");
                        return "";
                }
            default:
                switch (ID)
                {
                    case 0:
                        return "Incoming";
                    case 1:
                        return "Overdue";
                    case 2:
                        return "Other";
                    case 3:
                        return "Categories";
                    case 4:
                        return "Add cathegory";
                    case 5:
                        return "Rewards";
                    case 6:
                        return "Next level reward";
                    case 7:
                        return "Add your reward!";
                    case 8:
                        return "Claim your rewards";
                    //Calendar
                    case 9:
                        return "Mon";
                    case 10:
                        return "Tue";
                    case 11:
                        return "Wed";
                    case 12:
                        return "Thu";
                    case 13:
                        return "Fri";
                    case 14:
                        return "Sat";
                    case 15:
                        return "Sun";
                    case 16:
                        return "Repeat every:";
                    case 17:
                        return "Remind me:";
                    case 18:
                        return "Auto remove:";
                    case 19:
                        return "No category";
                    case 20:
                        return "Every day";
                    case 21:
                        return "Settings";
                    case 22:
                        return "About";
                    case 23:
                        return "Help";
                    case 24:
                        return "Today";
                    case 25:
                        return "Tommorow";
                    case 26:
                        return "day";
                    case 27:
                        return "week";
                    case 28:
                        return "2 weeks";
                    case 29:
                        return "month";
                    case 30:
                        return "Every";
                    default:
                        Debug.LogWarning("Translation with id " + ID + " not found");
                        return "";
                }
        }
    }

    public CultureInfo GetCultureInfo()
    {
        switch(settings.GetLanguage())
        {
            case "pl":
                return new CultureInfo("pl-PL");
            default:
                return new CultureInfo("en-US");
        }
    }
}
