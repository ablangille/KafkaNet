using KafkaDocker.Data.Models;

namespace KafkaDocker.Publisher.Request
{
    public interface IOrderRequestHandler
    {
        public Task<RequestResponse> SendOrderRequest(Order order);
    }
}
