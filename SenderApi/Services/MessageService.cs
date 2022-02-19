using RabbitMQ.Client;
using System;
using System.Text;

namespace SenderApi.Services
{
    public interface IMessageService
    {
        bool Enqueue(string message);
    }
    public class MessageService : IMessageService
    {
        ConnectionFactory connectionFactory;    
        IConnection connection;
        IModel _channel;

        public MessageService()
        {
            System.Console.WriteLine("connecting to rabbitmq");

            connectionFactory = new ConnectionFactory()
            {
                HostName = "rabbitmq",
                Port = 5672
            };
            connectionFactory.UserName = "guest";
            connectionFactory.Password = "guest";
            connection =  connectionFactory.CreateConnection();
            _channel = connection.CreateModel();
            _channel.ExchangeDeclare(exchange:"my-direct-exchange",type:ExchangeType.Direct);
            //direct exhange tipinde queuelar routing key üstünden exchange'i dinler

        }

        public bool Enqueue(string message)
        {
            var body = Encoding.UTF8.GetBytes("server processed: "+ message);
            _channel.BasicPublish(exchange:"my-direct-exchange",
                routingKey:"directexchange.demoroutingkey",basicProperties:null,body:body);
            Console.WriteLine(" [x] Published {0} to RabbitMQ", message);
            return true;
        }
    }
}
