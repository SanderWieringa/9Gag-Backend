using Microsoft.Extensions.Configuration;
using PostService.Dtos;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;

namespace PostService.AsyncDataServices
{
    public class RabbitMQProducer : IRabbitMQProducer
    {
        private readonly IConfiguration _configuration;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQProducer(IConfiguration configuration)
        {
            _configuration = configuration;
            var factory = new ConnectionFactory()
            {
                HostName = _configuration["RabbitMQHost"],
                Port = int.Parse(_configuration["RabbitMQPort"])
            };
            try
            {
                _connection = factory.CreateConnection();
                _channel = _connection.CreateModel();

                _channel.ExchangeDeclare(exchange: "PublishPostExchange", type: ExchangeType.Fanout);

                _channel.QueueDeclare("PublishPostQueue",
                    durable: true,
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);
                _channel.QueueBind("PublishPostQueue", "PublishPostExchange", "PostService.Created.*");

                _connection.ConnectionShutdown += RabbitMQ_ConnectionShutdown;

                Console.WriteLine("--> Connected to Message Bus");
            }
            catch (Exception e)
            {
                Console.WriteLine($"--> Could not connect to the Message Bus: {e.Message}");
            }
        }

        public void PublishNewPost(PostPublishedDto postPublishedDto)
        {
            var message = JsonSerializer.Serialize(postPublishedDto);

            try
            {
                if (_connection.IsOpen)
                {
                    Console.WriteLine("--> RabbitMQ Connection Open, sending message...");
                    SendMessage(message);
                }
            }
            catch (Exception)
            {
                Console.WriteLine("--> Connection is not open...");
            }
        }

        private void SendMessage(string message)
        {
            var body = Encoding.UTF8.GetBytes(message);

            _channel.BasicPublish(exchange: "trigger",
                routingKey: "",
                 basicProperties: null,
                 body: body);

            Console.WriteLine($"--> We have sent message");
        }

        private void RabbitMQ_ConnectionShutdown(object sender, ShutdownEventArgs e)
        {
            Console.WriteLine("--> RabbitMQ Connection Shutdown");
        }
    }
}
