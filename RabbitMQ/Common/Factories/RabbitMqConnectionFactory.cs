using Common.Intefaces;
using RabbitMQ.Client;

namespace Common.Factories;

public class RabbitMqConnectionFactory : IRabbitMqConnectionFactory
{
    private readonly IRabbitMqConfigFactory<IRabbitMqConfig> _configFactory;
    private IRabbitMqConfig Config => _configFactory.CreateConfig();

    public RabbitMqConnectionFactory(IRabbitMqConfigFactory<IRabbitMqConfig> configFactory)
    {
        _configFactory = configFactory;
    }

    public ConnectionFactory CreateConnectionFactory()
    {
        return new ConnectionFactory { HostName = Config.RabbitMqHostName };
    }
}