using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace KafkaDocker.Data.Persistence
{
    public class KafkaDockerDbContextFactory : IDesignTimeDbContextFactory<KafkaDockerDbContext>
    {
        public KafkaDockerDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<KafkaDockerDbContext>();
            optionsBuilder.UseNpgsql(
                Environment.GetEnvironmentVariable("TestApi_ConnectionString")
            );

            return new KafkaDockerDbContext(optionsBuilder.Options);
        }
    }
}
