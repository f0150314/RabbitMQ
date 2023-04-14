using Common.Intefaces;
using Common.Others;
using Common.Services;
using RpcClient.Services;
using System.Collections.Concurrent;
using System.Text;
using System.Threading.Channels;

namespace RpcClient;

internal class RemoteProcedureCallClient : IDisposable
{
    private bool _disposed;
    private readonly IValidation? _validation;
    private readonly RabbitMqRpcClientReceiverService? _receiverService;
    private readonly RabbitMqRpcClientSenderService? _senderService;
    private string _replyQueueName = string.Empty;

    private ConcurrentDictionary<string, TaskCompletionSource<string>> _callbackMapper = new();

    public RemoteProcedureCallClient(RabbitMqRpcClientSenderService senderService, RabbitMqRpcClientReceiverService receiverService, IValidation validation)
    {
        _senderService = senderService;
        _receiverService = receiverService;
        _validation = validation;
        Initialize();
    }

    private void Initialize()
    {
        _validation!.CheckNull(_receiverService, "has not been established!");

        _receiverService!.EstablishConnectionToService();
        _receiverService.CallbackMapper = _callbackMapper;

        _replyQueueName = _receiverService.DeclareQueue().QueueName;
        _receiverService.AddConsumerReceivedEvent();
        _receiverService.ConsumeMessages(queue: _replyQueueName, autoAck: true);
    }

    public async Task<string> CallAsync(string message, CancellationToken token = default)
    {
        _validation!.CheckNull(_senderService, "has not been established!");
        _senderService!.EstablishConnectionToService();
        _senderService.CallbackMapper = _callbackMapper;

        string CorrelationId = Guid.NewGuid().ToString();
        _senderService.CorrelationId = CorrelationId;
        _senderService.CreateBasicProperties(p =>
        {
            p.CorrelationId = CorrelationId;
            p.ReplyTo = _replyQueueName;
        });

        return await _senderService.CallAsync(
            exchange: string.Empty, 
            routingKey: AppConstants.RoutingKey_Rpc, 
            message: message, 
            token: token);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (_disposed)
            return;

        if (disposing)
        {
            _senderService?.Dispose();
            _receiverService?.Dispose();
        }

        _disposed = true;
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
}