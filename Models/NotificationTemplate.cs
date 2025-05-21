using System;
using System.ComponentModel.DataAnnotations;

namespace PatientRecovery.NotificationService.Models
{
    public class NotificationTemplate
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public string Subject { get; set; }

        [Required]
        public string Body { get; set; }

        public NotificationType Type { get; set; }

        public bool IsActive { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? UpdatedAt { get; set; }
    }
}