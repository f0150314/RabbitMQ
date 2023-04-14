using Common.Intefaces;
using Common.Services;

namespace DirectSender.Services;

internal class RabbitMqDirectSenderService : RabbitMqSenderAbstractService
{
    public RabbitMqDirectSenderService(IRabbitMqConnectionFactory rabbitMqConnectionFactory, IValidation validation)
        :base(rabbitMqConnectionFactory, validation) {}

    public override void PublishMessage(
        string exchange, 
        string routingKey,
        string? message = null)
    {
        message = message ?? $"'This message is coming from {nameof(RabbitMqDirectSenderService)}.'";
        base.PublishMessage(exchange, routingKey, message);
        Console.WriteLine($" [x] Sent {message}, routingKey: {routingKey}");
    }
}