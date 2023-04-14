using Common.Intefaces;
using Common.Models;
using Microsoft.Extensions.Configuration;

namespace PublishConfirmReceiver.Factories;

internal class RabbitMqPublishConfirmReceiverConfigFactory : IRabbitMqConfigFactory<IRabbitMqConfig>
{
    private readonly IConfiguration _config;

    public RabbitMqPublishConfirmReceiverConfigFactory(IConfiguration config)
        => _config = config;

    public IRabbitMqConfig CreateConfig()
        => new RabbitMqConfig(_config);
}