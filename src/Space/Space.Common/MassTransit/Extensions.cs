using System.Reflection;
using GreenPipes;
using GreenPipes.Configurators;
using MassTransit;
using MassTransit.Definition;
using MassTransit.ExtensionsDependencyInjectionIntegration;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Space.Common.Settings;

namespace Space.Common.MassTransit;

public static class Extensions
{
    public static IServiceCollection AddMassTransitWithRabbitMq(
        this IServiceCollection services,
        Action<IRetryConfigurator> configureRetries = null)
    {
        services.AddMassTransit(configure =>
        {
            configure.AddConsumers(Assembly.GetEntryAssembly());

            configure.UsingSpaceInfrastructureRabbitMq(configureRetries);
        });

        services.AddMassTransitHostedService();

        return services;
    }

    public static void UsingSpaceInfrastructureRabbitMq(
        this IServiceCollectionBusConfigurator configure,
        Action<IRetryConfigurator> configureRetries = null)
    {
        configure.UsingRabbitMq((context, configurator) =>
        {
            var configuration = context.GetRequiredService<IConfiguration>();
            var serviceSettings = configuration.GetRequiredSection(nameof(ServiceSettings)).Get<ServiceSettings>();
            var rabbitMQSettings = configuration.GetRequiredSection(nameof(RabbitMQSettings)).Get<RabbitMQSettings>();

            configurator.Host(rabbitMQSettings.Host);

            configurator.ConfigureEndpoints(context, new KebabCaseEndpointNameFormatter(serviceSettings.ServiceName, false));

            if (configureRetries is null)
            {
                // If a message wasn't consumed by a consumer or there was an exception - it will retry 3 times with a delay of 5 seconds between this attempts
                configureRetries = (retryConfigurator) => retryConfigurator.Interval(retryCount: 3, interval: TimeSpan.FromSeconds(5));
            }

            configurator.UseMessageRetry(configureRetries);
        });
    }
}