using OneSignalSDK;
using UnityEngine;

public class OneSignalInitializer : MonoBehaviour
{

    private void Start()
    {
        // Enable lines below to debug issues with OneSignal
        OneSignal.Default.LogLevel = LogLevel.Info;
        OneSignal.Default.AlertLevel = LogLevel.Fatal;

        // Setup the below to listen for and respond to events from notifications
        OneSignal.Default.NotificationOpened += _notificationOpened;
        OneSignal.Default.NotificationWillShow += _notificationReceived;

        OneSignal.Default.Initialize("ccb9a835-c4c0-47e5-9010-3df303938e31");
    }

    private void _log(object message)
    {
        Debug.Log(message);
    }

    private void _notificationOpened(NotificationOpenedResult result)
    {
        _log($"Notification was opened with result: {JsonUtility.ToJson(result)}");
    }

    private Notification _notificationReceived(Notification notification)
    {
        var additionalData = notification.additionalData != null
            ? Json.Serialize(notification.additionalData)
                : null;

        _log($"Notification was received in foreground: {JsonUtility.ToJson(notification)}\n{additionalData}");
        return notification; // show the notification
    }
}