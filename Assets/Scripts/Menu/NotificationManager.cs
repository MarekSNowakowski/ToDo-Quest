using System;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    [SerializeField]
    SettingsManager settings;
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
        notification.Text = "Today is the deadline!";
        notification.FireTime = notificationDay.AddHours(settings.GetNotificationHour()).AddMinutes(settings.GetNotificationMinutes());
        notification.SmallIcon = "app_icon_small";
        notification.LargeIcon = "app_icon_large";

        AndroidNotificationCenter.SendNotificationWithExplicitID(notification, "channel_todoquest", notificationID);
    }

    public void ShedudleRemindNotification(int notificationID)
    {
        var notification = new AndroidNotification();
        notification.Title = "We miss you!";
        notification.Text = "You haven't visited our app in 3 days";
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
