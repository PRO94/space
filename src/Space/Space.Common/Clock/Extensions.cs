using Microsoft.Extensions.DependencyInjection;
using Space.Common.Clock.Abstractions;

namespace Space.Common.Clock;

public static class Extensions
{
    public static IServiceCollection AddDateTimeProvider(this IServiceCollection services)
    {
        services.AddTransient<IDateTimeProvider, DateTimeProvider>();

        return services;
    }
}