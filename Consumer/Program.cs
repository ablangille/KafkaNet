using Consumer;

IHost host = Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddHostedService<ConsumerService>();
    })
    .Build();

await host.RunAsync();
