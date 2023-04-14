using Common.Others;
using Common.Factories;
using Common.Intefaces;
using Common.Models;
using Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using PublishConfirmSender.Factories;
using PublishConfirmSender.Services;

try
{
    IHost host = CreateDefaultBuilder().Build();

    using IServiceScope serviceScope = host.Services.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;

    var service = provider.GetRequiredService<RabbitMqSenderAbstractService>();
    service.EstablishConnectionToService();
    service.EnablePublishConfirmed = true;
    service.DeclareQueue(queue: AppConstants.RoutingKey_Queue, durable: true, exclusive: false, autoDelete: false);
    service.CreateBasicProperties(p => p.Persistent = true);
    service.PublishMessage(exchange: string.Empty, routingKey: AppConstants.RoutingKey_Queue);
}
catch (Exception ex)
{
    Console.Error.WriteLine($"Error: {ex.Message}");
    Console.Error.WriteLine($"Stack Trace: {ex.StackTrace}");
}
finally
{
    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}

static IHostBuilder CreateDefaultBuilder() =>
    Host.CreateDefaultBuilder()
        .ConfigureAppConfiguration(app => app.AddJsonFile(AppConstants.AppSettingFile))
        .ConfigureServices(services =>
        {
            services.AddSingleton<IValidation, Validation>();
            services.AddScoped<IRabbitMqConfig, RabbitMqConfig>();
            services.AddScoped(typeof(IRabbitMqConfigFactory<IRabbitMqConfig>), typeof(RabbitMqPublishConfirmSenderConfigFactory));
            services.AddScoped<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
            services.AddScoped<RabbitMqSenderAbstractService, RabbitMqPublishConfirmSenderService>();
        });