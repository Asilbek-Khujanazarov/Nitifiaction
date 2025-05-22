using AutoMapper;
using NotificationService.Models;
using NotificationService.DTOs;

namespace PatientRecovery.NotificationService.Mapping
{
    public class NotificationMappingProfile : Profile
    {
        public NotificationMappingProfile()
        {
            CreateMap<ChatMessage, ChatMessageDto>().ReverseMap();
        }
    }
}