using Common.Intefaces;
using Common.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace QueueReceiver.Services;

internal class RabbitMqQueueReceiverService : RabbitMqReceiverAbstractService
{
    public RabbitMqQueueReceiverService(IRabbitMqConnectionFactory rabbitMqConnectionFactory, IValidation validation)
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
            Console.WriteLine($" [x] Received {message}");

            int dots = message.Split('.').Length - 1;
            if (dots == 5)
            {
                Thread.Sleep(dots * 100);
            }
            else
            {
                Thread.Sleep(dots * 300);
            }

            Console.WriteLine(" [x} Done");

            IModel channel = ((EventingBasicConsumer)sender!).Model;
            channel.BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            Console.Error.WriteLine($"Stack Trace: {ex.StackTrace}");
        }
    }
}