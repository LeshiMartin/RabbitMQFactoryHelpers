using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using static RabbitMQFactoryHelpers.DefaultConnectionValues;

namespace RabbitMQFactoryHelpers;

public static class ConsumerExtension
{
    public static IServiceCollection RegisterConsumer(this IServiceCollection services,
        string hostName = LOCALHOST_HOSTNAME,
        int port = LOCALHOST_PORT,
        string userName = LOCALHOST_USERNAME,
        string password = LOCALHOST_PASSWORD)
    {
        services.TryAddSingleton(_ =>
        {
            var connectionFactory = new ConnectionFactory
            {
                HostName = hostName,
                Port = port,
                UserName = userName,
                Password = password
            };
            return new PersistentConnection(connectionFactory);
        });
        return services;
    }

    public static IApplicationBuilder UseRabbitListener(this IApplicationBuilder app,
        params IConsumerChannel[] channels)
    {
        var listener = app.ApplicationServices.GetRequiredService<PersistentConnection>();
        var life = app.ApplicationServices.GetRequiredService<IHostApplicationLifetime>();
        OnStarted(listener, channels);
        life.ApplicationStopping.Register(() => OnStopping(listener));
        return app;
    }

    public static void UseRabbitListener(this IHost app,
        params IConsumerChannel[] channels)
    {
        var listener = app.Services.GetRequiredService<PersistentConnection>();
        var life = app.Services.GetRequiredService<IHostApplicationLifetime>();
        OnStarted(listener, channels);
        life.ApplicationStopping.Register(() => OnStopping(listener));
    }


    private static void OnStarted(PersistentConnection listener, params IConsumerChannel[] channels)
    {
        listener.CreateConsumerChannel(channels);
    }

    private static void OnStopping(PersistentConnection listener)
    {
        listener.Disconnect();
    }
}