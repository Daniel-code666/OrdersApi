using Microsoft.EntityFrameworkCore;
using Order.Infrastructure;
using Order.Infrastructure.Messaging;
using Order.Infrastructure.OrdersDbContext;
using Orders.Application;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers().AddJsonOptions(o =>
{
    o.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
});

builder.Services.AddDbContext<OrdersApiDbContext>(options => options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),
    x => x.MigrationsAssembly("Order.Infrastructure")))
    .AddApplication().AddInfrastructure();

builder.Services.Configure<RabbitMqOptions>(
    builder.Configuration.GetSection("RabbitMq"));

var app = builder.Build();

//using (var scope = app.Services.CreateScope())
//{
//    var dbContext = scope.ServiceProvider.GetRequiredService<OrdersApiDbContext>();
//    dbContext.Database.Migrate();
//}

// Configure the HTTP request pipeline.
app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
