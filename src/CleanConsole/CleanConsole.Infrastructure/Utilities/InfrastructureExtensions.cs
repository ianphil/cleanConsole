using CleanConsole.Infrastructure.Configuration;
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
    }
}
