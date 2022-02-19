using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Receiver1Api
{
    public class ConsumeMessageService : BackgroundService
    {
        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {

            Console.WriteLine("Consuming Queue Now");

            ConnectionFactory factory = new ConnectionFactory() { HostName = "rabbitmq", Port = 5672 };
            factory.UserName = "guest";
            factory.Password = "guest";
            IConnection conn = factory.CreateConnection();
            IModel channel = conn.CreateModel();

            channel.ExchangeDeclare("my-direct-exchange", ExchangeType.Direct);

            
            channel.QueueDeclare(queue: "my-queue-1-for-mydirectexchange",
                                    durable: true,
                                    exclusive: false,
                                    autoDelete: false,
                                    arguments: null);

            channel.QueueBind("my-queue-1-for-mydirectexchange", "my-direct-exchange", "directexchange.demoroutingkey");

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += (model, ea) =>
            {
                var body = ea.Body.Span;
                var message = Encoding.UTF8.GetString(body);
                Console.WriteLine(" [x] Received from Rabbit: {0}", message);
            };
            channel.BasicConsume(queue: "my-queue-1-for-mydirectexchange",
                                    autoAck: true,
                                    consumer: consumer);
            return Task.CompletedTask;
        }
    }
}
