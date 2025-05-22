using Microsoft.AspNetCore.SignalR;
using PatientRecovery.NotificationService.Services;
using PatientRecovery.NotificationService.Models;

namespace PatientRecovery.NotificationService.Hubs
{
    public class ChatHub : Hub
    {
        private readonly IChatService _chatService;
        private readonly ILogger<ChatHub> _logger;

        public ChatHub(IChatService chatService, ILogger<ChatHub> logger)
        {
            _chatService = chatService;
            _logger = logger;
        }

        public async Task JoinChat(string userId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, userId);
            _logger.LogInformation("User {UserId} joined chat", userId);
        }

        public async Task LeaveChat(string userId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, userId);
            _logger.LogInformation("User {UserId} left chat", userId);
        }

        public async Task SendMessage(string senderId, string receiverId, string content, string senderRole)
        {
            try
            {
                var message = new ChatMessage
                {
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    Content = content,
                    Timestamp = DateTime.UtcNow,
                    SenderRole = senderRole,
                    IsRead = false
                };

                await _chatService.SaveMessageAsync(message);
                await Clients.Group(receiverId).SendAsync("ReceiveMessage", message);
                
                _logger.LogInformation("Message sent from {SenderId} to {ReceiverId}", senderId, receiverId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending message from {SenderId} to {ReceiverId}", senderId, receiverId);
                throw;
            }
        }

        public async Task MarkMessageAsRead(Guid messageId)
        {
            await _chatService.MarkMessageAsReadAsync(messageId);
        }
    }
}