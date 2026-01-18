namespace WeatherDashboard.Api.Features.Weather.GetWeatherByCity;

public sealed class GetWeatherByCityResponse
{
    public string City { get; set; } = string.Empty;
    public double Temperature { get; set; }
    public string Condition { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Wind { get; set; }
    public int Humidity { get; set; }
}