using Common.Intefaces;
using Common.Services;

namespace TopicSender.Services;

internal class RabbitMqTopicSenderService : RabbitMqSenderAbstractService
{
    public RabbitMqTopicSenderService(IRabbitMqConnectionFactory rabbitMqConnectionFactory, IValidation validation)
        :base(rabbitMqConnectionFactory, validation) {}

    public override void PublishMessage(
        string exchange, 
        string routingKey,
        string? message = null)
    {
        message = message ?? $"'This message is coming from {nameof(RabbitMqTopicSenderService)}.'";
        base.PublishMessage(exchange, routingKey, message);
        Console.WriteLine($" [x] Sent {message}, routingKey: {routingKey}");
    }
}