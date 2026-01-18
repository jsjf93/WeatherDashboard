namespace WeatherDashboard.Api.Services;

public interface IAiService
{
    Task<string?> GenerateForecastSummaryAsync(string city, object forecastData, CancellationToken cancellationToken);
}
