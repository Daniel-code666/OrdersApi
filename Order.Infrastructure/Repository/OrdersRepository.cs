using Microsoft.EntityFrameworkCore;
using Order.Domain.Entitites.Orders;
using Order.Infrastructure.Common;
using Order.Infrastructure.OrdersDbContext;
using Orders.Application.Interfaces;
using Orders.Application.Orders.Dtos;

namespace Order.Infrastructure.Repository
{
    public class OrdersRepository : IOrdersRepository
    {
        private readonly OrdersApiDbContext _dbContext;

        public OrdersRepository(OrdersApiDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        async Task<int> IOrdersRepository.CreateOrder(OrdersEntity order)
        {
            _dbContext.Orders.Add(order);
            var rows_affected = await _dbContext.SaveChangesAsync();
            if (rows_affected == 0)
                return 0;
            return order.Id;
        }

        async Task<OrdersEntity?> IOrdersRepository.GetOrderById(int id)
            => await _dbContext.Orders.FirstOrDefaultAsync(x => x.Id == id);

        async Task<IEnumerable<OrdersEntity>> IOrdersRepository.GetAllOrders(OrdersFilters filters)
        {
            IQueryable<OrdersEntity> query = _dbContext.Orders.AsQueryable();

            query = query
                .ApplyStringFilter(x => x.OrderNumber, filters.OrderNumber, false)
                .ApplyStringFilter(x => x.CustomerName, filters.CustomerName, true)
                .ApplyStringFilter(x => x.CustomerEmail, filters.CustomerEmail, false)
                .ApplyStringFilter(x => x.CustomerPhone, filters.CustomerPhone, false);

            if (filters.Active.HasValue)
                query = query.Where(x => x.Active == filters.Active.Value);

            if (filters.OrderState.HasValue)
                query = query.Where(x => x.OrderState == filters.OrderState.Value);

            query = query.ApplyPaging(filters.Page, filters.PageSize);

            return await query.ToListAsync();
        }
    }
}
