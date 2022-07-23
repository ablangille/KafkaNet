using Data.Models;

namespace Data.Repository
{
    public interface IOrderRepository
    {
        public Order GetOrder(Guid id);
        public IEnumerable<Order> GetOrders();
        public Order AddOrder(Order order);
        public Order UpdateOrder(Order order);
        public Order DeleteOrder(Guid id);
    }
}
