using Common;
using Common.Stock;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Stock.API.Business.Abstract;
using Stock.API.Consumer;
using Stock.API.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<StockDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("StockDbContext"));
}, ServiceLifetime.Singleton);
builder.Services.AddSingleton<IStockService, StockService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedEventConsumer>();
    x.AddConsumer<PaymentFailedConsumer>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMq"));

        cfg.ReceiveEndpoint(QueueConst.OrderCreatedEventQueueName, x =>
        {
            x.ConfigureConsumer<OrderCreatedEventConsumer>(ctx);
        });
        cfg.ReceiveEndpoint(QueueConst.StockPaymentFailedEventQueueName, x =>
        {
            x.ConfigureConsumer<PaymentFailedConsumer>(ctx);
        });
    });
});
builder.Services.AddMassTransitHostedService();

//inserting mock data
{
    try
    {
        using ServiceProvider serviceProvider = builder.Services.BuildServiceProvider();

        var context = serviceProvider.GetRequiredService<StockDbContext>();
        if (context.Stocks.FirstOrDefault() == null)
        {
            MockStock.Stocks.ForEach((x) =>
            {
                _ = context.Add(new Stock.API.Database.Stock()
                {
                    ProductId = x.ProductId,
                    ProductName = x.ProductName,
                    UnitInStock = x.UnitInStock,
                    UnitPrice = x.UnitPrice,
                });
            });
            context.SaveChangesAsync();
        }
    }
    catch (Exception)
    {
    }
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
