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
            _channel.QueueDeclare(queue:"queue1",
                                  durable:true,
                                  exclusive:false,
                                  autoDelete:false,
                                  arguments:null);


        }

        public bool Enqueue(string message)
        {
            var body = Encoding.UTF8.GetBytes("server processed: "+ message);
            _channel.BasicPublish(exchange:"",
                routingKey:"queue1",basicProperties:null,body:body);
            Console.WriteLine(" [x] Published {0} to RabbitMQ", message);
            return true;
        }
    }
}
