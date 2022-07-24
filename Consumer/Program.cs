using KafkaDocker.Consumer;
using KafkaDocker.Data.Persistence;
using KafkaDocker.Data.Repository;
using Microsoft.EntityFrameworkCore;

var dbConnectionString = Environment.GetEnvironmentVariable("TestApi_ConnectionString");

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<ConsumerService>();
        services.AddDbContext<KafkaDockerDbContext>(
            options => options.UseNpgsql(dbConnectionString)
        );
        services.AddTransient<IOrderRepository, OrderRepository>();
    })
    .Build();

await host.RunAsync();
