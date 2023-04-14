using Common.Intefaces;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace Common.Services;

public abstract class RabbitMqSenderAbstractService : RabbitMqAbstractService
{
    protected IBasicProperties? _properties;
    protected ConcurrentDictionary<ulong, string> _outstandingConfirms = new();
    protected ulong NextPublishSeqNo => _channel?.NextPublishSeqNo ?? 0;

    private bool _enabledPublishConfirmed;
    public bool EnablePublishConfirmed 
    {
        private get => _enabledPublishConfirmed;
        set
        {
            _enabledPublishConfirmed = value;

            if (_enabledPublishConfirmed)
            {
                _validation.CheckNull(_channel, "has not been created!");
                _channel!.ConfirmSelect();
                RegisterBasicAcks();
                RegisterBasicNacks();
            }
            else
            {
                Dispose();
                EstablishConnectionToService();
            }
        }
    }

    protected RabbitMqSenderAbstractService(IRabbitMqConnectionFactory rabbitMqConnectionFactory, IValidation validation) 
        : base(rabbitMqConnectionFactory, validation) { }

    private void RegisterBasicAcks()
    {
        _validation.CheckNull(_channel, "has not been created!");
        _channel!.BasicAcks += BasicAcksEvent;
    }

    private void RegisterBasicNacks()
    {
        _validation.CheckNull(_channel, "has not been created!");
        _channel!.BasicNacks += BasicNacksEvent;
    }

    protected virtual void BasicAcksEvent(object? sender, BasicAckEventArgs e)
    {
        bool isSingular = _outstandingConfirms.Count == 1;
        Console.WriteLine($"Total {_outstandingConfirms.Count} message{(isSingular ? " was" : "s were")} publish confirmed.");
        CleanOutstandingConfirms(e);
    }

    protected virtual void BasicNacksEvent(object? sender, BasicNackEventArgs e)
    {
        _outstandingConfirms.TryGetValue(e.DeliveryTag, out string? body);
        Console.WriteLine($"Message with body {body ?? "null"} has been nack-ed. Sequence number: {e.DeliveryTag}, multiple: {e.Multiple}");
        CleanOutstandingConfirms(e);
    }

    private void CleanOutstandingConfirms(BasicAckEventArgs e) 
        => CleanOutstandingConfirms(e.DeliveryTag, e.Multiple);
    private void CleanOutstandingConfirms(BasicNackEventArgs e) 
        => CleanOutstandingConfirms(e.DeliveryTag, e.Multiple);

    protected virtual void CleanOutstandingConfirms(ulong sequenceNumber, bool multiple)
    {
        if (!multiple)
        {
            _outstandingConfirms.TryRemove(sequenceNumber, out _);
        }
        else
        {
            var confirmed = _outstandingConfirms.Where(c => c.Key <= sequenceNumber);
            foreach (var confirm in confirmed)
                _outstandingConfirms.TryRemove(confirm.Key, out _);
        }
    }

    public virtual void CreateBasicProperties(Action<IBasicProperties>? action)
    {
        CreateBasicProperties();

        if (action == null)
            return;

        _validation.CheckNull(_properties, "have not been created!");
        action(_properties!);
    }

    private void CreateBasicProperties()
    {
        _validation.CheckNull(_channel, "has not been created!");
        _properties = _channel!.CreateBasicProperties();
    }

    public virtual void PublishMessage(
        string exchange, 
        string routingKey, 
        string? message = null)
    {
        if (message is null)
            return;

        _validation.CheckNull(_channel, "has not been created!");

        if (EnablePublishConfirmed)
            _outstandingConfirms.TryAdd(NextPublishSeqNo, message);

        byte[] body = Encoding.UTF8.GetBytes(message);

        _channel!.BasicPublish(
            exchange: exchange,
            routingKey: routingKey,
            basicProperties: _properties,
            body: body);
    }
}