using Orders.Worker;
using Orders.Worker.Consumers;
using Orders.Worker.Messaging;
using Orders.Worker.Services;

var builder = Host.CreateApplicationBuilder(args);

builder.Services.Configure<RabbitMqOptions>(builder.Configuration.GetSection("RabbitMq"));

builder.Services.Configure<MailPitOptions>(builder.Configuration.GetSection("Mail"));

builder.Services.AddHostedService<OrdersCreatedConsumer>();

builder.Services.AddServices();

var host = builder.Build();
host.Run();
