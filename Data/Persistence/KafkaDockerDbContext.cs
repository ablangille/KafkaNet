using Microsoft.EntityFrameworkCore;
using KafkaDocker.Data.Models;

namespace KafkaDocker.Data.Persistence
{
    public class KafkaDockerDbContext : DbContext
    {
        public KafkaDockerDbContext(DbContextOptions options) : base(options) { }

        public virtual DbSet<Order> Orders { get; set; }
    }
}
