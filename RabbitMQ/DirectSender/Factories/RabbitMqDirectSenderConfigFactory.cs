using Common.Intefaces;
using Common.Models;
using Microsoft.Extensions.Configuration;

namespace DirectSender.Factories;

internal class RabbitMqDirectSenderConfigFactory : IRabbitMqConfigFactory<IRabbitMqConfig>
{
    private readonly IConfiguration _config;

    public RabbitMqDirectSenderConfigFactory(IConfiguration config)
        => _config = config;

    public IRabbitMqConfig CreateConfig()
        => new RabbitMqConfig(_config);
}