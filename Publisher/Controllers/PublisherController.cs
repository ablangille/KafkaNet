using Microsoft.AspNetCore.Mvc;
using Confluent.Kafka;
using KafkaDocker.Data.Models;
using System.Text.Json;
using System.Net;

namespace KafkaDocker.Publisher.Controllers
{
    [Route("api/publisher")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly string _bootstrapServers = "localhost:9092";
        private readonly string _topic = "test";
        private readonly ILogger<PublisherController> _logger;

        public PublisherController(ILogger<PublisherController> logger)
        {
            _logger = logger;
        }

        [Route("send")]
        [HttpPost]
        public async Task<IActionResult> SendOrderRequest(OrderRequest orderRequest)
        {
            string message = JsonSerializer.Serialize(
                new Order
                {
                    Id = Guid.NewGuid(),
                    ProductId = orderRequest.ProductId,
                    CustomerId = orderRequest.CustomerId,
                    Quantity = orderRequest.Quantity,
                    Status = "Sent"
                }
            );

            if (await SendOrderRequest(_topic, message))
            {
                return Ok(JsonSerializer.Deserialize<Order>(message));
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error");
            }
        }

        private async Task<bool> SendOrderRequest(string topic, string message)
        {
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
                        topic,
                        new Message<Null, string> { Value = message }
                    );

                    _logger.LogInformation($"Delivery Timestamp: {result.Timestamp.UtcDateTime}");
                }

                return await Task.FromResult(true);
            }
            catch (KafkaException ex)
            {
                _logger.LogError(ex, $"Error occurred: {ex.Message}");
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, $"Error occurred: {ex.Message}");
            }
            catch (Exception)
            {
                throw;
            }

            return await Task.FromResult(false);
        }
    }
}
