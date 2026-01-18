namespace WeatherDashboard.Api.Features.Weather.GetWeatherByCity;

public sealed class GetWeatherByCityResponse
{
    public string Location { get; set; } = string.Empty;
    public double Temperature { get; set; }
    public string? Condition { get; set; }
    public double Wind { get; set; }
    public int Humidity { get; set; }
}