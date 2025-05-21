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

        // ... other methods implementation ...

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

        // ... implement other interface methods ...
    }
}