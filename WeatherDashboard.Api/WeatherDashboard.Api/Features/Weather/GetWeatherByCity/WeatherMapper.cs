using FastEndpoints;
using WeatherDashboard.Api.Models;

namespace WeatherDashboard.Api.Features.Weather.GetWeatherByCity;

public sealed class WeatherMapper : Mapper<GetWeatherByCityRequest, GetWeatherByCityResponse, WeatherResponse>
{
    public override GetWeatherByCityResponse FromEntity(WeatherResponse weatherResponse)
    {
        var firstWeather = weatherResponse.Weather.FirstOrDefault();

        return new GetWeatherByCityResponse(
            weatherResponse.Name,
            weatherResponse.Main.Temp,
            firstWeather?.Main ?? "Unknown",
            firstWeather?.Description ?? "No description",
            weatherResponse.Wind.Speed,
            weatherResponse.Main.Humidity,
            weatherResponse.Main.TempMin,
            weatherResponse.Main.TempMax
        );
    }
}
