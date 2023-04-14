using Common.Intefaces;
using Common.Models;
using Microsoft.Extensions.Configuration;

namespace RpcClient.Factories;

internal class RabbitMqRpcClientConfigFactory : IRabbitMqConfigFactory<IRabbitMqConfig>
{
    private readonly IConfiguration _config;

    public RabbitMqRpcClientConfigFactory(IConfiguration config)
        => _config = config;

    public IRabbitMqConfig CreateConfig()
        => new RabbitMqConfig(_config);
}