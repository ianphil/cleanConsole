using CleanConsole.Models;
using CleanConsole.Models.Output;
using CleanConsole.Entities;
using CleanConsole.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace CleanConsole.Domain.Services;

public interface IWeatherService
{
    Task<OperationResult<WeatherDto>> GetWeatherDataAsync(Guid currentAccountId, double latitude, double logitude);
}

internal class WeatherService : IWeatherService
{
    private readonly SimplifiedContext _dbContext;
    private readonly IWeatherApiClient _weatherApiClient;

    public WeatherService(SimplifiedContext dbContext, IWeatherApiClient weatherApiClient)
    {
        _dbContext = dbContext;
        _weatherApiClient = weatherApiClient;
    }

    public async Task<OperationResult<WeatherDto>> GetWeatherDataAsync(Guid currentAccountId, double latitude, double longitude)
    {
        if (!await _dbContext.Accounts.AnyAsync(x => x.Id == currentAccountId && x.Enabled))
        {
            return OperationResultStatus.Forbidden;
        }

        var existingData = await _dbContext.WeatherForecasts
            .SingleOrDefaultAsync(x => x.Id == currentAccountId && x.Latitude == latitude && x.Longitude == longitude);

        if (existingData != null)
        {
            return new WeatherDto
            {
                Latitude = latitude,
                Longitude = longitude,
                Temerature = existingData.Temperature,
                Unit = existingData.Unit
            };
        }

        var weatherAnalyticsData = await _weatherApiClient.GetWeatherDataAsync(latitude, longitude);

        if (weatherAnalyticsData == null)
            return new OperationResult<WeatherDto>(OperationResultStatus.NotFound, $"Weather data not found for the latitude {latitude} and longitude {longitude}");

        var weatherForecast = new WeatherForecast
        {
            AccountId = currentAccountId,
            Latitude = latitude,
            Longitude = longitude,
            Temperature = weatherAnalyticsData.Temperature,
            Unit = weatherAnalyticsData.Unit,
            Time = DateTimeOffset.UtcNow
        };

        _dbContext.WeatherForecasts.Add(weatherForecast);
        await _dbContext.SaveChangesAsync();

        return new WeatherDto
        {
            Latitude = latitude,
            Longitude = longitude,
            Temerature = weatherAnalyticsData.Temperature,
            Unit = weatherAnalyticsData.Unit
        };
    }
}