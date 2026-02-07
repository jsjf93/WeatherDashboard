using FastEndpoints;
using WeatherDashboard.Api.Models;

namespace WeatherDashboard.Api.Features.Weather.GetWeatherByCity;

public sealed class WeatherMapper : Mapper<GetWeatherByCityRequest, GetWeatherByCityResponse, WeatherResponse>
{
    public override GetWeatherByCityResponse FromEntity(WeatherResponse weatherResponse) => new (
        weatherResponse.Name,
        weatherResponse.Main.Temp,
        weatherResponse.Weather.FirstOrDefault()?.Main ?? "Unknown",
        weatherResponse.Weather.FirstOrDefault()?.Description ?? "No description",
        weatherResponse.Wind.Speed,
        weatherResponse.Main.Humidity
    );
}
