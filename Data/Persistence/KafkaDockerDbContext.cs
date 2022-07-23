using Microsoft.EntityFrameworkCore;
using Data.Models;

namespace Data.Persistence
{
    public class KafkaDockerDbContext : DbContext
    {
        public KafkaDockerDbContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<Order> Orders { get; set; }
    }
}
