using System;
using Unity.Notifications.Android;
using UnityEngine;

public class NotificationManager : MonoBehaviour
{
    private int notificationHour = 9;

    private void Start()
    {
        var channel = new AndroidNotificationChannel()
        {
            Id = "channel_deadline",
            Name = "Deadline Channel",
            Importance = Importance.Default,
            Description = "Deadline notifications",
        };
        AndroidNotificationCenter.RegisterNotificationChannel(channel);
    }

    public void ShedudleNotification(string title, DateTime notificationDay, int notificationID)
    {
        var notification = new AndroidNotification();
        notification.Title = title;
        notification.Text = "Today is the deadline!";
        notification.FireTime = notificationDay.AddHours(notificationHour);
        notification.SmallIcon = "app_icon_small";
        notification.LargeIcon = "app_icon_large";

        AndroidNotificationCenter.SendNotificationWithExplicitID(notification, "channel_deadline", notificationID);
    }

    public void CancelNotification(int notifactionID)
    {
        AndroidNotificationCenter.CancelNotification(notifactionID);
    }
}
