using Microsoft.EntityFrameworkCore;
using NotificationService.Data;
using NotificationService.Models;
using NotificationService.DTOs;

namespace NotificationService.Services
{
    public class ChatService : IChatService
    {
        private readonly ChatDbContext _context;
        private readonly ILogger<ChatService> _logger;

        public ChatService(ChatDbContext context, ILogger<ChatService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ChatMessage> SaveMessageAsync(ChatMessage message)
        {
            try
            {
                message.Timestamp = DateTime.UtcNow;
                await _context.ChatMessages.AddAsync(message);
                await _context.SaveChangesAsync();

                _logger.LogInformation(
                    "Message saved: {MessageId} from {SenderId} to {ReceiverId}", 
                    message.Id, message.SenderId, message.ReceiverId);

                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving chat message");
                throw;
            }
        }

        public async Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync(
            string userId1, string userId2, int skip = 0, int take = 50)
        {
            return await _context.ChatMessages
                .Where(m => (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                           (m.SenderId == userId2 && m.ReceiverId == userId1))
                .OrderByDescending(m => m.Timestamp)
                .Skip(skip)
                .Take(take)
                .Select(m => new ChatMessageDto
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Content = m.Content,
                    Timestamp = m.Timestamp,
                    IsRead = m.IsRead,
                    SenderType = m.SenderType
                })
                .ToListAsync();
        }

        public async Task<IEnumerable<ChatMessageDto>> GetUnreadMessagesAsync(string userId)
        {
            return await _context.ChatMessages
                .Where(m => m.ReceiverId == userId && !m.IsRead)
                .OrderByDescending(m => m.Timestamp)
                .Select(m => new ChatMessageDto
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Content = m.Content,
                    Timestamp = m.Timestamp,
                    IsRead = m.IsRead,
                    SenderType = m.SenderType
                })
                .ToListAsync();
        }

        public async Task<int> GetUnreadMessageCountAsync(string userId)
        {
            return await _context.ChatMessages
                .CountAsync(m => m.ReceiverId == userId && !m.IsRead);
        }

        public async Task MarkMessageAsReadAsync(Guid messageId)
        {
            try
            {
                var message = await _context.ChatMessages.FindAsync(messageId);
                if (message != null)
                {
                    message.IsRead = true;
                    await _context.SaveChangesAsync();
                    
                    _logger.LogInformation("Message marked as read: {MessageId}", messageId);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error marking message as read: {MessageId}", messageId);
                throw;
            }
        }

        public async Task<bool> DeleteMessageAsync(Guid messageId, string userId)
        {
            try
            {
                var message = await _context.ChatMessages.FindAsync(messageId);
                
                if (message == null || message.SenderId != userId)
                {
                    return false;
                }

                _context.ChatMessages.Remove(message);
                await _context.SaveChangesAsync();
                
                _logger.LogInformation(
                    "Message deleted: {MessageId} by {UserId}", 
                    messageId, userId);
                
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting message: {MessageId}", messageId);
                throw;
            }
        }
    }
}