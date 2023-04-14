using Common.Intefaces;
using Common.Services;
using RabbitMQ.Client.Events;
using System.Collections.Concurrent;
using System.Text;

namespace RpcClient.Services;

internal class RabbitMqRpcClientReceiverService : RabbitMqReceiverAbstractService
{
    public ConcurrentDictionary<string, TaskCompletionSource<string>> CallbackMapper { private get; set; } = new();

    public RabbitMqRpcClientReceiverService(IRabbitMqConnectionFactory rabbitMqConnectionFactory, IValidation validation)
        : base(rabbitMqConnectionFactory, validation) { }

    public override void AddConsumerReceivedEvent()
    {
        _validation.CheckNull(_channel, "has not been created!");

        _consumer = new EventingBasicConsumer(_channel);
        _consumer.Received += ConsumerReceivedEvent!;
    }

    protected override void ConsumerReceivedEvent(object sender, BasicDeliverEventArgs eventArgs)
    {
        try
        {
            if (!CallbackMapper.TryRemove(eventArgs.BasicProperties.CorrelationId, out var tcs))
                return;

            byte[] body = eventArgs.Body.ToArray();
            string response = Encoding.UTF8.GetString(body);
            tcs.TrySetResult(response);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            Console.Error.WriteLine($"Stack Trace: {ex.StackTrace}");
        }
    }
}