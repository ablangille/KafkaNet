using KafkaDocker.Consumer;
using KafkaDocker.Data.Repository;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<ConsumerService>();
        services.AddTransient<IOrderRepository, OrderRepository>();
    })
    .Build();

await host.RunAsync();
