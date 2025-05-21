using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PatientRecovery.NotificationService.Models;
using PatientRecovery.NotificationService.DTOs;

namespace PatientRecovery.NotificationService.Services
{
    public interface INotificationService
    {
        Task<Notification> CreateNotificationAsync(CreateNotificationRequest request);
        Task<Notification> GetNotificationByIdAsync(Guid id);
        Task<IEnumerable<Notification>> GetNotificationsByRecipientAsync(
            string recipientId,
            NotificationStatus? status = null,
            DateTime? fromDate = null,
            int page = 1,
            int pageSize = 10);
        Task<bool> MarkAsReadAsync(Guid id);
        Task<BulkNotificationResultDto> SendBulkNotificationsAsync(BulkNotificationRequest request);
        Task<int> ProcessPendingNotificationsAsync();
        Task<Notification> CreateFromTemplateAsync(
            string templateName,
            Dictionary<string, string> parameters,
            string recipientId);
    }
}