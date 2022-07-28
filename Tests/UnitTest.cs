using Xunit;
using Moq;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using KafkaDocker.Publisher.Request;
using KafkaDocker.Data.Models;
using KafkaDocker.Data.Helpers;

namespace Tests
{
    public class UnitTest
    {
        [Fact]
        public async Task HandleSendOrderOk()
        {
            // Arrange
            var mockupLogger = new Mock<ILogger<OrderRequestHandler>>();
            var mockupKafkaConnection = new Mock<KafkaConnection>();

            var handler = new OrderRequestHandler(
                mockupLogger.Object,
                mockupKafkaConnection.Object
            );
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = 1,
                ProductId = 1,
                Quantity = 1,
                Status = "Sent"
            };

            // Act
            var result = await handler.SendOrderRequest(order);

            // Assert
            Assert.Equal(StatusCodes.Status201Created, result.Status);
        }
    }
}
