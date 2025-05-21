using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using PatientRecovery.NotificationService.Data;
using PatientRecovery.NotificationService.Models;
using PatientRecovery.NotificationService.DTOs;

namespace PatientRecovery.NotificationService.Services
{
    public class NotificationTemplateService : INotificationTemplateService
    {
        private readonly NotificationDbContext _context;

        public NotificationTemplateService(NotificationDbContext context)
        {
            _context = context;
        }

        public async Task<NotificationTemplate> CreateTemplateAsync(CreateNotificationTemplateRequest request)
        {
            var template = new NotificationTemplate
            {
                Id = Guid.NewGuid(),
                Name = request.Name,
                Subject = request.Subject,
                Body = request.Body,
                Type = request.Type,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            await _context.NotificationTemplates.AddAsync(template);
            await _context.SaveChangesAsync();

            return template;
        }

        public async Task<NotificationTemplate> GetTemplateByIdAsync(Guid id)
        {
            return await _context.NotificationTemplates.FindAsync(id);
        }

        public async Task<NotificationTemplate> GetTemplateByNameAsync(string name)
        {
            return await _context.NotificationTemplates
                .FirstOrDefaultAsync(t => t.Name == name && t.IsActive);
        }

        public async Task<IEnumerable<NotificationTemplate>> GetAllTemplatesAsync()
        {
            return await _context.NotificationTemplates.ToListAsync();
        }

        public async Task<NotificationTemplate> UpdateTemplateAsync(UpdateNotificationTemplateRequest request)
        {
            var template = await _context.NotificationTemplates.FindAsync(request.Id);
            if (template == null)
                return null;

            template.Subject = request.Subject;
            template.Body = request.Body;
            template.IsActive = request.IsActive;
            template.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();
            return template;
        }

        public async Task<bool> DeleteTemplateAsync(Guid id)
        {
            var template = await _context.NotificationTemplates.FindAsync(id);
            if (template == null)
                return false;

            _context.NotificationTemplates.Remove(template);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}