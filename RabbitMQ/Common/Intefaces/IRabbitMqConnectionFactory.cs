using RabbitMQ.Client;

namespace Common.Intefaces;
public interface IRabbitMqConnectionFactory
{
    ConnectionFactory CreateConnectionFactory();
}