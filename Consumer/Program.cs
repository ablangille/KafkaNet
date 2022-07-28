using KafkaDocker.Consumer;
using KafkaDocker.Data.Repository;
using KafkaDocker.Data.Helpers;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<ConsumerService>();
        services.AddTransient<IOrderRepository, OrderRepository>();
        services.AddScoped<KafkaConnection>();
    })
    .Build();

await host.RunAsync();
