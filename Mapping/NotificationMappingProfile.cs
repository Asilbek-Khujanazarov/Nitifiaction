using AutoMapper;
using PatientRecovery.NotificationService.Models;
using PatientRecovery.NotificationService.DTOs;

namespace PatientRecovery.NotificationService.Mapping
{
    public class NotificationMappingProfile : Profile
    {
        public NotificationMappingProfile()
        {
            CreateMap<Notification, NotificationDto>();
            CreateMap<CreateNotificationRequest, Notification>();
            
            CreateMap<NotificationTemplate, NotificationTemplateDto>();
            CreateMap<CreateNotificationTemplateRequest, NotificationTemplate>();
            CreateMap<UpdateNotificationTemplateRequest, NotificationTemplate>();
        }
    }
}