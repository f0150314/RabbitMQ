using Common.Intefaces;
using Common.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace TopicReceiver.Services;

internal class RabbitMqTopicReceiverService : RabbitMqReceiverAbstractService
{
    public RabbitMqTopicReceiverService(IRabbitMqConnectionFactory rabbitMqConnectionFactory, IValidation validation)
        : base(rabbitMqConnectionFactory, validation) { }

    public override void AddConsumerReceivedEvent()
    {
        _validation.CheckNull(_channel, "has not been created!");

        Console.WriteLine(" [*] Waiting for messages.");
        _consumer = new EventingBasicConsumer(_channel);
        _consumer.Received += ConsumerReceivedEvent!;
    }

    protected override void ConsumerReceivedEvent(object sender, BasicDeliverEventArgs eventArgs)
    {
        try
        {
            byte[] body = eventArgs.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);
            string routingKey = eventArgs.RoutingKey;
            Console.WriteLine($" [x] Received {message}, routingKey: {routingKey}");
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            Console.Error.WriteLine($"Stack Trace: {ex.StackTrace}");
        }
    }
}