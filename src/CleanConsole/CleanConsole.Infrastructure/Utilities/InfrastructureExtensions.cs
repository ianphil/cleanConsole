using CleanConsole.Infrastructure.Configuration;
using CleanConsole.Infrastructure.Services;
using CleanConsole.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http.Headers;

namespace CleanConsole.Infrastructure.Utilities;

public static class InfrastructureExtensions
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        var dbOptions = configuration.GetRequiredSection("Database").Get<WeatherDbOptions>();
        dbOptions.Validate();

        services.AddDbContext<SimplifiedContext>(opt =>
        {
            var connectionString = $"Server={dbOptions.Server};Database={dbOptions.Database};Username={dbOptions.Username};Password={dbOptions.Password}";
            opt.UseSqlServer(connectionString);
        });

        var weatherApiOptions = configuration.GetRequiredSection("WeatherApi").Get<WeatherApiOptions>();
        weatherApiOptions.Validate();

        services.AddHttpClient<IWeatherApiClient, WeatherApiClient>(nameof(IWeatherApiClient), client =>
        {
            client.BaseAddress = new Uri(weatherApiOptions.BaseUrl);
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", weatherApiOptions.Token);
        });

        return services;
    }
}
