using Common.Intefaces;
using Common.Models;
using Microsoft.Extensions.Configuration;

namespace QueueSender.Factories;

internal class RabbitMqQueueSenderConfigFactory : IRabbitMqConfigFactory<IRabbitMqConfig>
{
    private readonly IConfiguration _config;

    public RabbitMqQueueSenderConfigFactory(IConfiguration config) 
        => _config = config;

    public IRabbitMqConfig CreateConfig() 
        => new RabbitMqConfig(_config);
}