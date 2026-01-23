using WeatherDashboard.Api.Models;

namespace WeatherDashboard.Api.Features.Weather.GetForecastByCity;

public sealed class GetForecastByCityResponse
{
    public List<DailyForecast> DailySummaries { get; set; } = new();
}
