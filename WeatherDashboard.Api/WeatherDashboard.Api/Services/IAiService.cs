using WeatherDashboard.Api.Models;

namespace WeatherDashboard.Api.Services;

public interface IAiService
{
    Task<string?> GenerateForecastSummaryAsync(string city, CondensedForecastData forecastData, CancellationToken cancellationToken);
}
