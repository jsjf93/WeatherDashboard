namespace WeatherDashboard.Api.Features.Weather.GetWeatherByCity;

public sealed record GetWeatherByCityResponse(
    string City,
    double Temperature,
    string Condition,
    string Description,
    double Wind,
    int Humidity
);