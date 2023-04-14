using Common.Intefaces;
using Common.Services;
using RabbitMQ.Client.Events;
using System.Text;

namespace FanoutReceiver.Services;

internal class RabbitMqFanoutReceiverService : RabbitMqReceiverAbstractService
{
    public RabbitMqFanoutReceiverService(IRabbitMqConnectionFactory rabbitMqConnectionFactory, IValidation validation)
        : base(rabbitMqConnectionFactory, validation) { }

    public override void AddConsumerReceivedEvent()
    {
        _validation.CheckNull(_channel, "has not been created!");

        Console.WriteLine(" [*] Waiting for logs.");
        _consumer = new EventingBasicConsumer(_channel);
        _consumer.Received += ConsumerReceivedEvent!;
    }

    protected override void ConsumerReceivedEvent(object sender, BasicDeliverEventArgs eventArgs)
    {
        try
        {
            byte[] body = eventArgs.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);
            Console.WriteLine($" [x] Received {message}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            Console.Error.WriteLine($"Stack Trace: {ex.StackTrace}");
        }
    }
}