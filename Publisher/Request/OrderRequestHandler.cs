using Confluent.Kafka;
using KafkaDocker.Data.Models;
using KafkaDocker.Data.Helpers;
using System.Net;
using System.Text.Json;

namespace KafkaDocker.Publisher.Request
{
    public class OrderRequestHandler : IOrderRequest
    {
        private readonly string _bootstrapServers = Environment.GetEnvironmentVariable("BrokerUrl");
        private readonly string _topic = "test";
        private readonly ILogger<OrderRequestHandler> _logger;
        private readonly KafkaConnection _kafkaConnection;

        public OrderRequestHandler(
            ILogger<OrderRequestHandler> logger,
            KafkaConnection kafkaConnection
        )
        {
            _logger = logger;
            _kafkaConnection = kafkaConnection;
        }

        public async Task<RequestResponse> SendOrderRequest(Order order)
        {
            bool connected = await _kafkaConnection.CheckConnection();

            if (!connected)
            {
                return await Task.FromResult(
                    new RequestResponse
                    {
                        Status = StatusCodes.Status503ServiceUnavailable,
                        Message = "Connection failed"
                    }
                );
            }

            string message = JsonSerializer.Serialize(order);

            ProducerConfig config = new ProducerConfig
            {
                BootstrapServers = _bootstrapServers,
                ClientId = Dns.GetHostName()
            };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                    var result = await producer.ProduceAsync(
                        _topic,
                        new Message<Null, string> { Value = message }
                    );

                    _logger.LogInformation(
                        $"Info: Delivery Timestamp: {result.Timestamp.UtcDateTime}"
                    );
                }
                catch (ArgumentException ex)
                {
                    _logger.LogError(ex, $"Error occurred: {ex.Message}");
                    producer.AbortTransaction();
                    return await Task.FromResult(
                        new RequestResponse
                        {
                            Status = StatusCodes.Status400BadRequest,
                            Message = $"{ex.Message}"
                        }
                    );
                }
            }

            return await Task.FromResult(
                new RequestResponse { Status = StatusCodes.Status201Created }
            );
        }
    }
}
