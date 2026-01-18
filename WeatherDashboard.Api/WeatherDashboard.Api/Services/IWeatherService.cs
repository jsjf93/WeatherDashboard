using WeatherDashboard.Api.Models;

namespace WeatherDashboard.Api.Services;

public interface IWeatherService
{
    Task<WeatherResponse?> GetWeatherByCityAsync(string city, CancellationToken cancellationToken);
    Task<ForecastResponse?> GetForecastByCityAsync(string city, CancellationToken cancellationToken);
}
