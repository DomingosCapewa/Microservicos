using System.Text;
using RabbitMQ.Client;

namespace MS.Service
{
    public class RabbitMqService
    {
        private readonly IConnection _connection;

        public RabbitMqService()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                Port = 5672,
                UserName = "guest",
                Password = "guest"
            };

            _connection = factory.CreateConnection();
        }

        public void SendMessageAsync(string message)
        {
            if (_connection == null)
            {
                throw new Exception("RabbitMQ connection is not established.");
            }

            using (var channel = _connection.CreateModel())
            {
                channel.QueueDeclare(
                    queue: "myQueue",
                    durable: true,     // FIXED
                    exclusive: false,
                    autoDelete: false,
                    arguments: null);

                var body = Encoding.UTF8.GetBytes(message);
                var props = channel.CreateBasicProperties();
                props.Persistent = true;

                channel.BasicPublish(
                    exchange: "",
                    routingKey: "myQueue",
                    basicProperties: props,
                    body: body);
            }
        }
    }
}
