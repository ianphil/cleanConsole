using CleanConsole.Domain.Services;
using Microsoft.Extensions.DependencyInjection;

namespace CleanConsole.Domain.Utilities;

public static class DomainExtensions
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddScoped<IWeatherService, WeatherService>();

        return services;
    }
}