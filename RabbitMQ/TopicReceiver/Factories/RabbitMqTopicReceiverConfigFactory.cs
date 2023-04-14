using Common.Intefaces;
using Common.Models;
using Microsoft.Extensions.Configuration;

namespace TopicReceiver.Factories;

internal class RabbitMqTopicReceiverConfigFactory : IRabbitMqConfigFactory<IRabbitMqConfig>
{
    private readonly IConfiguration _config;

    public RabbitMqTopicReceiverConfigFactory(IConfiguration config)
        => _config = config;

    public IRabbitMqConfig CreateConfig()
        => new RabbitMqConfig(_config);
}