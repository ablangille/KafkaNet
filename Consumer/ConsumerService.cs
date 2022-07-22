using Confluent.Kafka;
using System.Text.Json;

namespace Consumer
{
    public class ConsumerService : BackgroundService
    {
        private readonly string _topic = "test";
        private readonly string _groupId = "test_group";
        private readonly string _bootstrapServers = "localhost:9092";
        private readonly ILogger<ConsumerService> _logger;

        public ConsumerService(ILogger<ConsumerService> logger)
        {
            _logger = logger;
        }

        protected override Task ExecuteAsync(CancellationToken cancellationToken)
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
                        _logger.LogInformation($"Processing Order Id: {order?.Id}");
                    }
                }
                catch (ConsumeException ex)
                {
                    _logger.LogError(ex, $"Error occurred: {ex.Message}");
                }
                catch (OperationCanceledException ex)
                {
                    _logger.LogError(ex, $"Error occurred: {ex.Message}");
                }

                consumerBuilder.Close();
            }

            return Task.CompletedTask;
        }
    }
}
