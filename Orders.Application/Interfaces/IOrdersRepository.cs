using Order.Domain.Entitites.Orders;
using Orders.Application.Orders.Dtos;

namespace Orders.Application.Interfaces
{
    public interface IOrdersRepository
    {
        Task<int> CreateOrder(OrdersEntity order);
        Task<OrdersEntity?> GetOrderById(int Id);
        Task<IEnumerable<OrdersEntity>> GetAllOrders(OrdersFilters filters);
    }
}
