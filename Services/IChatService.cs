using PatientRecovery.NotificationService.Models;
using PatientRecovery.NotificationService.DTOs;

namespace PatientRecovery.NotificationService.Services
{
    public interface IChatService
    {
        Task<ChatMessage> SaveMessageAsync(ChatMessage message);
        Task<IEnumerable<ChatMessageDto>> GetChatHistoryAsync(string userId1, string userId2);
        Task<IEnumerable<ChatMessageDto>> GetUnreadMessagesAsync(string userId);
        Task MarkMessageAsReadAsync(Guid messageId);
    }
}