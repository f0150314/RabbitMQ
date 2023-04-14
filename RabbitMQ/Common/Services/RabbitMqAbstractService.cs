using Common.Intefaces;
using RabbitMQ.Client;

namespace Common.Services;

public abstract class RabbitMqAbstractService : IDisposable
{
    private bool _disposed;
    private IConnection? _connection;
    private readonly IRabbitMqConnectionFactory? _rabbitMqConnectionFactory;

    protected IModel? _channel;
    protected readonly IValidation _validation = default!;

    public RabbitMqAbstractService(IRabbitMqConnectionFactory rabbitMqConnectionFactory, IValidation validation)
    {
        _rabbitMqConnectionFactory = rabbitMqConnectionFactory;
        _validation = validation;
    }

    public RabbitMqAbstractService EstablishConnectionToService()
    {
        CreateConnection();
        CreateChannel();
        return this;
    }

    protected virtual void CreateConnection()
    {
        _validation.CheckNull(_rabbitMqConnectionFactory, "has not been initialized!");

        ConnectionFactory connectionFactory = _rabbitMqConnectionFactory!.CreateConnectionFactory();
        _connection = connectionFactory.CreateConnection();
    }

    protected virtual void CreateChannel()
    {
        _validation.CheckNull(_connection, "has not been created!");
        _channel = _connection!.CreateModel();
    }

    public virtual QueueDeclareOk DeclareQueue(
        string queue = "",
        bool durable = false,
        bool exclusive = true,
        bool autoDelete = true,
        IDictionary<string, object>? arguments = null)
    {
        _validation.CheckNull(_channel, "has not been created!");

        return _channel!.QueueDeclare(
                    queue: queue,
                    durable: durable,
                    exclusive: exclusive,
                    autoDelete: autoDelete,
                    arguments: arguments);
    }

    public virtual void DeclareExchange(string exchange, string type)
    {
        _validation.CheckNull(_channel, "has not been created!");
        _channel!.ExchangeDeclare(exchange: exchange, type: type);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _channel?.Dispose();
            _connection?.Dispose();
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}