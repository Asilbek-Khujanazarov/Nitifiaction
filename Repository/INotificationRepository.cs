using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PatientRecovery.NotificationService.Models;

namespace PatientRecovery.NotificationService.Repository
{
    public interface INotificationRepository
    {
        Task<Notification> CreateAsync(Notification notification);
        Task<Notification> GetByIdAsync(Guid id);
        Task<IEnumerable<Notification>> GetByRecipientIdAsync(string recipientId);
        Task<Notification> UpdateAsync(Notification notification);
        Task<bool> DeleteAsync(Guid id);
        Task<IEnumerable<Notification>> GetUnreadNotificationsAsync(string recipientId);
        Task<IEnumerable<Notification>> GetPendingNotificationsAsync();
    }
}