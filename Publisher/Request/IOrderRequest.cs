using KafkaDocker.Data.Models;

namespace KafkaDocker.Publisher.Request
{
    public interface IOrderRequest
    {
        public Task<RequestResponse> SendOrderRequest(Order order);
    }
}
