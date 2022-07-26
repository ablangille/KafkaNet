using Confluent.Kafka;
using Confluent.Kafka.Admin;
using System.Net;

namespace KafkaDocker.Publisher.Helpers
{
    public class TopicCreator
    {
        private readonly ILogger<TopicCreator> _logger;

        public TopicCreator(ILogger<TopicCreator> logger)
        {
            _logger = logger;
        }

        public async Task<bool> CreateTopic(
            string name,
            int numPartitions,
            short replicationFactor,
            string brokerUrl
        )
        {
            ClientConfig config = new ClientConfig
            {
                BootstrapServers = brokerUrl,
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
                catch (CreateTopicsException e)
                {
                    if (e.Results[0].Error.Code != ErrorCode.TopicAlreadyExists)
                    {
                        _logger.LogError(
                            $"An error occured creating topic {name}: {e.Results[0].Error.Reason}"
                        );
                    }
                    else
                    {
                        _logger.LogWarning("Topic already exists");
                    }
                }
                catch (KafkaException e)
                {
                    _logger.LogError(e, $"Error occurred: {e.Message}");
                    return await Task.FromResult(false);
                }
            }

            return await Task.FromResult(true);
        }
    }
}
