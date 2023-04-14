using Common.Intefaces;
using Common.Others;
using Common.Services;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace RpcServer.Services;

internal class RabbitMqRpcServerService : RabbitMqReceiverAbstractService
{
    public RabbitMqRpcServerService(IRabbitMqConnectionFactory rabbitMqConnectionFactory, IValidation validation)
        : base(rabbitMqConnectionFactory, validation) { }

    public override void AddConsumerReceivedEvent()
    {
        _validation.CheckNull(_channel, "has not been created!");

        Console.WriteLine(" [*] Awaiting for RPC requests.");
        _consumer = new EventingBasicConsumer(_channel);
        _consumer.Received += ConsumerReceivedEvent!;
    }

    protected override void ConsumerReceivedEvent(object sender, BasicDeliverEventArgs eventArgs)
    {
        string response = string.Empty;
        IModel channel = ((EventingBasicConsumer)sender!).Model;

        IBasicProperties replyProperties = channel.CreateBasicProperties();
        replyProperties.CorrelationId = eventArgs.BasicProperties.CorrelationId;

        try
        {
            byte[] body = eventArgs.Body.ToArray();
            string message = Encoding.UTF8.GetString(body);
            int number = int.Parse(message);
            Console.WriteLine($" [*] Calculate Fib({number})");
            response = Algorithm.Fib(number).ToString();
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine($"Error: {ex.Message}");
            Console.Error.WriteLine($"Stack Trace: {ex.StackTrace}");
        }
        finally
        {
            channel.BasicPublish(
                exchange: string.Empty, 
                routingKey: eventArgs.BasicProperties.ReplyTo, 
                basicProperties: replyProperties,
                body: Encoding.UTF8.GetBytes(response));

            channel.BasicAck(deliveryTag: eventArgs.DeliveryTag, multiple: false);
        }
    }
}