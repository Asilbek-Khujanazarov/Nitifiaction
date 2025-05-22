namespace PatientRecovery.NotificationService.DTOs
{
    public class ChatMessageDto
    {
        public Guid Id { get; set; }
        public string SenderId { get; set; } = null!;
        public string ReceiverId { get; set; } = null!;
        public string Content { get; set; } = null!;
        public DateTime Timestamp { get; set; }
        public bool IsRead { get; set; }
        public string SenderRole { get; set; } = null!;
        public string SenderName { get; set; } = null!;
    }

    public class SendMessageRequest
    {
        public string ReceiverId { get; set; } = null!;
        public string Content { get; set; } = null!;
    }
}