using PatientRecovery.NotificationService.Services;

namespace PatientRecoverySystem.NotificationService.BackgroundServices
{
    public class NotificationProcessorService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<NotificationProcessorService> _logger;
        private readonly IConfiguration _configuration;

        public NotificationProcessorService(
            IServiceProvider serviceProvider,
            ILogger<NotificationProcessorService> logger,
            IConfiguration configuration)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _configuration = configuration;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var notificationService = 
                            scope.ServiceProvider.GetRequiredService<INotificationService>();
                        await notificationService.ProcessPendingNotificationsAsync();
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error processing notifications");
                }

                var delay = _configuration.GetValue("NotificationProcessorDelaySeconds", 30);
                await Task.Delay(TimeSpan.FromSeconds(delay), stoppingToken);
            }
        }
    }
}