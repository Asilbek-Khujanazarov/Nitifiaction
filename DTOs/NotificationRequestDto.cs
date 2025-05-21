using PatientRecovery.NotificationService.Models;

namespace PatientRecovery.NotificationService.DTOs
{
    public class CreateNotificationRequest
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public string RecipientId { get; set; }
        public string? RecipientEmail { get; set; }
        public string? RecipientPhone { get; set; }
        public NotificationType Type { get; set; }
        public NotificationPriority Priority { get; set; }
    }

    public class UpdateNotificationRequest
    {
        public NotificationStatus Status { get; set; }
        public bool IsRead { get; set; }
    }
}