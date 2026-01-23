using FastEndpoints;
using WeatherDashboard.Api.Models;

namespace WeatherDashboard.Api.Features.Weather.GetForecastByCity;

public sealed class CondensedForecastMapper : Mapper<GetForecastByCityRequest, GetForecastByCityResponse, CondensedForecastData>
{
    public override GetForecastByCityResponse FromEntity(CondensedForecastData condensedForecast)
    {
        return new GetForecastByCityResponse
        {
            DailySummaries = condensedForecast.DailySummaries
        };
    }
}
