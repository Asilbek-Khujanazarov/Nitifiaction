using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PatientRecovery.NotificationService.Models;
using PatientRecovery.NotificationService.Configuration;

namespace PatientRecovery.NotificationService.Services
{
    public class SmsService : ISmsService
    {
        private readonly SmsSettings _smsSettings;

        public SmsService(IOptions<SmsSettings> smsSettings)
        {
            _smsSettings = smsSettings.Value;
        }

        public async Task SendSmsAsync(Notification notification)
        {
            // SMS yuborish logikasi
            // Bu yerda SMS provider (Twilio, Nexmo kabi) bilan integratsiya qilish kerak
            await Task.CompletedTask;
        }
    }
}