using System;
using System.ComponentModel.DataAnnotations;

namespace PatientRecovery.NotificationService.Models
{
    public class Notification
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Message { get; set; }

        [Required]
        public string RecipientId { get; set; }

        public string? RecipientEmail { get; set; }

        public string? RecipientPhone { get; set; }

        [Required]
        public NotificationType Type { get; set; }

        [Required]
        public NotificationPriority Priority { get; set; }

        [Required]
        public NotificationStatus Status { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime? SentAt { get; set; }

        public DateTime? DeliveredAt { get; set; }

        public DateTime? ReadAt { get; set; }

        public bool IsRead { get; set; }

        public string? ErrorMessage { get; set; }
    }
}