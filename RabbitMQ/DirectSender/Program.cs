using Common.Others;
using Common.Factories;
using Common.Intefaces;
using Common.Models;
using Common.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using DirectSender.Factories;
using DirectSender.Services;
using RabbitMQ.Client;

try
{
    IHost host = CreateDefaultBuilder().Build();

    using IServiceScope serviceScope = host.Services.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;

    var service = provider.GetRequiredService<RabbitMqSenderAbstractService>();
    service.EstablishConnectionToService();
    service.DeclareExchange(exchange: AppConstants.ExchangeKey_Direct, type: ExchangeType.Direct);
    service.PublishMessage(exchange: AppConstants.ExchangeKey_Direct, routingKey: AppConstants.RoutingKey_Direct);
    service.PublishMessage(exchange: AppConstants.ExchangeKey_Direct, routingKey: "abc");
    service.PublishMessage(exchange: AppConstants.ExchangeKey_Direct, routingKey: "123");
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
            services.AddScoped(typeof(IRabbitMqConfigFactory<IRabbitMqConfig>), typeof(RabbitMqDirectSenderConfigFactory));
            services.AddScoped<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
            services.AddScoped<RabbitMqSenderAbstractService, RabbitMqDirectSenderService>();
        });