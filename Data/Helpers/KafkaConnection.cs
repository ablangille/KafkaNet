using Confluent.Kafka;
using Confluent.Kafka.Admin;
using System.Net;

namespace KafkaDocker.Data.Helpers
{
    public class KafkaConnection
    {
        private readonly string _bootstrapServers = Environment.GetEnvironmentVariable("BrokerUrl");

        public KafkaConnection() { }

        public async Task<bool> CreateTopic(string name, int numPartitions, short replicationFactor)
        {
            ClientConfig config = new ClientConfig
            {
                BootstrapServers = _bootstrapServers,
                ClientId = Dns.GetHostName(),
                SocketMaxFails = 1
            };

            using (var adminClient = new AdminClientBuilder(config).Build())
            {
                try
                {
                    await adminClient.CreateTopicsAsync(
                        new List<TopicSpecification>
                        {
                            new TopicSpecification
                            {
                                Name = name,
                                NumPartitions = numPartitions,
                                ReplicationFactor = replicationFactor
                            }
                        }
                    );
                }
                catch (KafkaException)
                {
                    return await Task.FromResult(false);
                }
            }

            return await Task.FromResult(true);
        }

        public async Task<bool> CheckConnection()
        {
            ClientConfig config = new ClientConfig
            {
                BootstrapServers = _bootstrapServers,
                ClientId = Dns.GetHostName()
            };

            using (var adminClient = new AdminClientBuilder(config).Build())
            {
                try
                {
                    adminClient.GetMetadata(TimeSpan.FromSeconds(1));
                }
                catch (KafkaException)
                {
                    return await Task.FromResult(false);
                }
            }

            return await Task.FromResult(true);
        }
    }
}
