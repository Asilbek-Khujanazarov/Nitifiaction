namespace PatientRecovery.NotificationService.Models
{
    public enum NotificationType
    {
        Email,
        SMS,
        PushNotification,
        InApp
    }

    public enum NotificationPriority
    {
        Low,
        Medium,
        High,
        Critical
    }

    public enum NotificationStatus
    {
        Pending,
        Sent,
        Failed,
        Delivered,
        Read
    }
}