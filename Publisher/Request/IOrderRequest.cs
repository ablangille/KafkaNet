using KafkaDocker.Data.Models;

namespace KafkaDocker.Publisher.Request
{
    public interface IOrderRequest
    {
        public Task<bool> SendOrderRequest(Order order);
    }
}
