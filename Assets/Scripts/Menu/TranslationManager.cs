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
                        //Settings
                    case 31:
                        return "Bazowe punkty doświadczenia:";
                    case 32:
                        return "Przypomnij o deadlinie o godzinie:";
                    case 33:
                        return "Język:";
                    //About
                    case 34:
                        return "Autor:";
                    case 35:
                        return "O aplikacji:";
                    case 36:
                        return "ToDo Quest to planer który pomoże ci zorganizować i zaplanować zadania. Dzięki systemowi nagród, możesz zmotywować się poprzez wyznaczanie sobie celów i nagradzanie się za ich realizację. Używaj wygodnych funkcji takich jak dodawanie komentarzy, ustawianie deadlinu i przypomnień, tworzenie zadań cyklicznych czy sortowanie.";
                    case 37:
                        return "Wspomóż autora";
                    case 38:
                        return "E-mail do kontaktu:";
                    case 39:
                        return "TAK";
                    case 40:
                        return "ANULUJ";
                    case 41:
                        return "Odrzucić zmiany?";
                    case 42:
                        return "Czy chcesz usunąć to zadanie?";
                    case 43:
                        return "Czy chcesz usunąć tę kategorię?";
                    //Tutorial
                    case 44:
                        return "Naciśnij ten przycisk, aby rozwinąć menu.";
                    case 45:
                        return "Naciśnij ten przycisk, aby otworzyć panel dodawania zadań.";
                    case 46:
                        return "Dodaj nazwę zadania.";
                    case 47:
                        return "Tutaj możesz dodać nagrodę za wykonanie zadania.";
                    case 48:
                        return "Kliknij tu, aby dodać komentarz.";
                    case 49:
                        return "Wybierz datę wykonania zadania. Wybrana data oznaczona jest na <color=red>czerwono</color>, aktualna data na <color=green>zielono</color>. Potwierdź datę klikając ikonę zapisu bądź odrzuć klikając strzałkę cofania.";
                    case 50:
                        return "Ten dolny pasek pozwala wybrać, czy zadanie ma być powtarzane.";
                    case 51:
                        return "Naciśnij tutaj, aby otworzyć kalendarz i dodać datę.";
                    case 52:
                        return "2021\nKwiecień";
                    case 53:
                        return "2021\nCzerwiec";
                    case 54:
                        return "Ten dolny pasek pozwala wybrać czy po przekroczeniu daty zadanie ma być usunięte automatycznie i czy chcesz dostać powiadomienie o danym zadaniu w wybranym dniu.";
                    case 55:
                        return "Naciśnij tutaj, aby otworzyć kalendarz i dodać deadline.";
                    case 56:
                        return "Kliknij tutaj, aby zmienić wagę zadania. Biały jest najmniej ważny, <color=green>zielony</color> jest istotny, <color=blue>niebieski</color> jest ważny i <color=red>czerwony</color> jest do najważniejszych zadań.";
                    case 57:
                        return "Kliknij tutaj, by wybrać między istniejącymi kategoriami.";
                    case 58:
                        return "Kiedy zadanie jest gotowe, wciśnij przycisk zapisu.";
                    case 59:
                        return "Kiedy jesteś na stronie \"Nadchodzące\", zadania są sortowane w zależności od daty.";
                    case 60:
                        return "Możesz otworzyć detale zadania klikając na nie. Zobaczysz tam wszelkie informacje o zadaniu, a także możesz edytować je lub usunąć.";
                    case 61:
                        return "Możesz zmienić na sortowanie w zależności od kategorii, klikając tutaj bądź przesuwając palcem po ekranie.";
                    case 62:
                        return "Otwórz detale kategorii, klikając na jej etykietę. Poprzez ten panel możesz edytować lub usunąć kategorię.";
                    case 63:
                        return "Klikając w ten przycisk otwierasz panel tworzenia kategorii.";
                    case 64:
                        return "Dodaj nazwę kategorii, wybierz kolor i wciśnij ikonę zapisu, aby stworzyć kategorię. Kolor musi być unikalny dla każdej kategorii.";
                    case 65:
                        return "Nagrody za wykonane zadania czekają na ciebie tutaj. Odbierz je klikając w okrąg.";
                    case 66:
                        return "Zdobywaj doświadczenie wykonując zadania. Doświadczenie jest przyznawane w zależności od wagi zadania. Ilość doświadczenia bazowego możesz zmienić w ustawieniach. Możesz również dodać dodatkowe doświadczenie pisząc zamiast nagrody \" + [liczba]\", np. \" + 6\".";
                    case 67:
                        return "Naciśnij tutaj, aby dodać nagrodę za level. Kiedy awansujesz, nagroda zostanie automatycznie dodana.";
                    case 68:
                        return "Naciśnij tutaj, aby zakończyć samouczek";
                    case 69:
                        return "Dziękuję za ukończenie samouczka!";
                    case 70:
                        return "Czy chcesz rozpocząć samouczek?";
                    case 71:
                        return "NIE";
                    case 72:
                        return "Maksymalny rozmiar archiwum:";
                    case 73:
                        return "Przywróc domyślne";
                    case 74:
                        return "Archiwum";
                    case 75:
                        return "Dodaj podzadanie";
                    case 76:
                        return "Wczoraj";
                    case 77:
                        return "Większy przycisk dodawania";
                    case 78:
                        return "Czy chcesz ocenić tą aplikację?";
                    case 79:
                        return "Statystyki";
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
                        return "Add category";
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
                        return "Tomorrow";
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
                        //Settings
                    case 31:
                        return "Base experience points:";
                    case 32:
                        return "Remind about the deadline at:";
                    case 33:
                        return "Language:";
                    //About
                    case 34:
                        return "Author:";
                    case 35:
                        return "About the app:";
                    case 36:
                        return "ToDo Quest is a basic planner that helps you to organize your tasks and make plans for the future. Thanks to reward system, you can motivate yourself by creating goals and declaring how you will reward yourself for completing them. Use comfortable features including writing comments, setting deadlines and remainders, creating cyclic quests, or sorting tasks by category.";
                    case 37:
                        return "Donate the creator";
                    case 38:
                        return "Contact email:";
                    //Discard panels
                    case 39:
                        return "YES";
                    case 40:
                        return "CANCEL";
                    case 41:
                        return "Discard changes?";
                    case 42:
                        return "Do you want to remove this task?";
                    case 43:
                        return "Do you want to remove this category?";
                    //Tutorial
                    case 44:
                        return "Click the menu button to slide the menu.";
                    case 45:
                        return "Click the add task button to open add panel.";
                    case 46:
                        return "Here add name of the task you want to add.";
                    case 47:
                        return "Here you can add the reward you will get for completing the task.";
                    case 48:
                        return "Click here to add comment.";
                    case 49:
                        return "Choose date you want to do the task on. Selected date is marked <color=red>red</color>, current is marked with <color=green>green</color> color. Click save icon to apply or back to discard.";
                    case 50:
                        return "This bottom bar allows you to decide if you want to repeat task.";
                    case 51:
                        return "Click here to open calendar and add task date.";
                    case 52:
                        return "2021\nApril";
                    case 53:
                        return "2021\nJune";
                    case 54:
                        return "This bottom bar allows you to decide if task after deadline shall be automatically removed and if you want to receive notification at that date.";
                    case 55:
                        return "Click here to open calendar and add task deadline.";
                    case 56:
                        return "Click here to change importance of the task. White is least important, <color=green>green</color> is normal, <color=blue>blue</color> is important and <color=red>red</color> is for very important things.";
                    case 57:
                        return "Click here to choose between your categories.";
                    case 58:
                        return "When task is ready to add, click here to save it.";
                    case 59:
                        return "When you are at incoming page, quests are sorted by date and organized with labels.";
                    case 60:
                        return "You can open task details by clicking on it. There you can see all information about it, edit the task or remove it.";
                    case 61:
                        return "You can change to sorting by category by clicking here or swiping.";
                    case 62:
                        return "Open category details by clicking on its label. There you will be able to edit or remove it.";
                    case 63:
                        return "You can open category creation panel by clicking this button.";
                    case 64:
                        return "Add category name, choose color and click save icon to create category. The colors must be unique for each category.";
                    case 65:
                        return "Rewards for completed quests are waiting for you here. You can claim them by pressing circle icon.";
                    case 66:
                        return "Earn experience by completing quests. Experience rewards are given based on task importance. You can change the base amount in the settings. You can also add additional experience by writing instead of reward \" + [number]\", e.g. \" + 6\".";
                    case 67:
                        return "Click here to add reward for the level. When you level up, the reward will be added automatically.";
                    case 68:
                        return "Click here to end tutorial.";
                    case 69:
                        return "Thank you for completing tutorial!";
                    case 70:
                        return "Do you want to start the tutorial?";
                    case 71:
                        return "NO";
                    case 72:
                        return "Archive maximum size:";
                    case 73:
                        return "Restore to default";
                    case 74:
                        return "Archive";
                    case 75:
                        return "Add subtask";
                    case 76:
                        return "Yesterday";
                    case 77:
                        return "Floating add button";
                    case 78:
                        return "Do you want to rate this app?";
                    case 79:
                        return "Statistics";
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
