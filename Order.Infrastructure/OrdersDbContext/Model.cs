using Microsoft.EntityFrameworkCore;
using Order.Domain.Entitites.Orders;

namespace Order.Infrastructure.OrdersDbContext
{
    public sealed partial class OrdersApiDbContext
    {
        public DbSet<OrdersEntity> Orders { get; set; }
    }
}
