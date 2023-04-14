using Common.Intefaces;
using Common.Services;

namespace QueueSender.Services;

internal class RabbitMqQueueSenderService : RabbitMqSenderAbstractService
{
    public RabbitMqQueueSenderService(IRabbitMqConnectionFactory rabbitMqConnectionFactory, IValidation validation)
        :base(rabbitMqConnectionFactory, validation) {}

    public override void PublishMessage(
        string exchange, 
        string routingKey,
        string? message = null)
    {
        int times = 10;

        for (int i = 1; i <= times; i++)
        {
            Task.Delay(300).Wait();
            string newMessage = message ?? BuildMessage(i);
            base.PublishMessage(exchange, routingKey, newMessage);
            Console.WriteLine($" [x] Sent {newMessage}");
        }
    }

    private string BuildMessage(int i)
    {
        string message = $"{i} message{(i == 1 ? string.Empty : "s")}";

        while (i > 0)
        {
            message += ".";
            i--;
        }

        return message;
    }
}