using CleanConsole.Infrastructure.Models;
using System.Net.Http.Json;

namespace CleanConsole.Infrastructure.Services;

public interface IWeatherApiClient
{
    Task<WeatherAnalyticsDto> GetWeatherDataAsync(double latitude, double longitude);
}

internal class WeatherApiClient : IWeatherApiClient
{
    private readonly HttpClient _httpClient;

    public WeatherApiClient(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<WeatherAnalyticsDto> GetWeatherDataAsync(double latitude, double longitude)
    {
        return (await _httpClient.GetFromJsonAsync<WeatherAnalyticsDto>($"/something-something?lat={latitude}&long={longitude}"))!;
    }
}
