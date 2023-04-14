using Common.Intefaces;
using Common.Models;
using Microsoft.Extensions.Configuration;

namespace PublishConfirmSender.Factories;

internal class RabbitMqPublishConfirmSenderConfigFactory : IRabbitMqConfigFactory<IRabbitMqConfig>
{
    private readonly IConfiguration _config;

    public RabbitMqPublishConfirmSenderConfigFactory(IConfiguration config)
        => _config = config;

    public IRabbitMqConfig CreateConfig()
        => new RabbitMqConfig(_config);
}