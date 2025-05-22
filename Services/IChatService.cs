using NotificationService.Models;
using NotificationService.DTOs;

namespace NotificationService.Services
{
    public interface IChatService
    {
        Task<ChatMessage> SaveMessageAsync(ChatMessage message);
        Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync(string userId1, string userId2, int skip = 0, int take = 50);
        Task<IEnumerable<ChatMessageDto>> GetUnreadMessagesAsync(string userId);
        Task MarkMessageAsReadAsync(Guid messageId);
        Task<int> GetUnreadMessageCountAsync(string userId);
        Task<bool> DeleteMessageAsync(Guid messageId, string userId);
    }
}