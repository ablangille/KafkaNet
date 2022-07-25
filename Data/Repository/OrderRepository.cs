using KafkaDocker.Data.Models;
using KafkaDocker.Data.Persistence;
using Microsoft.EntityFrameworkCore;

namespace KafkaDocker.Data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        public OrderRepository() { }

        public Order GetOrder(Guid id)
        {
            using (var context = new KafkaDockerDbContext())
            {
                return context.Orders.Find(id);
            }
        }

        public IEnumerable<Order> GetOrders()
        {
            using (var context = new KafkaDockerDbContext())
            {
                return context.Orders.ToList();
            }
        }

        public Order AddOrder(Order order)
        {
            using (var context = new KafkaDockerDbContext())
            {
                context.Orders.Add(order);
                context.SaveChanges();
                return order;
            }
        }

        public Order UpdateOrder(Order order)
        {
            using (var context = new KafkaDockerDbContext())
            {
                context.Entry(order).State = EntityState.Modified;
                context.SaveChanges();
                return order;
            }
        }

        public Order DeleteOrder(Guid id)
        {
            using (var context = new KafkaDockerDbContext())
            {
                Order order = context.Orders.Find(id);

                if (order == null)
                {
                    return null;
                }

                context.Orders.Remove(order);
                context.SaveChanges();
                return order;
            }
        }
    }
}
