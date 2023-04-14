using Common.Others;
using Common.Factories;
using Common.Intefaces;
using Common.Models;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RpcClient.Factories;
using RpcClient.Services;
using RpcClient;

try
{
    IHost host = CreateDefaultBuilder().Build();

    using IServiceScope serviceScope = host.Services.CreateScope();
    IServiceProvider provider = serviceScope.ServiceProvider;

    string number = "40";
    Console.WriteLine("RPC Client");

    var client = provider.GetRequiredService<RemoteProcedureCallClient>();
    Console.WriteLine($" [x] Requesting fib ({number})");

    string response = await client.CallAsync(number);
    Console.WriteLine($" [.] Got '{response}'");
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
            services.AddScoped(typeof(IRabbitMqConfigFactory<IRabbitMqConfig>), typeof(RabbitMqRpcClientConfigFactory));
            services.AddScoped<IRabbitMqConnectionFactory, RabbitMqConnectionFactory>();
            services.AddScoped<RabbitMqRpcClientSenderService>();
            services.AddScoped<RabbitMqRpcClientReceiverService>();
            services.AddScoped<RemoteProcedureCallClient>();
        });