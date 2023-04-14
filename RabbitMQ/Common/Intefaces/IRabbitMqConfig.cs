namespace Common.Intefaces;

public interface IRabbitMqConfig : IConfig
{
    string? RabbitMqHostName { get; }
}