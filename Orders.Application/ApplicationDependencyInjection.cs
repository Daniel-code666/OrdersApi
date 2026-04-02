using Microsoft.Extensions.DependencyInjection;
using Orders.Application.Common;
using Orders.Application.Orders.UseCases.OrdersBusiness.Interfaces;
using Orders.Application.Orders.UseCases.OrdersBusiness.Repository;

namespace Orders.Application
{
    public static class ApplicationDependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
            => services.AddAutoMapper(cfg => { }, typeof(ApplicationAppProfile).Assembly).AddScoped<IOrdersBusiness, OrdersBusiness>();
    }
}
