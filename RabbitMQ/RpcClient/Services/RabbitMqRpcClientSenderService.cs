using Common.Intefaces;
using Common.Services;
using System.Collections.Concurrent;

namespace RpcClient.Services;

internal class RabbitMqRpcClientSenderService : RabbitMqSenderAbstractService
{
    public string CorrelationId { private get; set; } = string.Empty;

    public ConcurrentDictionary<string, TaskCompletionSource<string>> CallbackMapper { private get; set; } = new();

    public RabbitMqRpcClientSenderService(IRabbitMqConnectionFactory rabbitMqConnectionFactory, IValidation validation)
        :base(rabbitMqConnectionFactory, validation) {}

    public Task<string> CallAsync(string exchange, string routingKey, string message, CancellationToken token)
    {
        var tcs = new TaskCompletionSource<string>();
        CallbackMapper.TryAdd(CorrelationId, tcs);

        base.PublishMessage(exchange, routingKey, message);

        token.Register(() => CallbackMapper.TryRemove(CorrelationId, out _));
        return tcs.Task;
    }
}