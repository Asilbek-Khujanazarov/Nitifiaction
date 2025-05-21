using PatientRecovery.NotificationService.Models;

namespace PatientRecovery.NotificationService.DTOs
{
    public class CreateNotificationTemplateRequest
    {
        public string Name { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public NotificationType Type { get; set; }
    }

    public class UpdateNotificationTemplateRequest
    {
        public Guid Id { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public bool IsActive { get; set; }
    }
}