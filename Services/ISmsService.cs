using System.Threading.Tasks;
using PatientRecovery.NotificationService.Models;

namespace PatientRecovery.NotificationService.Services
{
    public interface ISmsService
    {
        Task SendSmsAsync(Notification notification);
    }
}