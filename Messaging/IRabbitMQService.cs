namespace PatientRecovery.NotificationService.Messaging
{
    public interface IRabbitMQService
    {
        void PublishMessage(string message, string routingKey);
        void SubscribeToQueue<T>(string queueName, Func<T, Task> handler) where T : class;
    }
}