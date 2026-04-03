namespace Orders.Worker.Services
{
    public static class ServiceDependencyInjection
    {
        public static IServiceCollection AddServices(this IServiceCollection services)
            => services.AddSingleton<IEmailSender, EmailSender>();
    }
}
