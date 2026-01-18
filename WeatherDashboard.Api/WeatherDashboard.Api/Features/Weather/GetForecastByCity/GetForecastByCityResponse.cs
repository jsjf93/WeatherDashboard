namespace WeatherDashboard.Api.Features.Weather.GetForecastByCity;

public sealed class GetForecastByCityResponse
{
    public List<DailyForecast> DailySummaries { get; set; } = new();
    public List<ForecastItem> FullForecast { get; set; } = new();
}

public sealed class DailyForecast
{
    public string Date { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public double Temp { get; set; }
    public double MinTemp { get; set; }
    public double MaxTemp { get; set; }
    public string Condition { get; set; } = string.Empty;
}

public sealed class ForecastItem
{
    public long Dt { get; set; }
    public string DtTxt { get; set; } = string.Empty;
    public double Temp { get; set; }
    public double FeelsLike { get; set; }
    public double TempMin { get; set; }
    public double TempMax { get; set; }
    public int Pressure { get; set; }
    public int Humidity { get; set; }
    public string Condition { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string Icon { get; set; } = string.Empty;
    public int Clouds { get; set; }
    public double WindSpeed { get; set; }
    public int WindDeg { get; set; }
    public double Pop { get; set; }
    public double? Rain3h { get; set; }
    public double? Snow3h { get; set; }
}
