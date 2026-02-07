using WeatherDashboard.Api.Models;

namespace WeatherDashboard.Api.Features.Weather.GetForecastByCity;

public sealed record GetForecastByCityResponse(IEnumerable<DailyForecast> DailySummaries);
