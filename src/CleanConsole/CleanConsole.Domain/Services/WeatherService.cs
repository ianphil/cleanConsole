using CleanConsole.Models;
using CleanConsole.Models.Output;

namespace CleanConsole.Domain.Services;

public interface IWeatherService
{
    Task<OperationResult<WeatherDto>> GetWeatherDataAsync(Guid currentAccountId, double latitude, double logitude);
}

public class WeatherService : IWeatherService
{
    public Task<OperationResult<WeatherDto>> GetWeatherDataAsync(Guid currentAccountId, double latitude, double logitude)
    {
        throw new NotImplementedException();
    }
}