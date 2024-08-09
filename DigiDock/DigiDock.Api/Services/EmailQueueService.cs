using Newtonsoft.Json;
using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using System.Text;
using MediatR;
using DigiDock.Business.Cqrs;

namespace DigiDock.Api.Services
{
    public class EmailQueueService : IDisposable
    {
        private readonly IConfiguration configuration;
        private readonly EmailService emailService;
        private readonly IConnection connection;
        private readonly IModel channel;
        private readonly IMediator mediator;

        public EmailQueueService(IConfiguration configuration, EmailService emailService)
        {
            this.configuration = configuration;
            this.emailService = emailService;

            var factory = new ConnectionFactory()
            {
                HostName = configuration["RabbitMQ:HostName"],
                UserName = configuration["RabbitMQ:UserName"],
                Password = configuration["RabbitMQ:Password"]
            };

            connection = factory.CreateConnection();
            channel = connection.CreateModel();
            channel.QueueDeclare(queue: configuration["RabbitMQ:EmailQueueName"],
                                  durable: true,
                                  exclusive: false,
                                  autoDelete: false,
                                  arguments: null);
        }

        // Publish email to specific email
        public void EnqueueEmailTo(string toEmail, string subject, string body)
        {
            var emailMessage = new { ToEmail = toEmail, Subject = subject, Body = body };
            var message = JsonConvert.SerializeObject(emailMessage);
            var bodyBytes = Encoding.UTF8.GetBytes(message);

            channel.BasicPublish(exchange: "",
                                  routingKey: configuration["RabbitMQ:EmailQueueName"],
                                  basicProperties: null,
                                  body: bodyBytes);
        }

        // Publish email to all users
        public void EnqueueEmail(string subject, string body)
        {
            // Get User email from db
            List<String> userEmailList = mediator.Send(new GetAllUserEmailListQuery()).Result.Data;

            foreach (var email in userEmailList)
            {
                EnqueueEmailTo(email, subject, body);
            }
        }

        public async Task ProcessQueue()
        {
            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var emailMessage = JsonConvert.DeserializeObject<dynamic>(message);

                await emailService.SendEmailAsync((string)emailMessage.ToEmail, (string)emailMessage.Subject, (string)emailMessage.Body);
            };

            channel.BasicConsume(queue: configuration["RabbitMQ:EmailQueueName"],
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