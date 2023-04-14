using Common.Intefaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace Common.Services;

public abstract class RabbitMqReceiverAbstractService : RabbitMqAbstractService
{
    protected EventingBasicConsumer? _consumer;

    protected RabbitMqReceiverAbstractService(IRabbitMqConnectionFactory rabbitMqConnectionFactory, IValidation validation) 
        : base(rabbitMqConnectionFactory, validation) { }

    public virtual void SetBasicQos(uint prefetchSize, ushort prefetchCount, bool global)
    {
        _validation.CheckNull(_channel, "has not been created!");
        _channel!.BasicQos(prefetchSize, prefetchCount, global);
    }

    public virtual void BindQueue(string queue, string exchange, string routingKey, IDictionary<string, object>? arguments = default)
    {
        _validation.CheckNull(_channel, "has not been created!");
        _channel!.QueueBind(queue: queue, exchange: exchange, routingKey: routingKey, arguments: arguments);
    }

    public abstract void AddConsumerReceivedEvent();

    protected abstract void ConsumerReceivedEvent(object sender, BasicDeliverEventArgs eventArgs);

    public virtual void ConsumeMessages(string queue, bool autoAck)
    {
        _validation.CheckNull(_channel, "has not been created!");

        _channel.BasicConsume(
            queue: queue,
            autoAck: autoAck,
            consumer: _consumer);
    }
}