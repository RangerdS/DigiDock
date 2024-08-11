using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Polly;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using RabbitMQ.Client.Exceptions;
using Serilog;
using System.Net.Sockets;
using System.Text;

namespace DigiDock.Business.Services
{
    public class LogQueueService : IDisposable
    {
        private readonly IConfiguration configuration;
        private readonly LogService logService;
        private IConnection connection;
        private IModel channel;

        public LogQueueService(IConfiguration configuration, LogService logService)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            this.logService = logService ?? throw new ArgumentNullException(nameof(logService));

            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:HostName"],
                UserName = configuration["RabbitMQ:UserName"],
                Password = configuration["RabbitMQ:Password"]
            };

            var retryPolicy = Policy
                .Handle<BrokerUnreachableException>()
                .Or<SocketException>()
                .WaitAndRetry(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), (exception, timeSpan, retryCount, context) =>
                {
                    Log.Error($"RabbitMQ connection attempt {retryCount} failed. Waiting {timeSpan} before next retry. Exception: {exception.Message}");
                });

            retryPolicy.Execute(() =>
            {
                connection = factory.CreateConnection();
                channel = connection.CreateModel();
                channel.QueueDeclare(queue: configuration["RabbitMQ:LogQueueName"],
                                      durable: true,
                                      exclusive: false,
                                      autoDelete: false,
                                      arguments: null);
            });
        }

        public void EnqueueLog(string logLevel, string message)
        {
            var logMessage = new { LogLevel = logLevel, Message = message };
            var logJson = JsonConvert.SerializeObject(logMessage);
            var bodyBytes = Encoding.UTF8.GetBytes(logJson);

            channel.BasicPublish(exchange: "",
                                  routingKey: configuration["RabbitMQ:LogQueueName"],
                                  basicProperties: null,
                                  body: bodyBytes);
        }

        public async Task ProcessQueue()
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var logMessage = JsonConvert.DeserializeObject<dynamic>(message);

                await logService.LogAsync((string)logMessage.LogLevel, (string)logMessage.Message);
            };

            channel.BasicConsume(queue: configuration["RabbitMQ:LogQueueName"],
                                  autoAck: true,
                                  consumer: consumer);
        }

        public void Dispose()
        {
            channel?.Close();
            connection?.Close();
        }
    }
}
