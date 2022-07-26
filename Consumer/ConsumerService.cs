using Confluent.Kafka;
using System.Text.Json;
using KafkaDocker.Data.Models;
using KafkaDocker.Data.Repository;
using KafkaDocker.Data.Persistence;

namespace KafkaDocker.Consumer
{
    public class ConsumerService : BackgroundService
    {
        private readonly string _topic = "test";
        private readonly string _groupId = "test_group";
        private readonly string _bootstrapServers = "localhost:9092";
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly ILogger<ConsumerService> _logger;

        public ConsumerService(ILogger<ConsumerService> logger, IServiceScopeFactory scopeFactory)
        {
            _logger = logger;
            _scopeFactory = scopeFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            var config = new ConsumerConfig
            {
                GroupId = _groupId,
                BootstrapServers = _bootstrapServers,
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var consumerBuilder = new ConsumerBuilder<Ignore, string>(config).Build())
            {
                consumerBuilder.Subscribe(_topic);

                try
                {
                    while (true)
                    {
                        var consumer = consumerBuilder.Consume(cancellationToken);
                        var order = JsonSerializer.Deserialize<Order>(consumer.Message.Value);
                        _logger.LogInformation($"Info: Processing Order Id: {order?.Id}");

                        order.Status = "Confirmed";

                        using (var scope = _scopeFactory.CreateScope())
                        {
                            var orderRepository =
                                scope.ServiceProvider.GetRequiredService<IOrderRepository>();
                            await Task.FromResult(orderRepository.AddOrder(order));
                        }
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, $"Error occurred: {ex.Message}");
                }
                catch (KafkaException ex)
                {
                    _logger.LogError(ex, $"Error occurred: {ex.Message}");
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogInformation(ex, $"Info: {ex.Message}");
                }

                consumerBuilder.Close();
            }
        }
    }
}
