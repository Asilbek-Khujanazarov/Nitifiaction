using System.Threading.Tasks;
using PatientRecovery.NotificationService.Models;

namespace PatientRecovery.NotificationService.Services
{
    public interface IEmailService
    {
        Task SendEmailAsync(Notification notification);
    }
}