using System.ComponentModel.DataAnnotations;
using NotificationService.Enums;

namespace NotificationService.Models
{
    public class ChatMessage
    {
        public Guid Id { get; set; }

        [Required]
        public string SenderId { get; set; } = null!;

        [Required]
        public string ReceiverId { get; set; } = null!;

        [Required]
        public string Content { get; set; } = null!;

        public DateTime Timestamp { get; set; }

        public bool IsRead { get; set; }

        [Required]
        public UserType SenderType { get; set; }
    }
}