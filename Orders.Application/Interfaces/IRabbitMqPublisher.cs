using Orders.Application.Orders.Dtos;

namespace Orders.Application.Interfaces
{
    public interface IRabbitMqPublisher
    {
        Task PublishOrderCreatedAsync(OrdersCreated order_created);
    }
}
