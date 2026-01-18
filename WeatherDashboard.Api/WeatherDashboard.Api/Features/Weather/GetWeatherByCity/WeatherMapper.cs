using FastEndpoints;
using WeatherDashboard.Api.Models;

namespace WeatherDashboard.Api.Features.Weather.GetWeatherByCity;

public sealed class WeatherMapper : Mapper<GetWeatherByCityRequest, GetWeatherByCityResponse, WeatherResponse>
{
    public override GetWeatherByCityResponse FromEntity(WeatherResponse e)
    {
        return new GetWeatherByCityResponse
        {
            City = e.Name,
            Temperature = e.Main.Temp,
            Condition = e.Weather.FirstOrDefault()?.Main ?? "Unknown",
            Description = e.Weather.FirstOrDefault()?.Description ?? "No description",
            Wind = e.Wind.Speed,
            Humidity = e.Main.Humidity
        };
    }
}
