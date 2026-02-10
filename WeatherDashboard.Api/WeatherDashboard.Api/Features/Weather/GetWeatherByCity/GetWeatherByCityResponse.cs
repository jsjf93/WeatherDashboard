namespace WeatherDashboard.Api.Features.Weather.GetWeatherByCity;

public sealed record GetWeatherByCityResponse(
    string City,
    double Temperature,
    string Condition,
    string Description,
    double Wind,
    int Humidity,
    double MinTemperature,
    double MaxTemperature,
    long Sunrise,
    long Sunset,
    int Timezone
);