using Microsoft.Extensions.Hosting;
using PatientRecovery.NotificationService.Services;
using PatientRecovery.NotificationService.Models;

namespace PatientRecovery.NotificationService.Messaging
{
    public class EmergencyNotificationConsumer : IHostedService
    {
        private readonly IRabbitMQService _messageBus;
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<EmergencyNotificationConsumer> _logger;

        public EmergencyNotificationConsumer(
            IRabbitMQService messageBus,
            IServiceProvider serviceProvider,
            ILogger<EmergencyNotificationConsumer> logger)
        {
            _messageBus = messageBus;
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Starting Emergency Notification Consumer at {Time}", DateTime.UtcNow);

            _messageBus.SubscribeToQueue<EmergencyAlert>("emergency_notifications_queue", async (message) =>
            {
                try
                {
                    using var scope = _serviceProvider.CreateScope();
                    var notificationService = scope.ServiceProvider.GetRequiredService<INotificationService>();

                    var parameters = new Dictionary<string, string>
                    {
                        { "AlertType", message.Type.ToString() },
                        { "PatientId", message.PatientId.ToString() },
                        { "Description", message.Description },
                        { "Location", message.Location ?? "Unknown" },
                        { "Priority", message.Priority.ToString() }
                    };

                    await notificationService.CreateFromTemplateAsync(
                        "EmergencyAlert",
                        parameters,
                        message.DoctorId?.ToString() ?? "AllDoctors");

                    _logger.LogInformation(
                        "Created emergency notification for alert {AlertId} at {Time}",
                        message.Id, DateTime.UtcNow);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, 
                        "Error processing emergency notification for alert {AlertId} at {Time}",
                        message.Id, DateTime.UtcNow);
                }
            });

            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _logger.LogInformation("Stopping Emergency Notification Consumer at {Time}", DateTime.UtcNow);
            return Task.CompletedTask;
        }
    }
}