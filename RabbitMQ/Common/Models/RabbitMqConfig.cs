using Common.Intefaces;
using Microsoft.Extensions.Configuration;

namespace Common.Models;

public class RabbitMqConfig : IRabbitMqConfig
{
	private readonly IConfiguration _config;

	public RabbitMqConfig(IConfiguration config)
	{
        _config = config;
    }

    public string? RabbitMqHostName => _config[nameof(RabbitMqHostName).ToLower()];
}