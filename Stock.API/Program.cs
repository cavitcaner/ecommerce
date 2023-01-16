using Common;
using MassTransit;
using MassTransit.Internals;
using Microsoft.EntityFrameworkCore;
using Stock.API.Consumer;
using Stock.API.Database;
using static System.Net.Mime.MediaTypeNames;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddLogging();
builder.Services.AddMassTransit(x =>
{
    x.AddConsumer<OrderCreatedEventConsumer>();

    x.UsingRabbitMq((ctx, cfg) =>
    {
        cfg.Host(builder.Configuration.GetConnectionString("RabbitMq"));

        cfg.ReceiveEndpoint(QueueConst.OrderCreatedEventQueueName, x =>
        {
            x.ConfigureConsumer<OrderCreatedEventConsumer>(ctx);
        });
    });
});
builder.Services.AddMassTransitHostedService();

builder.Services.AddDbContext<StockDbContext>(opts =>
{
    opts.UseSqlServer(builder.Configuration.GetConnectionString("StockDbContext"));
});

using (ServiceProvider serviceProvider = builder.Services.BuildServiceProvider())
{
    //inserting mock data
    try
    {
        var context = serviceProvider.GetRequiredService<StockDbContext>();
        if (context.Stocks.FirstOrDefault() == null)
        {
            var rand = new Random();
            Enumerable.Range(1, 10).ToList().ForEach((x) =>
            {
                _ = context.Add(new Stock.API.Database.Stock()
                {
                    ProductName = $"Product (" + x + ")",
                    UnitInStock = rand.Next(1, 50),
                    UnitPrice = rand.Next(150, 500),
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
