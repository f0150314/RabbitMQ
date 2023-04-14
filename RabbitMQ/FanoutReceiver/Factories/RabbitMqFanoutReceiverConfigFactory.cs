using Common.Intefaces;
using Common.Models;
using Microsoft.Extensions.Configuration;

namespace FanoutReceiver.Factories;

internal class RabbitMqFanoutReceiverConfigFactory : IRabbitMqConfigFactory<IRabbitMqConfig>
{
    private readonly IConfiguration _config;

    public RabbitMqFanoutReceiverConfigFactory(IConfiguration config)
        => _config = config;

    public IRabbitMqConfig CreateConfig()
        => new RabbitMqConfig(_config);
}