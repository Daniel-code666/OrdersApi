using Microsoft.EntityFrameworkCore;
using Order.Domain.Entitites.Orders;

namespace Order.Infrastructure.OrdersDbContext
{
    public sealed partial class OrdersApiDbContext : DbContext
    {
        public OrdersApiDbContext(DbContextOptions<OrdersApiDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.HasDefaultSchema("data");
            ConfigureOrders(modelBuilder);
        }

        private static void ConfigureOrders(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OrdersEntity>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.OrderNumber).IsUnique();
                entity.Property(e => e.OrderNumber).IsRequired();
                entity.Property(e => e.CustomerName).IsRequired();
                entity.Property(e => e.CustomerEmail).IsRequired();
                entity.Property(e => e.CustomerPhone).IsRequired();
                entity.Property(e => e.TotalAmount).HasColumnType("decimal(18,2)");
                entity.Property(e => e.TotalFee).HasColumnType("decimal(18,2)");
                entity.Property(e => e.OrderState).IsRequired();
                entity.Property(e => e.Active).IsRequired();
            });
        }
    }
}
