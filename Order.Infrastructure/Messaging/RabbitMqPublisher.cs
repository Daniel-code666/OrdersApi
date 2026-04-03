using Microsoft.Extensions.Options;
using Orders.Application.Interfaces;
using Orders.Application.Orders.Dtos;
using RabbitMQ.Client;
using System.Text;
using System.Text.Json;

namespace Order.Infrastructure.Messaging
{
    public class RabbitMqPublisher : IRabbitMqPublisher
    {
        private readonly RabbitMqOptions _rabbit_mq_options;
        private readonly IConnection _connection;
        private readonly IChannel _channel;

        public RabbitMqPublisher(IOptions<RabbitMqOptions> rabbit_mq_options)
        {
            _rabbit_mq_options = rabbit_mq_options.Value;

            var factory = new ConnectionFactory
            {
                HostName = _rabbit_mq_options.Host,
                Port = _rabbit_mq_options.Port,
                UserName = _rabbit_mq_options.Username,
                Password = _rabbit_mq_options.Password
            };

            _connection = factory.CreateConnectionAsync().GetAwaiter().GetResult();
            _channel = _connection.CreateChannelAsync().GetAwaiter().GetResult();

            _channel.ExchangeDeclareAsync(
                exchange: _rabbit_mq_options.Exchange,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false
            ).GetAwaiter().GetResult();

            _channel.QueueDeclareAsync(
                queue: _rabbit_mq_options.Queue,
                durable: true,
                exclusive: false,
                autoDelete: false
            ).GetAwaiter().GetResult();

            _channel.QueueBindAsync(
                queue: _rabbit_mq_options.Queue,
                exchange: _rabbit_mq_options.Exchange,
                routingKey: _rabbit_mq_options.RoutingKey
            ).GetAwaiter().GetResult();
        }

        public async Task PublishOrderCreatedAsync(OrdersCreated order_created)
        {
            var message = JsonSerializer.Serialize(order_created);
            var body = Encoding.UTF8.GetBytes(message);

            var properties = new BasicProperties
            {
                Persistent = true,
                ContentType = "application/json"
            };

            await _channel.BasicPublishAsync(
                exchange: _rabbit_mq_options.Exchange,
                routingKey: _rabbit_mq_options.RoutingKey,
                mandatory: false,
                basicProperties: properties,
                body: body
            );
        }

        public void Dispose()
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }
    }
}
