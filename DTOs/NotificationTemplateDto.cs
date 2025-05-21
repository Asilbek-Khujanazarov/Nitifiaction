using System;
using PatientRecovery.NotificationService.Models;

namespace PatientRecovery.NotificationService.DTOs
{
    public class CreateNotificationTemplateRequest
    {
        public string Name { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
        public NotificationType Type { get; set; }
    }

    public class UpdateNotificationTemplateRequest
    {
        public Guid Id { get; set; }
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
        public bool IsActive { get; set; }
    }

    public class NotificationTemplateDto
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = null!;
        public string Subject { get; set; } = null!;
        public string Body { get; set; } = null!;
        public NotificationType Type { get; set; }
        public bool IsActive { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}