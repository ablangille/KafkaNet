using Confluent.Kafka;
using KafkaDocker.Data.Models;
using System.Net;
using System.Text.Json;

namespace KafkaDocker.Publisher.Request
{
    public class OrderRequestHandler : IOrderRequest
    {
        private readonly string _bootstrapServers = "localhost:9092";
        private readonly string _topic = "test";
        private readonly ILogger<OrderRequestHandler> _logger;

        public OrderRequestHandler(ILogger<OrderRequestHandler> logger)
        {
            _logger = logger;
        }

        public async Task<bool> SendOrderRequest(Order order)
        {
            string message = JsonSerializer.Serialize(order);

            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers,
                ClientId = Dns.GetHostName()
            };

            try
            {
                using (var producer = new ProducerBuilder<Null, string>(config).Build())
                {
                    var result = await producer.ProduceAsync(
                        _topic,
                        new Message<Null, string> { Value = message }
                    );

                    _logger.LogInformation(
                        $"Info: Delivery Timestamp: {result.Timestamp.UtcDateTime}"
                    );
                }

                return await Task.FromResult(true);
            }
            catch (ProduceException<Null, string> ex)
            {
                _logger.LogError(ex, $"Error occurred: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, $"Error occurred: {ex.Message}");
            }

            return await Task.FromResult(false);
        }
    }
}
