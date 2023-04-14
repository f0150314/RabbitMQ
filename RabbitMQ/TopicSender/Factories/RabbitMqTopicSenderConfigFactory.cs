using Common.Intefaces;
using Common.Models;
using Microsoft.Extensions.Configuration;

namespace TopicSender.Factories;

internal class RabbitMqTopicSenderConfigFactory : IRabbitMqConfigFactory<IRabbitMqConfig>
{
    private readonly IConfiguration _config;

    public RabbitMqTopicSenderConfigFactory(IConfiguration config) 
        => _config = config;

    public IRabbitMqConfig CreateConfig() 
        => new RabbitMqConfig(_config);
}