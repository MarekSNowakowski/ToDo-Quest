using System;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    [SerializeField]
    SettingsManager settings;
    [SerializeField]
    TranslationManager translationManager;
    private int remindNotificationID = 0;

    private void Start()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_todoquest",
            Name = "ToDo Quest Channel",
            Importance = Importance.Default,
            Description = "ToDo Quest notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
        CancelNotification(remindNotificationID);
        ShedudleRemindNotification(remindNotificationID);
    }

    public void ShedudleNotification(string title, DateTime notificationDay, int notificationID)
    {
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = translationManager.GetStaticString(79);
        notification.FireTime = notificationDay.AddHours(settings.GetNotificationHour()).AddMinutes(settings.GetNotificationMinutes());
        notification.SmallIcon = "app_icon_small";
        notification.LargeIcon = "app_icon_large";

        AndroidNotificationCenter.SendNotificationWithExplicitID(notification, "channel_todoquest", notificationID);
    }

    public void ShedudleRemindNotification(int notificationID)
    {
        var notification = new AndroidNotification();
        notification.Title = translationManager.GetStaticString(80);
        notification.Text = translationManager.GetStaticString(81);
        notification.FireTime = DateTime.Today.AddDays(3).AddHours(settings.GetNotificationHour()).AddMinutes(settings.GetNotificationMinutes());
        notification.SmallIcon = "app_icon_small";
        notification.LargeIcon = "app_icon_large";

        AndroidNotificationCenter.SendNotificationWithExplicitID(notification, "channel_todoquest", notificationID);
    }

    public void CancelNotification(int notifactionID)
    {
        AndroidNotificationCenter.CancelNotification(notifactionID);
    }
}
