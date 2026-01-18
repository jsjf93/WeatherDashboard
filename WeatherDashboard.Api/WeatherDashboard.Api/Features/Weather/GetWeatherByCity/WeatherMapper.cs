using FastEndpoints;
using WeatherDashboard.Api.Models;

namespace WeatherDashboard.Api.Features.Weather.GetWeatherByCity;

public sealed class WeatherMapper : Mapper<GetWeatherByCityRequest, GetWeatherByCityResponse, WeatherResponse>
{
    public override GetWeatherByCityResponse FromEntity(WeatherResponse e)
    {
        return new GetWeatherByCityResponse
        {
            Location = e.Name,
            Temperature = e.Main.Temp,
            Condition = e.Weather.FirstOrDefault()?.Main,
            Wind = e.Wind.Speed,
            Humidity = e.Main.Humidity
        };
    }
}
