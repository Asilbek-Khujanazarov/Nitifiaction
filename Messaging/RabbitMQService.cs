using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace PatientRecovery.NotificationService.Messaging
{
    public class RabbitMQService : IRabbitMQService, IDisposable
    {
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly string _exchangeName = "patient_recovery";

        public RabbitMQService(string hostName = "localhost")
        {
            var factory = new ConnectionFactory { HostName = hostName };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();

            _channel.ExchangeDeclare(_exchangeName, ExchangeType.Topic);
        }

        public void PublishMessage(string message, string routingKey)
        {
            var body = Encoding.UTF8.GetBytes(message);
            _channel.BasicPublish(
                exchange: _exchangeName,
                routingKey: routingKey,
                basicProperties: null,
                body: body);
        }

        public void SubscribeToQueue<T>(string queueName, Func<T, Task> handler) where T : class
        {
            _channel.QueueDeclare(queueName, durable: true, exclusive: false, autoDelete: false);
            
            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var data = JsonSerializer.Deserialize<T>(message);
                
                if (data != null)
                {
                    await handler(data);
                }
                
                _channel.BasicAck(ea.DeliveryTag, false);
            };

            _channel.BasicConsume(queue: queueName,
                                autoAck: false,
                                consumer: consumer);
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}