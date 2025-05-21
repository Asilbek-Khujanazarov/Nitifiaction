using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using PatientRecovery.NotificationService.Models;
using PatientRecovery.NotificationService.DTOs;

namespace PatientRecovery.NotificationService.Services
{
    public interface INotificationTemplateService
    {
        Task<NotificationTemplate> CreateTemplateAsync(CreateNotificationTemplateRequest request);
        Task<NotificationTemplate> GetTemplateByIdAsync(Guid id);
        Task<NotificationTemplate> GetTemplateByNameAsync(string name);
        Task<IEnumerable<NotificationTemplate>> GetAllTemplatesAsync();
        Task<NotificationTemplate> UpdateTemplateAsync(UpdateNotificationTemplateRequest request);
        Task<bool> DeleteTemplateAsync(Guid id);
    }
}