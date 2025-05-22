using Microsoft.AspNetCore.SignalR;
using NotificationService.Services;
using NotificationService.Models;
using NotificationService.Enums;

namespace NotificationService.Hubs
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

        public async Task SendMessage(string senderId, string receiverId, string content, UserType senderType)
        {
            try
            {
                var message = new ChatMessage
                {
                    Id = Guid.NewGuid(),
                    SenderId = senderId,
                    ReceiverId = receiverId,
                    Content = content,
                    SenderType = senderType,
                    Timestamp = DateTime.UtcNow,
                    IsRead = false
                };

                await _chatService.SaveMessageAsync(message);
                await Clients.Group(receiverId).SendAsync("ReceiveMessage", message);
                
                _logger.LogInformation(
                    "Message sent from {SenderId} to {ReceiverId}", 
                    senderId, receiverId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, 
                    "Error sending message from {SenderId} to {ReceiverId}", 
                    senderId, receiverId);
                throw;
            }
        }

        public async Task MarkAsRead(Guid messageId)
        {
            await _chatService.MarkMessageAsReadAsync(messageId);
            await Clients.All.SendAsync("MessageRead", messageId);
        }

        public override async Task OnConnectedAsync()
        {
            _logger.LogInformation("Client connected: {ConnectionId}", Context.ConnectionId);
            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? exception)
        {
            _logger.LogInformation("Client disconnected: {ConnectionId}", Context.ConnectionId);
            await base.OnDisconnectedAsync(exception);
        }
    }
}