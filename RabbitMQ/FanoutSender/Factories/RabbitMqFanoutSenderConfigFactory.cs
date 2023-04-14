using Common.Intefaces;
using Common.Models;
using Microsoft.Extensions.Configuration;

namespace FanoutSender.Factories;

internal class RabbitMqFanoutSenderConfigFactory : IRabbitMqConfigFactory<IRabbitMqConfig>
{
    private readonly IConfiguration _config;

    public RabbitMqFanoutSenderConfigFactory(IConfiguration config)
        => _config = config;

    public IRabbitMqConfig CreateConfig()
        => new RabbitMqConfig(_config);
}