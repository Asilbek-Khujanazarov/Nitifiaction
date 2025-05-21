using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PatientRecovery.NotificationService.DTOs;
using PatientRecovery.NotificationService.Models;
using PatientRecovery.NotificationService.Repository;

namespace PatientRecovery.NotificationService.Services
{
    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _repository;
        private readonly INotificationTemplateService _templateService;
        private readonly IEmailService _emailService;
        private readonly ISmsService _smsService;
        private readonly ILogger<NotificationService> _logger;

        public NotificationService(
            INotificationRepository repository,
            INotificationTemplateService templateService,
            IEmailService emailService,
            ISmsService smsService,
            ILogger<NotificationService> logger)
        {
            _repository = repository;
            _templateService = templateService;
            _emailService = emailService;
            _smsService = smsService;
            _logger = logger;
        }

        public async Task<Notification> CreateNotificationAsync(CreateNotificationRequest request)
        {
            var notification = new Notification
            {
                Id = Guid.NewGuid(),
                Title = request.Title,
                Message = request.Message,
                RecipientId = request.RecipientId,
                RecipientEmail = request.RecipientEmail,
                RecipientPhone = request.RecipientPhone,
                Type = request.Type,
                Priority = request.Priority,
                Status = NotificationStatus.Pending,
                CreatedAt = DateTime.UtcNow,
                IsRead = false
            };

            try
            {
                notification = await _repository.CreateAsync(notification);
                await SendNotificationAsync(notification);
                return notification;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating notification {NotificationId}", notification.Id);
                notification.Status = NotificationStatus.Failed;
                notification.ErrorMessage = ex.Message;
                await _repository.UpdateAsync(notification);
                throw;
            }
        }

        public async Task<Notification> GetNotificationByIdAsync(Guid id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByRecipientAsync(
            string recipientId,
            NotificationStatus? status = null,
            DateTime? fromDate = null,
            int page = 1,
            int pageSize = 10)
        {
            var notifications = await _repository.GetByRecipientIdAsync(recipientId);

            if (status.HasValue)
                notifications = notifications.Where(n => n.Status == status.Value);

            if (fromDate.HasValue)
                notifications = notifications.Where(n => n.CreatedAt >= fromDate.Value);

            return notifications
                .Skip((page - 1) * pageSize)
                .Take(pageSize);
        }

        public async Task<bool> MarkAsReadAsync(Guid id)
        {
            var notification = await _repository.GetByIdAsync(id);
            if (notification == null)
                return false;

            notification.IsRead = true;
            notification.ReadAt = DateTime.UtcNow;
            await _repository.UpdateAsync(notification);
            return true;
        }

        public async Task<BulkNotificationResultDto> SendBulkNotificationsAsync(BulkNotificationRequest request)
        {
            var result = new BulkNotificationResultDto
            {
                TotalRecipients = request.RecipientIds.Count,
                SuccessCount = 0,
                FailureCount = 0,
                FailedRecipientIds = new List<string>(),
                ErrorMessages = new List<string>()
            };

            foreach (var recipientId in request.RecipientIds)
            {
                try
                {
                    var notificationRequest = new CreateNotificationRequest
                    {
                        Title = request.Title,
                        Message = request.Message,
                        RecipientId = recipientId,
                        Type = request.Type,
                        Priority = request.Priority
                    };

                    await CreateNotificationAsync(notificationRequest);
                    result.SuccessCount++;
                }
                catch (Exception ex)
                {
                    result.FailureCount++;
                    result.FailedRecipientIds.Add(recipientId);
                    result.ErrorMessages.Add($"Error for recipient {recipientId}: {ex.Message}");
                }
            }

            return result;
        }

        public async Task<int> ProcessPendingNotificationsAsync()
        {
            var pendingNotifications = await _repository.GetPendingNotificationsAsync();
            var processedCount = 0;

            foreach (var notification in pendingNotifications)
            {
                try
                {
                    await SendNotificationAsync(notification);
                    processedCount++;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing notification {NotificationId}", notification.Id);
                }
            }

            return processedCount;
        }

        public async Task<Notification> CreateFromTemplateAsync(
            string templateName,
            Dictionary<string, string> parameters,
            string recipientId)
        {
            var template = await _templateService.GetTemplateByNameAsync(templateName);
            if (template == null)
                throw new ArgumentException($"Template {templateName} not found");

            var message = template.Body;
            foreach (var param in parameters)
            {
                message = message.Replace($"{{{param.Key}}}", param.Value);
            }

            var request = new CreateNotificationRequest
            {
                Title = template.Subject,
                Message = message,
                RecipientId = recipientId,
                Type = template.Type,
                Priority = NotificationPriority.High
            };

            return await CreateNotificationAsync(request);
        }

        private async Task SendNotificationAsync(Notification notification)
        {
            try
            {
                switch (notification.Type)
                {
                    case NotificationType.Email when !string.IsNullOrEmpty(notification.RecipientEmail):
                        await _emailService.SendEmailAsync(notification);
                        break;
                    case NotificationType.SMS when !string.IsNullOrEmpty(notification.RecipientPhone):
                        await _smsService.SendSmsAsync(notification);
                        break;
                }

                notification.Status = NotificationStatus.Sent;
                notification.SentAt = DateTime.UtcNow;
                await _repository.UpdateAsync(notification);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending notification {NotificationId}", notification.Id);
                throw;
            }
        }
    }
}