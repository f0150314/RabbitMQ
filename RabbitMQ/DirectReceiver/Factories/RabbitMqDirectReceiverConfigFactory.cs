using Common.Intefaces;
using Common.Models;
using Microsoft.Extensions.Configuration;

namespace DirectReceiver.Factories;

internal class RabbitMqDirectReceiverConfigFactory : IRabbitMqConfigFactory<IRabbitMqConfig>
{
    private readonly IConfiguration _config;

    public RabbitMqDirectReceiverConfigFactory(IConfiguration config)
        => _config = config;

    public IRabbitMqConfig CreateConfig()
        => new RabbitMqConfig(_config);
}