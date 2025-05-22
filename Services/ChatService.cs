using Microsoft.EntityFrameworkCore;
using PatientRecovery.NotificationService.Data;
using PatientRecovery.NotificationService.Models;
using PatientRecovery.NotificationService.DTOs;

namespace PatientRecovery.NotificationService.Services
{
    public class ChatService : IChatService
    {
        private readonly NotificationDbContext _context;
        private readonly ILogger<ChatService> _logger;

        public ChatService(NotificationDbContext context, ILogger<ChatService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<ChatMessage> SaveMessageAsync(ChatMessage message)
        {
            try
            {
                await _context.ChatMessages.AddAsync(message);
                await _context.SaveChangesAsync();
                return message;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error saving chat message");
                throw;
            }
        }

        public async Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync(string userId1, string userId2)
        {
            return await _context.ChatMessages
                .Where(m => (m.SenderId == userId1 && m.ReceiverId == userId2) ||
                           (m.SenderId == userId2 && m.ReceiverId == userId1))
                .OrderByDescending(m => m.Timestamp)
                .Select(m => new ChatMessageDto
                {
                    Id = m.Id,
                    SenderId = m.SenderId,
                    ReceiverId = m.ReceiverId,
                    Content = m.Content,
                    Timestamp = m.Timestamp,
                    IsRead = m.IsRead,
                    SenderRole = m.SenderRole,
                    SenderName = m.SenderRole // Bu yerda real nomni olish kerak
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
                    SenderRole = m.SenderRole,
                    SenderName = m.SenderRole
                })
                .ToListAsync();
        }

        public async Task MarkMessageAsReadAsync(Guid messageId)
        {
            var message = await _context.ChatMessages.FindAsync(messageId);
            if (message != null)
            {
                message.IsRead = true;
                await _context.SaveChangesAsync();
            }
        }
    }
}