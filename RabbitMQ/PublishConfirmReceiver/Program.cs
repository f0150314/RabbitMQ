using Common.Factories;
using Common.Intefaces;
using Common.Models;
using Common.Others;
using Common.Services;
using PublishConfirmReceiver.Factories;
using PublishConfirmReceiver.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

try
{
    IHost host = CreateDefaultBuilder().Build();

    using IServiceScope serviceScope = host.Services.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;

    var service = provider.GetRequiredService<RabbitMqReceiverAbstractService>();
    service.EstablishConnectionToService();
    service.DeclareQueue(queue: AppConstants.RoutingKey_PublishConfirms, durable: true, exclusive: false, autoDelete: false);
    service.SetBasicQos(prefetchSize: 0, prefetchCount: 1, global: false);
    service.AddConsumerReceivedEvent();
    service.ConsumeMessages(queue: AppConstants.RoutingKey_PublishConfirms, autoAck: false);

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
            services.AddScoped(typeof(IRabbitMqConfigFactory<IRabbitMqConfig>), typeof(RabbitMqPublishConfirmReceiverConfigFactory));
            services.AddScoped<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
            services.AddScoped<RabbitMqReceiverAbstractService, RabbitMqPublishConfirmReceiverService>();
        });