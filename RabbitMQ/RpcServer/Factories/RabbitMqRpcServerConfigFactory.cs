using Common.Intefaces;
using Common.Models;
using Microsoft.Extensions.Configuration;

namespace RpcServer.Factories;

internal class RabbitMqRpcServerConfigFactory : IRabbitMqConfigFactory<IRabbitMqConfig>
{
    private readonly IConfiguration _config;

    public RabbitMqRpcServerConfigFactory(IConfiguration config)
        => _config = config;

    public IRabbitMqConfig CreateConfig()
        => new RabbitMqConfig(_config);
}