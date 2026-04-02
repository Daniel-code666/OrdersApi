using Order.Domain.Entitites.Base;
using Orders.Application.Orders.Dtos;

namespace Orders.Application.Orders.UseCases.OrdersBusiness.Interfaces
{
    public interface IOrdersBusiness
    {
        Task<DbActions> CreateOrder(OrdersCreate order);
        Task<OrdersRead?> GetById(int id);
        Task<IEnumerable<OrdersRead>> GetAll(OrdersFilters filters);
    }
}
