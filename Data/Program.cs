using Data.Persistence;
using Data.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

var dbConnectionString = Environment.GetEnvironmentVariable("TestApi_ConnectionString");

builder.Services.AddDbContext<KafkaDockerDbContext>(
    options => options.UseNpgsql(dbConnectionString)
);

// link and add repository & interface to services
builder.Services.AddTransient<IOrderRepository, OrderRepository>();

var app = builder.Build();

app.Run();
