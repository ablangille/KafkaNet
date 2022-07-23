using Data.Models;
using Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly KafkaDockerDbContext _dbContext;

        public OrderRepository(KafkaDockerDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public Order GetOrder(Guid id)
        {
            return _dbContext.Orders.Find(id);
        }

        public IEnumerable<Order> GetOrders()
        {
            return _dbContext.Orders.ToList();
        }

        public Order AddOrder(Order order)
        {
            _dbContext.Orders.Add(order);
            _dbContext.SaveChanges();
            return order;
        }

        public Order UpdateOrder(Order order)
        {
            _dbContext.Entry(order).State = EntityState.Modified;
            _dbContext.SaveChanges();
            return order;
        }

        public Order DeleteOrder(Guid id)
        {
            Order order = _dbContext.Orders.Find(id);

            if (order != null)
            {
                _dbContext.Orders.Remove(order);
                _dbContext.SaveChanges();
                return order;
            }
            else
            {
                return null;
            }
        }
    }
}
