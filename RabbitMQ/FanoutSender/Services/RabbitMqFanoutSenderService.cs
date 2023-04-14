using Common.Intefaces;
using Common.Services;

namespace FanoutSender.Services;

internal class RabbitMqFanoutSenderService : RabbitMqSenderAbstractService
{
    public RabbitMqFanoutSenderService(IRabbitMqConnectionFactory rabbitMqConnectionFactory, IValidation validation)
        :base(rabbitMqConnectionFactory, validation) {}

    public override void PublishMessage(
        string exchange, 
        string routingKey,
        string? message = null)
    {
        message = message ?? $"'This message is coming from {nameof(RabbitMqFanoutSenderService)}.'";
        base.PublishMessage(exchange, routingKey, message);
        Console.WriteLine($" [x] Sent {message}");
    }
}