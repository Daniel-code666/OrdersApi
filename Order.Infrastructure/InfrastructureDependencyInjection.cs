using Microsoft.Extensions.DependencyInjection;
using Order.Infrastructure.Messaging;
using Order.Infrastructure.Repository;
using Orders.Application.Interfaces;

namespace Order.Infrastructure
{
    public static class InfrastructureDependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
            => services.AddScoped<IOrdersRepository, OrdersRepository>()
                .AddSingleton<IRabbitMqPublisher, RabbitMqPublisher>();
    }
}
