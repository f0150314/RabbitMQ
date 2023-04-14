using Common.Factories;
using Common.Intefaces;
using Common.Models;
using Common.Others;
using Common.Services;
using FanoutReceiver.Factories;
using FanoutReceiver.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;

try
{
    IHost host = CreateDefaultBuilder().Build();

    using IServiceScope serviceScope = host.Services.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;

    var service = provider.GetRequiredService<RabbitMqReceiverAbstractService>();
    service.EstablishConnectionToService();
    service.DeclareExchange(exchange: AppConstants.ExchangeKey_Fanout, type: ExchangeType.Fanout);

    string queueName = service.DeclareQueue().QueueName;
    service.BindQueue(queue: queueName, exchange: AppConstants.ExchangeKey_Fanout, routingKey: string.Empty);
    service.AddConsumerReceivedEvent();
    service.ConsumeMessages(queue: queueName, autoAck: true);

    await host.RunAsync();
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Error: {ex.Message}");
    Console.Error.WriteLine($"Stack Trace: {ex.StackTrace}");
}

static IHostBuilder CreateDefaultBuilder() =>
    Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(app => app.AddJsonFile(AppConstants.AppSettingFile))
        .ConfigureServices(services =>
        {
            services.AddSingleton<IValidation, Validation>();
            services.AddScoped<IRabbitMqConfig, RabbitMqConfig>();
            services.AddScoped(typeof(IRabbitMqConfigFactory<IRabbitMqConfig>), typeof(RabbitMqFanoutReceiverConfigFactory));
            services.AddScoped<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
            services.AddScoped<RabbitMqReceiverAbstractService, RabbitMqFanoutReceiverService>();
        });