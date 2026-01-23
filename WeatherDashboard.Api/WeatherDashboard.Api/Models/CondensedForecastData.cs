namespace WeatherDashboard.Api.Models;

public sealed class CondensedForecastData
{
    public List<DailyForecast> DailySummaries { get; set; } = [];
}

public sealed class DailyForecast
{
    public string Date { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public double Temp { get; set; }
    public double MinTemp { get; set; }
    public double MaxTemp { get; set; }
    public string Condition { get; set; } = string.Empty;
    public double FeelsLike { get; set; }
    public int Humidity { get; set; }
    public string Description { get; set; } = string.Empty;
    public double WindSpeed { get; set; }
}
