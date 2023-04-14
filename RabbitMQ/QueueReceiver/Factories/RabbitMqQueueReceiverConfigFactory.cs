using Common.Intefaces;
using Common.Models;
using Microsoft.Extensions.Configuration;

namespace QueueReceiver.Factories;

internal class RabbitMqQueueReceiverConfigFactory : IRabbitMqConfigFactory<IRabbitMqConfig>
{
    private readonly IConfiguration _config;

    public RabbitMqQueueReceiverConfigFactory(IConfiguration config)
        => _config = config;

    public IRabbitMqConfig CreateConfig()
        => new RabbitMqConfig(_config);
}