using Common;
using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Order.API.Business.Abstract;
using Order.API.Consumer;
using Order.API.Database;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddDbContext<OrderDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("OrderDbConnection"));
}, ServiceLifetime.Singleton);

builder.Services.AddSingleton<IOrderService, OrderService>();

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<StockReversedByMessageEventConsumer>();
    x.AddConsumer<PaymentSuccessEventConsumer>();
    x.AddConsumer<StockNotReservedEventConsumer>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMq"));

        cfg.ReceiveEndpoint(QueueConst.StockReversedByMessageEvent, x =>
        {
            x.ConfigureConsumer<StockReversedByMessageEventConsumer>(ctx);
        });
        cfg.ReceiveEndpoint(QueueConst.PaymentSuccessEventQueueName, x =>
        {
            x.ConfigureConsumer<PaymentSuccessEventConsumer>(ctx);
        });
        cfg.ReceiveEndpoint(QueueConst.StockNotReservedEventQueueName, x =>
        {
            x.ConfigureConsumer<StockNotReservedEventConsumer>(ctx);
        });
    });

});
builder.Services.AddMassTransitHostedService();

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
