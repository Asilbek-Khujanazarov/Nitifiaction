using PatientRecovery.NotificationService.Models;

namespace PatientRecovery.NotificationService.DTOs
{
    public class BulkNotificationRequest
    {
        public string Title { get; set; }
        public string Message { get; set; }
        public List<string> RecipientIds { get; set; }
        public NotificationType Type { get; set; }
        public NotificationPriority Priority { get; set; }
    }

    public class BulkNotificationResultDto
    {
        public int TotalRecipients { get; set; }
        public int SuccessCount { get; set; }
        public int FailureCount { get; set; }
        public List<string> FailedRecipientIds { get; set; }
        public List<string> ErrorMessages { get; set; }
    }
}