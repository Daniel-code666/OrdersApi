using Microsoft.Extensions.Options;
using Orders.Worker.Messaging;
using Orders.Worker.Orders;
using Orders.Worker.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Orders.Worker.Consumers
{
    public class OrdersCreatedConsumer : BackgroundService
    {
        private readonly IEmailSender _email_sender;
        private readonly RabbitMqOptions _rabbit_mq_options;
        private readonly ILogger<OrdersCreatedConsumer> _logger;
        private IConnection? _connection;
        private IChannel? _channel;

        public OrdersCreatedConsumer(
            IOptions<RabbitMqOptions> rabbit_mq_options,
            ILogger<OrdersCreatedConsumer> logger,
            IEmailSender emailSender)
        {
            _rabbit_mq_options = rabbit_mq_options.Value;
            _logger = logger;
            _email_sender = emailSender;
        }

        public override async Task StartAsync(CancellationToken cancellation_token)
        {
            var factory = new ConnectionFactory
            {
                HostName = _rabbit_mq_options.Host,
                Port = _rabbit_mq_options.Port,
                UserName = _rabbit_mq_options.Username,
                Password = _rabbit_mq_options.Password
            };

            _connection = await factory.CreateConnectionAsync(cancellation_token);
            _channel = await _connection.CreateChannelAsync(cancellationToken: cancellation_token);

            await _channel.ExchangeDeclareAsync(
                exchange: _rabbit_mq_options.Exchange,
                type: ExchangeType.Direct,
                durable: true,
                autoDelete: false,
                cancellationToken: cancellation_token);

            await _channel.QueueDeclareAsync(
                queue: _rabbit_mq_options.Queue,
                durable: true,
                exclusive: false,
                autoDelete: false,
                cancellationToken: cancellation_token);

            await _channel.QueueBindAsync(
                queue: _rabbit_mq_options.Queue,
                exchange: _rabbit_mq_options.Exchange,
                routingKey: _rabbit_mq_options.RoutingKey,
                cancellationToken: cancellation_token);

            await base.StartAsync(cancellation_token);
        }

        protected override async Task ExecuteAsync(CancellationToken stopping_token)
        {
            if (_channel is null)
                return;

            var consumer = new AsyncEventingBasicConsumer(_channel);

            consumer.ReceivedAsync += async (_, ea) =>
            {
                try
                {
                    var body = ea.Body.ToArray();
                    var message = Encoding.UTF8.GetString(body);

                    var order_created = JsonSerializer.Deserialize<OrdersCreated>(message);

                    if (order_created is not null)
                    {
                        _logger.LogInformation(
                            "Evento recibido. OrderId: {order_id}, OrderNumber: {order_number}, CustomerName: {customer_name}",
                            order_created.OrderId,
                            order_created.OrderNumber,
                            order_created.CustomerName);

                        await _email_sender.SendOrderCreatedEmailAsync(order_created);

                        _logger.LogInformation(
                            "Correo enviado correctamente para la orden {order_id}",
                            order_created.OrderId);
                    }

                    await _channel.BasicAckAsync(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false,
                        cancellationToken: stopping_token);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error procesando evento OrderCreated");

                    await _channel.BasicNackAsync(
                        deliveryTag: ea.DeliveryTag,
                        multiple: false,
                        requeue: true,
                        cancellationToken: stopping_token);
                }
            };

            await _channel.BasicConsumeAsync(
                queue: _rabbit_mq_options.Queue,
                autoAck: false,
                consumer: consumer,
                cancellationToken: stopping_token);

            while (!stopping_token.IsCancellationRequested)
            {
                await Task.Delay(1000, stopping_token);
            }
        }

        public override async Task StopAsync(CancellationToken cancellation_token)
        {
            if (_channel is not null)
                await _channel.CloseAsync(cancellationToken: cancellation_token);

            if (_connection is not null)
                await _connection.CloseAsync(cancellationToken: cancellation_token);

            await base.StopAsync(cancellation_token);
        }
    }
}
