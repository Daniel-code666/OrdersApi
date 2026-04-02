using Order.Domain.Entitites.Orders;
using Orders.Application.Orders.Dtos;

namespace Orders.Application.Interfaces
{
    public interface IOrdersRepository
    {
        Task<bool> CreateOrder(OrdersEntity order);
        Task<OrdersEntity?> GetOrderById(int Id);
        Task<IEnumerable<OrdersEntity>> GetAllOrders(OrdersFilters filters);
    }
}
