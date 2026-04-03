using Orders.Worker.Orders;

namespace Orders.Worker.Services
{
    public interface IEmailSender
    {
        Task SendOrderCreatedEmailAsync(OrdersCreated order_created);
    }
}
