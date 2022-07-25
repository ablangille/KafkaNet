using Microsoft.EntityFrameworkCore;
using KafkaDocker.Data.Models;

namespace KafkaDocker.Data.Persistence
{
    public class KafkaDockerDbContext : DbContext
    {
        private readonly string _dbConnectionString = Environment.GetEnvironmentVariable(
            "TestApi_ConnectionString"
        );

        public KafkaDockerDbContext(DbContextOptions options) : base(options) { }

        public KafkaDockerDbContext() { }

        public virtual DbSet<Order> Orders { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql(_dbConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Order>().Property(o => o.Quantity).IsRequired().HasMaxLength(3);
            modelBuilder.Entity<Order>().Property(o => o.CustomerId).IsRequired();
            modelBuilder.Entity<Order>().Property(o => o.ProductId).IsRequired();
        }
    }
}
