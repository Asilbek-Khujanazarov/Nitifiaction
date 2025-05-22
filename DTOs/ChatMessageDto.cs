using NotificationService.Enums;

namespace NotificationService.DTOs
{
    public class ChatMessageDto
    {
        public Guid Id { get; set; }
        public string SenderId { get; set; } = null!;
        public string ReceiverId { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
        public UserType SenderType { get; set; }
        public string? SenderName { get; set; }
    }

    public class SendMessageRequest
    {
        public string ReceiverId { get; set; } = null!;
        public string Content { get; set; } = null!;
        public UserType SenderType { get; set; }
    }
}